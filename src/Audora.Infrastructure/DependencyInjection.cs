using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Infrastructure.Identity;
using Audora.Infrastructure.Repositories;
using Audora.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

        services.AddMemoryCache();

        services.ConfigureApplicationCookie(opts =>
        {
            opts.Events.OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opts =>
            {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.User.RequireUniqueEmail = true;
                opts.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentStatRepository, CommentStatRepository>();
        services.AddScoped<IEpisodeRepository, EpisodeRepository>();
        services.AddScoped<IEpisodeStatRepository, EpisodeStatRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<IReactionStatRepository, ReactionStatsRepository>();
        services.AddScoped<IPlaybackSessionRepository, PlaybackSessionRepository>();
        services.AddScoped<IPodcastRatingRepository, PodcastRatingRepository>();
        services.AddScoped<IPodcastRepository, PodcastRepository>();
        services.AddScoped<IPodcastStatRepository, PodcastStatRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<IPlaylistEpisodeRepository, PlaylistEpisodeRepository>();
        services.AddScoped<IAudioFileRepository, AudioFileRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserDeletionService, UserDeletionService>();
        services.AddScoped<IUserSignInService, UserSignInService>();
        services.AddSingleton<IAuthResultStore, AuthResultStore>();
        services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
        services.AddSingleton<TokenGeneratorService>();

        return services;
    }
}