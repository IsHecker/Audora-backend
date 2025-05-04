using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Podcasts.Commands.CreatePodcast;

public record CreatePodcastCommand(Podcast Podcast, ICollection<Episode>? Episodes) : ICommand<Podcast>;

public class CreatePodcastCommandHandler : ICommandHandler<CreatePodcastCommand, Podcast>
{
    private readonly IPodcastRepository _podcastRepository;

    public CreatePodcastCommandHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result<Podcast>> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcast = request.Podcast;

        foreach (var episode in request.Episodes ?? Enumerable.Empty<Episode>())
        {
            podcast.AddEpisode(episode);
        }

        await _podcastRepository.AddAsync(podcast);

        return podcast;
    }
}