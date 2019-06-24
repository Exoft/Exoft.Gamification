using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Helpers;
using Exoft.Gamification.Api.Resources;
using Exoft.Gamification.Api.Services;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;

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
            services.AddCors();
            services.AddMvc()
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(ValidatorMessages));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation();

            services.AddDbContext<UsersDbContext>
            (
                options => options.UseSqlServer(Configuration.GetConnectionString("DataConnection"))
            );

            // configure DI for application services
            var jwtSecret = new JwtSecret(Configuration);
            services.AddScoped<IJwtSecret, JwtSecret>(s => jwtSecret);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserAchievementService, UserAchievementService>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAchievementRepository, AchievementRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IUserAchievementRepository, UserAchievementRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            // Validators
            services.AddTransient<IValidator<CreateUserModel>, CreateUserModelValidator>();
            services.AddTransient<IValidator<UpdateUserModel>, UpdateUserModelValidator>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret.Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                //var context = scope.ServiceProvider.GetService<UsersDbContext>();
                //ContextInitializer.Initialize(context);
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
