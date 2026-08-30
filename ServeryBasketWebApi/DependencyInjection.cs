using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasketWebApi.Interfaces;
using SurveyBasketWebApi.Persistence;
using SurveyBasketWebApi.Services;
using System.Reflection;

namespace SurveyBasketWebApi;

public static class DependencyInjection
{
    public static void AddWebApiDependencies(this IServiceCollection Services, IConfiguration configuration)
    {
        Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        Services.AddOpenApi();

        Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()).AddFluentValidationAutoValidation();

        // swagger
        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();

        Services.AddScoped<IPollService, PollService>();

        Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}