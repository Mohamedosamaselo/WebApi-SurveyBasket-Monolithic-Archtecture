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
using System.Text;

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

        ConfigureAuthentication(services, configuration);
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

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        //configure Identity Package with ApplicationUser and IdentityRole
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName)); // Bind JwtOptions without validation from configuration
        services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations() // Validate the options using data annotations
                .ValidateOnStart(); // Validate the options on application startup

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>(); // get jwtSettings

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
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                };
            })
            ;

        services.AddSingleton<IJwtProvider, JwtProvider>(); // conf JwtProvider as singleton because it doesn't have any state and can be shared across requests

        services.AddScoped<IAuthService, AuthService>();
    }
}