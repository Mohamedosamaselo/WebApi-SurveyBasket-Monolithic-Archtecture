using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasketWebApi.Authentication;
using SurveyBasketWebApi.Entities;
using SurveyBasketWebApi.Services;
using System.Reflection;

namespace SurveyBasketWebApi;

public static class DependencyInjection
{
    public static void AddWebApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureControllers(services);

        ConfigureValidation(services);

        ConfigureSwagger(services);

        ConfigureApplicationServices(services);

        ConfigureDatabase(services, configuration);

        ConfigureIdentity(services);
    }

    // ---------------------------
    // Controllers
    // ---------------------------

    private static void ConfigureControllers(
        IServiceCollection services)
    {
        services.AddControllers();

        services.AddOpenApi();
    }

    // ---------------------------
    // FluentValidation
    // ---------------------------

    private static void ConfigureValidation(
        IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();
    }

    // ---------------------------
    // Swagger
    // ---------------------------

    private static void ConfigureSwagger(
        IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    // ---------------------------
    // Application Services
    // ---------------------------

    private static void ConfigureApplicationServices(
        IServiceCollection services)
    {
        services.AddScoped<IPollService, PollService>();
    }

    // ---------------------------
    // Database
    // ---------------------------

    private static void ConfigureDatabase(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });
    }

    // ---------------------------
    // ASP.NET Core Identity
    // ---------------------------

    private static void ConfigureIdentity(
        IServiceCollection services)
    {
        services
            //.AddIdentityApiEndpoints<ApplicationUser>()
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        //.AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    // Set the signing key here (you should use a secure key)
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("QayFdiBLLPPJP3KdUxmorUE4U5jMmopkjmZYx3L2wn8")),
                    ValidIssuer = "SurveyBasket",
                    ValidAudience = "SurveyBasketUsers",
                };
            })
            ;

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddScoped<IAuthService, AuthService>();
    }
}