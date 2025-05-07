using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Commands.CreateEpisode;

public record CreateEpisodeCommand(Guid PodcastId, Episode Episode) : ICommand;

public class CreateEpisodeCommandHandler : ICommandHandler<CreateEpisodeCommand>
{
    private readonly IPodcastRepository _podcastRepository;

    public CreateEpisodeCommandHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result> Handle(CreateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository.GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        podcast.AddEpisode(request.Episode);

        return Result.Success;
    }
}