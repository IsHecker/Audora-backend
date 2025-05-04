using Audora.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Audora.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            options.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
        });

        return services;
    }
}