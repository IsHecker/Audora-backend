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
    public DbSet<EngagementStat> EngagementStats { get; set; }
    public DbSet<EpisodeStat> EpisodeStats { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistEpisode> PlaylistEpisodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>().HasNoKey();

        modelBuilder.Entity<Episode>()
            .HasMany(e => e.Playlists)
            .WithMany(p => p.Episodes)
            .UsingEntity<PlaylistEpisode>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}