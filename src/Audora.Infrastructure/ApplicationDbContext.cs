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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>().HasNoKey();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}