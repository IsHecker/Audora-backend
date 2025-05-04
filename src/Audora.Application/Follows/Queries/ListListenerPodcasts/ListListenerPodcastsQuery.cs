using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Follows.Queries.ListListenerPodcasts;

public record ListListenerPodcastsQuery(Guid ListenerId) : IQuery<IEnumerable<Podcast>>;

public class ListListenerPodcastsQueryHandler : IQueryHandler<ListListenerPodcastsQuery, IEnumerable<Podcast>>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IFollowRepository _followRepository;

    public ListListenerPodcastsQueryHandler(IFollowRepository followRepository, IPodcastRepository podcastRepository)
    {
        _followRepository = followRepository;
        _podcastRepository = podcastRepository;
    }


    public async Task<Result<IEnumerable<Podcast>>> Handle(ListListenerPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var follows = (await _followRepository.GetUserFollows(request.ListenerId))
            .Where(f => f.FollowTarget == FollowTarget.Podcast).Select(f => f.EntityId);

        var podcasts = await _podcastRepository.GetPodcastsAsync();
        
        // TODO check for errors.

        return podcasts.Where(podcast => follows.Contains(podcast.Id)).ToResult<IEnumerable<Podcast>>();
    }
}