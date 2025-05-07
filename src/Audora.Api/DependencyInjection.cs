using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        
        services.Configure<JsonOptions>(opts =>
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        
        return services;
    }
}