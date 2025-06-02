using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Commands.CreatePodcast;

public record CreatePodcastCommand(Podcast Podcast, IEnumerable<Episode>? Episodes) : ICommand<PodcastResponse>;

public class CreatePodcastCommandHandler : ICommandHandler<CreatePodcastCommand, PodcastResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;

    public CreatePodcastCommandHandler(
        IPodcastRepository podcastRepository,
        IPodcastStatRepository podcastStatRepository)
    {
        _podcastRepository = podcastRepository;
        _podcastStatRepository = podcastStatRepository;
    }

    public async Task<Result<PodcastResponse>> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcast = request.Podcast;

        foreach (var episode in request.Episodes ?? Enumerable.Empty<Episode>())
        {
            podcast.AddEpisode(episode);
        }

        await _podcastRepository.AddAsync(podcast);
        await _podcastStatRepository.AddAsync(new PodcastStat(podcast.Id, podcast.Name));

        return podcast.ToResponse();
    }
}