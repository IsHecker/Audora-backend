using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using MediatR;

namespace Audora.Application.Episodes.Commands.DeleteEpisode;

public record DeleteEpisodeCommand(Guid EpisodeId) : ICommand;

public class DeleteEpisodeCommandHandler : ICommandHandler<DeleteEpisodeCommand>
{
    private readonly IEpisodeRepository _episodeRepository;

    public DeleteEpisodeCommandHandler(IEpisodeRepository episodeRepository)
    {
        _episodeRepository = episodeRepository;
    }

    public async Task<Result> Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _episodeRepository.DeleteAsync(request.EpisodeId);

        if (!isDeleted)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }
        
        return Result.Success;
    }
}