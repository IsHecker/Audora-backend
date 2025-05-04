using Audora.Domain.Entities;

namespace Audora.Application.Search;

public class SearchResults
{
    public IEnumerable<Podcast> Podcasts { get; init; } = null!;
    public IEnumerable<Episode> Episodes { get; init; } = null!;
}