using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Contracts.PlaybackSessions.Requests;

namespace Audora.Application.PlaybackSessions.Commands.MarkPlaybackSessionProgress;

public record MarkPlaybackSessionProgressCommand(Guid PlaybackSessionId, MarkSessionProgressRequest SessionProgress) : ICommand;

public class MarkPlaybackSessionProgressHandler : ICommandHandler<MarkPlaybackSessionProgressCommand>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;
    private readonly IEpisodeRepository _episodeRepository;

    public MarkPlaybackSessionProgressHandler(
        IPlaybackSessionRepository playbackSessionRepository,
        IEpisodeRepository episodeRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
        _episodeRepository = episodeRepository;
    }

    public async Task<Result> Handle(MarkPlaybackSessionProgressCommand request, CancellationToken cancellationToken)
    {
        var session = await _playbackSessionRepository.AsTracking().GetByIdAsync(request.PlaybackSessionId);
        if (session is null)
        {
            return Error.NotFound(description: $"PlaybackSession with SessionId '{request.PlaybackSessionId}' is not found.");
        }

        var episodeDuration = await GetEpisodeDurationAsync(session.EpisodeId);

        var sessionProgress = request.SessionProgress;
        session.MarkProgress(sessionProgress.PlaybackPosition, sessionProgress.ListenedDuration, episodeDuration);

        return Result.Success;
    }

    private async Task<long> GetEpisodeDurationAsync(Guid episodeId)
    {
        return (await _episodeRepository.GetAllAsync())
            .Where(ep => ep.Id == episodeId)
            .Select(ep => ep.Duration)
            .First();
    }
}