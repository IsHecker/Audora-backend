using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audora.Infrastructure.Configurations;

public class PlaylistEpisodeConfiguration : IEntityTypeConfiguration<PlaylistEpisode>
{
    public void Configure(EntityTypeBuilder<PlaylistEpisode> builder)
    {
        builder.HasKey(pe => new { pe.PlaylistId, pe.EpisodeId });

        builder
            .HasOne(pe => pe.Playlist)
            .WithMany(p => p.PlaylistEpisodes)
            .HasForeignKey(pe => pe.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(pe => pe.Episode)
            .WithMany() // or .WithMany(e => e.PlaylistEpisodes) if needed
            .HasForeignKey(pe => pe.EpisodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}