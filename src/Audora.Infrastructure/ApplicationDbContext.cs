using System.Reflection;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Audora.Infrastructure.Configurations;
using Audora.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Podcast> Podcasts { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<PodcastRating> PodcastRatings { get; set; }
    public DbSet<PodcastStat> PodcastStats { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<ReactionStat> ReactionStats { get; set; }
    public DbSet<EpisodeStat> EpisodeStats { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistEpisode> PlaylistEpisodes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentStat> CommentStats { get; set; }
    public DbSet<PlaybackSession> PlaybackSessions { get; set; }
    public DbSet<AudioFile> AudioFiles { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Tag>().HasNoKey();
        builder.Entity<CommentStat>().HasNoKey();
        builder.Entity<ReactionStat>().HasNoKey();


        builder.Entity<CommentStat>().HasKey(cs => new { cs.EntityId, cs.EntityType });
        builder.Entity<ReactionStat>().HasKey(rs => new { rs.EntityId, rs.EntityType, rs.ReactionType });

        builder.Entity<PodcastRating>()
            .HasOne<Podcast>()
            .WithOne()
            .HasForeignKey<PodcastRating>(pr => pr.PodcastId);

        builder.Entity<PlaybackSession>()
            .HasOne<Episode>()
            .WithOne()
            .HasForeignKey<PlaybackSession>(ps => ps.EpisodeId);


        builder.Entity<EpisodeStat>()
            .HasOne<Podcast>()
            .WithMany()
            .HasForeignKey(es => es.PodcastId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<PlaybackSession>()
            .HasIndex(p => new { p.EpisodeId, p.ListenerId, p.LastPlayedAt });

        builder.ApplyUserIdConvention();

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}