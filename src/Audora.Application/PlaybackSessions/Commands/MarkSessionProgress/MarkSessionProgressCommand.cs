using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Contracts.PlaybackSessions.Requests;
using MediatR;

namespace Audora.Application.PlaybackSessions.Commands.MarkSessionProgress;

public record MarkSessionProgressCommand(Guid ListenerId, Guid EpisodeId, MarkSessionProgressRequest SessionProgress) : ICommand;

public class MarkSessionProgressHandler : ICommandHandler<MarkSessionProgressCommand>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;

    public MarkSessionProgressHandler(IPlaybackSessionRepository playbackSessionRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
    }

    public async Task<Result> Handle(MarkSessionProgressCommand request, CancellationToken cancellationToken)
    {
        var session = await _playbackSessionRepository.GetAsync(request.ListenerId, request.EpisodeId);
        if (session is null)
        {
            return Error.NotFound(description: $"PlaybackSession with EpisodeId '{request.EpisodeId}' is not found.");
        }
        
        var sessionProgress = request.SessionProgress;
        session.MarkProgress(sessionProgress.PlaybackPosition, sessionProgress.ListenedDuration, sessionProgress.IsCompleted);
        
        return Result.Success;
    }
}