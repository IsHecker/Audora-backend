using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Infrastructure.Repositories;
using Audora.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Audora.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));


        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IEngagementStatRepository, EngagementStatRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        services.AddScoped<IEpisodeStatRepository, EpisodeStatRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IPlaybackSessionRepository, PlaybackSessionRepository>();
        services.AddScoped<IPodcastRatingRepository, PodcastRatingRepository>();
        services.AddScoped<IPodcastRepository, PodcastRepository>();
        services.AddScoped<IPodcastStatRepository, PodcastStatRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}