using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audora.Infrastructure.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasOne(ep => ep.AudioFile)
            .WithOne()
            .HasForeignKey<Episode>(es => es.AudioFileId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}