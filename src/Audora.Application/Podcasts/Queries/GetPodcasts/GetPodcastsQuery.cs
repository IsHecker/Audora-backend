using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Queries.GetPodcasts;

public record GetPodcastsQuery() : IQuery<IEnumerable<Podcast>>;

public class GetPodcastsQueryHandler : IQueryHandler<GetPodcastsQuery, IEnumerable<Podcast>>
{
    public async Task<Result<IEnumerable<Podcast>>> Handle(GetPodcastsQuery request, CancellationToken cancellationToken)
    {
        // TODO Get trending/featured/new podcasts
        // filter parameter (e.g., type = "trending" | "new" | "featured").
        throw new NotImplementedException();
    }
}