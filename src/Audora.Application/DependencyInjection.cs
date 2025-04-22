using Microsoft.Extensions.DependencyInjection;

namespace Audora.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}