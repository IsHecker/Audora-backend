using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audora.Infrastructure.Configurations;

public class PodcastConfiguration : IEntityTypeConfiguration<Podcast>
{
    public void Configure(EntityTypeBuilder<Podcast> builder)
    {
    }
}