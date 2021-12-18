using Clinic.Domain.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Clinic.Core.CustomMiddlewares;
using Clinic.Core.EmailSenders;
using Clinic.Core.EmailSenders.Interfaces;
using Clinic.Core.Helpers;
using Clinic.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Clinic.API
{
    public class Startup
    {
        private readonly string _allowSpecificOrigins = "Allow4200";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic.API", Version = "v1" });
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please enter token: ",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.Http,
                //    BearerFormat = "JWT",
                //    Scheme = "bearer"
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {

                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            },

                //        },
                //        new string[] { }
                //    }

                //});
            });


            #region DbContext

            services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ClinicConnection")));

            #endregion


            #region AutoMapper

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion


            #region Identity

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ClinicDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<Role>();


            services.Configure<IdentityOptions>(options =>
            {
                //User Settings
                options.User.RequireUniqueEmail = true;

                //Password Settings
                options.Password.RequireNonAlphanumeric = false;
            });

            #endregion

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPanel", policy =>
                {
                    policy.Requirements.Add(new AdminPanelAccessRequirement("WebsiteStaff"));
                });
            });

            #region Cors

            services.AddCors(options =>
            {
                options.AddPolicy(name: _allowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                    });
            });

            #endregion


            #region IoC

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IVisitRepository, VisitRepository>();
            services.AddTransient<IUserManagerRepository, UserManagerRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IEmailSender, GMailSender>();
            services.AddSingleton<IAuthorizationHandler, AdminPanelAccessHandler>();

            #endregion


            #region JWT

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = "JwtBearer";
            //        options.DefaultChallengeScheme = "JwtBearer";
            //    })
            //    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
            //    {
            //        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey =
            //                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSoDoNotTellToAnyone")),
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.FromMinutes(5)
            //        };
            //    });

            #endregion
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinic.API v1"));
            }




            app.UseCustomResponseCode();


            app.UseHttpsRedirection();


            app.UseSerilogRequestLogging();


            app.UseRouting();


            if (env.IsDevelopment())
                app.UseCors(_allowSpecificOrigins);


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
