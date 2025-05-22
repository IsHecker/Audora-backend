using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;

namespace Audora.Application.Podcasts.Commands.DeletePodcast;

public record DeletePodcastCommand(Guid PodcastId) : ICommand;

public class DeletePodcastCommandHandler : ICommandHandler<DeletePodcastCommand>
{
    private readonly IPodcastRepository _podcastRepository;

    public DeletePodcastCommandHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result> Handle(DeletePodcastCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _podcastRepository.DeleteAsync(request.PodcastId);

        return isDeleted
            ? Result.Success
            : Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
    }
}