using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audora.Infrastructure.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasMany(e => e.Playlists)
            .WithMany(p => p.Episodes)
            .UsingEntity<PlaylistEpisode>();
    }
}