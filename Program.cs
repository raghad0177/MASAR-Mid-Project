using MASAR.Data;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;

namespace MASAR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Get the connection string settings
            string ConnectionStringVar = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MASARDBContext>(op => op.UseSqlServer(ConnectionStringVar));
            // Add Identity Service
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MASARDBContext>();
            builder.Services.AddHttpContextAccessor();
            //Add The Services & Interfaces Access
            builder.Services.AddScoped<IUser, IdentityAccountService>();
            builder.Services.AddScoped<IAdmin, Admin>();
            builder.Services.AddScoped<IDriver,Driver>();
            builder.Services.AddScoped<IStudent, Student>();
            builder.Services.AddScoped<IBus, BusService>();
            builder.Services.AddScoped<JwtTokenService>();
            // add auth service to the app using jwt
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = JwtTokenService.ValidateToken(builder.Configuration);
                });
            builder.Services.AddAuthorization(options =>
            {
                // Role Based Policy
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireDriverRole", policy => policy.RequireRole("Driver"));
                options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));
                options.AddPolicy("RequireAdminStudentRole", policy => policy.RequireRole("Admin", "Student"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("Admin","Driver","Student"));
            });

            // Swagger Configration
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("MASARApi", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "MASAR API",
                    Version = "v1",
                    Description = "This API developed as part of the MASAR Platform project, is designed to manage various entities."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter user token below."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Call Swagger Service
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/{documentName}/swagger.json";
            });

            // Call Swagger UI
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/MASARApi/swagger.json", "MASAR API v1");
                options.RoutePrefix = "";
            });

            app.MapControllers();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}