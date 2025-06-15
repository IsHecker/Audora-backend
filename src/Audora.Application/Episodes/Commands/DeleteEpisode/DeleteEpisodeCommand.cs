using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Domain.Common.Enums;
using MediatR;

namespace Audora.Application.Episodes.Commands.DeleteEpisode;

public record DeleteEpisodeCommand(Guid EpisodeId) : ICommand;

public class DeleteEpisodeCommandHandler : ICommandHandler<DeleteEpisodeCommand>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly EngagementStatsService _engagementStatsService;

    public DeleteEpisodeCommandHandler(IEpisodeRepository episodeRepository, EngagementStatsService engagementStatsService)
    {
        _episodeRepository = episodeRepository;
        _engagementStatsService = engagementStatsService;
    }

    public async Task<Result> Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _episodeRepository.DeleteAsync(request.EpisodeId);

        if (!isDeleted)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }

        var deleteResult = await _engagementStatsService.DeleteStatsAsync(request.EpisodeId, EntityType.Episode);

        if (deleteResult.IsError)
            return deleteResult.Errors;

        return Result.Success;
    }
}