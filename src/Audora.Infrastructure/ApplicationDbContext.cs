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
            
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}