using JobPortal1.O.DTOs;
using JobPortal1.O.Repositories.Implementation;
using JobPortal1.O.Repositories.Interface;
using JobPortal1.O.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JobPortal1.O
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ 1. Register Services
            RegisterServices(builder);

            var app = builder.Build();

            // ✅ 2. Configure Middleware
            ConfigureMiddleware(app);

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            // ✅ Authentication Setup (JWT)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            // ✅ Authorization Setup
            builder.Services.AddAuthorization();

            // ✅ Register Repositories
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

            // ✅ Register Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ApplicationsDetailService>();

            // ✅ Register DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ Register Controllers
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true; // For better JSON formatting
                })
                .AddApplicationPart(typeof(RegisterRequest).Assembly);

            // ✅ Swagger Setup
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JobPortal API",
                    Version = "v1"
                });

                // ✅ Enable JWT in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by the token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            // ✅ Environment-Specific Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ✅ JWT Authentication Middleware
            app.UseAuthentication();

            // ✅ Authorization Middleware
            app.UseAuthorization();

            // ✅ Map Controllers
            app.MapControllers();
        }
    }
}
