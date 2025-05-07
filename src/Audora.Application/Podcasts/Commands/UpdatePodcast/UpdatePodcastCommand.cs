using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Commands.UpdatePodcast;

public record UpdatePodcastCommand(Guid PodcastId, Podcast Podcast) : ICommand;

public class UpdatePodcastCommandHandler : ICommandHandler<UpdatePodcastCommand>
{
    private readonly IPodcastRepository _podcastRepository;

    public UpdatePodcastCommandHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result> Handle(UpdatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository.GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        podcast.Update(request.Podcast);

        return Result.Success;
    }
}