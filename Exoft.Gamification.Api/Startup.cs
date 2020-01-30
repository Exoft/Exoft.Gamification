using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Data.Seeds;
using Exoft.Gamification.Api.Helpers;
using Exoft.Gamification.Api.Resources;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

namespace Exoft.Gamification
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@".\server\share\"));

            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add<ErrorHandlingFilter>();
            })
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(ValidatorMessages));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<UsersDbContext>
            (
                options => options.UseSqlServer(Configuration.GetConnectionString("DataConnection"))
            );

            // configure DI for application services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IRefreshTokenProvider, RefreshTokenProvider>();
            services.AddTransient(typeof(ICacheManager<>), typeof(CacheManager<>));

            // Settings
            var jwtSecret = new JwtSecret(Configuration);
            services.AddScoped<IJwtSecret, JwtSecret>(s => jwtSecret);
            services.AddScoped<IEmailSenderSettings, EmailSenderSettings>();
            services.AddScoped<IResetPasswordSettings, ResetPasswordSettings>();
            services.AddScoped<IAppCenterSettings, AppCenterSettings>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserAchievementService, UserAchievementService>();
            services.AddScoped<IThankService, ThankService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRequestAchievementService, RequestAchievementService>();
            services.AddScoped<IReferenceBookService, ReferenceBookService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAchievementRepository, AchievementRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IUserAchievementRepository, UserAchievementRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IThankRepository, ThankRepository>();
            services.AddTransient<IRequestAchievementRepository, RequestAchievementRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IChapterRepository, ChapterRepository>();

            // Validators
            services.AddTransient<IValidator<CreateUserModel>, CreateUserModelValidator>();
            services.AddTransient<IValidator<UpdateFullUserModel>, UpdateFullUserModelValidator>();
            services.AddTransient<IValidator<UpdateUserModel>, UpdateUserModelValidator>();
            services.AddTransient<IValidator<CreateAchievementModel>, CreateAchievementModelValidator>();
            services.AddTransient<IValidator<UpdateAchievementModel>, UpdateAchievementModelValidator>();
            services.AddTransient<IValidator<CreateThankModel>, CreateThankModelValidator>();
            services.AddTransient<IValidator<ResetPasswordModel>, ResetPasswordModelValidator>();
            services.AddTransient<IValidator<RequestResetPasswordModel>, RequestResetPasswordModelValidator>();
            services.AddTransient<IValidator<RequestAchievementModel>, RequestAchievementModelValidator>();
            services.AddTransient<IValidator<PushRequestModel>, PushRequestModelValidator>();
            services.AddTransient<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Cache
            services.AddDistributedMemoryCache();

            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret.Secret),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();

            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Gamification", Version = "0.0.0.1" });
                
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<UsersDbContext>())
                {
                    ContextInitializer.Initialize(context);
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamification.Api");
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
