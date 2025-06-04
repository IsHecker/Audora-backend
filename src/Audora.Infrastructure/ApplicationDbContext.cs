using System.Reflection;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>().HasNoKey();
        modelBuilder.Entity<CommentStat>().HasNoKey();
        modelBuilder.Entity<ReactionStat>().HasNoKey();


        modelBuilder.Entity<CommentStat>().HasKey(cs => new { cs.EntityId, cs.EntityType });
        modelBuilder.Entity<ReactionStat>().HasKey(rs => new { rs.EntityId, rs.EntityType, rs.ReactionType });

        modelBuilder.Entity<PodcastRating>()
            .HasOne<Podcast>()
            .WithOne()
            .HasForeignKey<PodcastRating>(pr => pr.PodcastId);

        modelBuilder.Entity<PlaybackSession>()
            .HasOne<Episode>()
            .WithOne()
            .HasForeignKey<PlaybackSession>(ps => ps.EpisodeId);


        modelBuilder.Entity<EpisodeStat>()
            .HasOne<Podcast>()
            .WithMany()
            .HasForeignKey(es => es.PodcastId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PlaybackSession>()
            .HasIndex(p => new { p.EpisodeId, p.ListenerId, p.LastPlayedAt });



        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}