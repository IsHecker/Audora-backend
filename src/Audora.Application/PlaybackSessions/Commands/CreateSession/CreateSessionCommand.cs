using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mapping;
using Audora.Application.Common.Results;
using Audora.Contracts.PlaybackSessions.Responses;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.PlaybackSessions.Commands.CreateSession;

public record CreateSessionCommand(Guid ListenerId, Guid PodcastId, Guid EpisodeId) : ICommand<CreateSessionResponse>;

public class CreateSessionCommandHandler : ICommandHandler<CreateSessionCommand, CreateSessionResponse>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;

    public CreateSessionCommandHandler(IPlaybackSessionRepository playbackSessionRepository,
        IEpisodeStatRepository episodeStatRepository, IPodcastStatRepository podcastStatRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
        _episodeStatRepository = episodeStatRepository;
        _podcastStatRepository = podcastStatRepository;
    }

    public async Task<Result<CreateSessionResponse>> Handle(CreateSessionCommand request,
        CancellationToken cancellationToken)
    {
        // TODO should get the latest session since there are a multiple with the same Id combination.
        var session = await _playbackSessionRepository.GetAsync(request.ListenerId, request.EpisodeId);
        var podcastStat = await _podcastStatRepository.GetPodcastStatByPodcastIdAsync(request.PodcastId);


        if (session is not null && !session.IsSessionExpired)
        {
            session.UpdateLastPlayedAt();
            podcastStat.CalculateRetentionRate(session.LastPlayedAt);

            return session.ToResponse();
        }

        // TODO add old session progress to new session if available, in repo.
        var newSession = await _playbackSessionRepository.AddAsync(request.ListenerId, request.EpisodeId);

        if (session is not null)
        {
            newSession.MarkProgress(session.PlaybackPosition, session.ListenedDuration, session.IsCompleted);
            podcastStat.CalculateRetentionRate(session.LastPlayedAt);

            return newSession.ToResponse();
        }

        await IncreasePlayCount(request.EpisodeId, podcastStat);
        return newSession.ToResponse();
    }

    private async Task IncreasePlayCount(Guid episodeId, PodcastStat podcastStat)
    {
        var episodeStat = await _episodeStatRepository.GetEpisodeStatByEpisodeIdAsync(episodeId);

        episodeStat.IncreasePlayCount();
        podcastStat.IncreaseTotalPlays();
    }
}