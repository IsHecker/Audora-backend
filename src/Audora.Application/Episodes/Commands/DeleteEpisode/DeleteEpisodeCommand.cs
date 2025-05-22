using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using MediatR;

namespace Audora.Application.Episodes.Commands.DeleteEpisode;

public record DeleteEpisodeCommand(Guid EpisodeId) : ICommand;

public class DeleteEpisodeCommandHandler : ICommandHandler<DeleteEpisodeCommand>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public DeleteEpisodeCommandHandler(IEpisodeRepository episodeRepository,
        IEngagementStatRepository engagementStatRepository)
    {
        _episodeRepository = episodeRepository;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result> Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _episodeRepository.DeleteAsync(request.EpisodeId);

        if (!isDeleted)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }

        await _engagementStatRepository.DeleteByEntityIdAsync(request.EpisodeId);

        return Result.Success;
    }
}