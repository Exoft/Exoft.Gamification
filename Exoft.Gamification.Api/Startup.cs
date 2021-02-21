using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Common.Models.Order;
using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Common.Models.RequestOrder;
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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exoft.Gamification.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@".\Resources\"));

            services.AddCors();
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = true;
                options.Filters.Add<ErrorHandlingFilterAttribute>();
            })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(ValidatorMessages));
            })
            .AddNewtonsoftJson();

            // SQL Server.
            services.AddDbContext<GamificationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));
            
            // SQLite.
            // services.AddDbContext<GamificationDbContext>(
            //     options => options.UseSqlite(Configuration.GetConnectionString("DataConnection_SQLite")));

            // configure DI for application services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ContextInitializer>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IRefreshTokenProvider, RefreshTokenProvider>();
            services.AddTransient(typeof(ICacheManager<>), typeof(CacheManager<>));

            // Settings
            var jwtSecret = new JwtSecret(Configuration);
            services.AddScoped<IJwtSecret, JwtSecret>(s => jwtSecret);
            services.AddScoped<IEmailSenderSettings, EmailSenderSettings>();
            services.AddScoped<IResetPasswordSettings, ResetPasswordSettings>();

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
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRequestOrderService, RequestOrderService>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAchievementRepository, AchievementRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IUserAchievementRepository, UserAchievementRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IThankRepository, ThankRepository>();
            services.AddTransient<IRequestAchievementRepository, RequestAchievementRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRequestOrderRepository, RequestOrderRepository>();

            // Validators
            services.AddTransient<IValidator<CreateUserModel>, CreateUserModelValidator>();
            services.AddTransient<IValidator<UpdateFullUserModel>, UpdateFullUserModelValidator>();
            services.AddTransient<IValidator<UpdateUserModel>, UpdateUserModelValidator>();
            services.AddTransient<IValidator<CreateAchievementModel>, CreateAchievementModelValidator>();
            services.AddTransient<IValidator<UpdateAchievementModel>, UpdateAchievementModelValidator>();
            services.AddTransient<IValidator<CreateThankModel>, CreateThankModelValidator>();
            services.AddTransient<IValidator<ResetPasswordModel>, ResetPasswordModelValidator>();
            services.AddTransient<IValidator<RequestResetPasswordModel>, RequestResetPasswordModelValidator>();
            services.AddTransient<IValidator<CreateRequestAchievementModel>, CreateRequestAchievementModelValidator>();
            services.AddTransient<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();
            services.AddTransient<IValidator<PagingInfo>, PagingInfoValidator>();
            services.AddTransient<IValidator<CreateOrderModel>, CreateOrderModelValidator>();
            services.AddTransient<IValidator<UpdateOrderModel>, UpdateOrderModelValidator>();
            services.AddTransient<IValidator<CreateCategoryModel>, CreateCategoryModelValidator>();
            services.AddTransient<IValidator<UpdateCategoryModel>, UpdateCategoryModelValidator>();
            services.AddTransient<IValidator<CreateRequestOrderModel>, CreateRequestOrderModelValidator>();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gamification", Version = "0.0.0.1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var contextInitializer = scope.ServiceProvider.GetService<ContextInitializer>();

                contextInitializer.InitializeAsync().GetAwaiter().GetResult();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamification.Api");
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
