using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
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
        var oldSession = await _playbackSessionRepository.GetAsync(request.ListenerId, request.EpisodeId);
        var podcastStat = await _podcastStatRepository.GetByPodcastIdAsync(request.PodcastId);


        if (oldSession is not null && !oldSession.IsSessionExpired)
        {
            oldSession.UpdateLastPlayedAt();
            podcastStat.CalculateRetentionRate(oldSession.LastPlayedAt);

            return oldSession.ToResponse();
        }

        // TODO add old session progress to new session if available, in repo.
        var newSession = new PlaybackSession(request.ListenerId, request.EpisodeId);

        if (oldSession is not null)
        {
            newSession.MarkProgress(oldSession.PlaybackPosition, oldSession.ListenedDuration, oldSession.IsCompleted);
            podcastStat.CalculateRetentionRate(oldSession.LastPlayedAt);

            return newSession.ToResponse();
        }

        await IncreasePlayCount(request.EpisodeId, podcastStat);
        
        await _playbackSessionRepository.AddAsync(newSession);
        return newSession.ToResponse();
    }

    private async Task IncreasePlayCount(Guid episodeId, PodcastStat podcastStat)
    {
        var episodeStat = await _episodeStatRepository.GetByEpisodeIdAsync(episodeId);

        episodeStat.IncreasePlayCount();
        podcastStat.IncreaseTotalPlays();
    }
}