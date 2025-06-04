using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.PlaybackSessions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.PlaybackSessions.Queries.GetOrCreatePlaybackSession;

public record GetOrCreatePlaybackSessionCommand(Guid ListenerId, Guid EpisodeId) : ICommand<PlaybackSessionResponse>;

public class GetOrCreatePlaybackSessionCommandHandler : ICommandHandler<GetOrCreatePlaybackSessionCommand, PlaybackSessionResponse>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IEpisodeRepository _episodeRepository;
    public GetOrCreatePlaybackSessionCommandHandler(
        IPlaybackSessionRepository playbackSessionRepository,
        IEpisodeStatRepository episodeStatRepository,
        IPodcastStatRepository podcastStatRepository,
        IEpisodeRepository episodeRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
        _episodeStatRepository = episodeStatRepository;
        _podcastStatRepository = podcastStatRepository;
        _episodeRepository = episodeRepository;
    }

    public async Task<Result<PlaybackSessionResponse>> Handle(GetOrCreatePlaybackSessionCommand request,
        CancellationToken cancellationToken)
    {
        var episodeDataResult = await GetEpisodeDataAsync(request.EpisodeId);

        if (episodeDataResult.IsError)
            return episodeDataResult.Errors;

        var (PodcastId, EpisodeDuration) = episodeDataResult.Value;



        // TODO should get the latest session since there are a multiple with the same Id combination.
        var podcastStat = await _podcastStatRepository.AsTracking().GetByPodcastIdAsync(PodcastId);
        if (podcastStat is null)
            return Error.Unexpected();


        var oldSession = await _playbackSessionRepository.AsTracking().GetAsync(request.ListenerId, request.EpisodeId);

        if (oldSession is not null && !oldSession.IsSessionExpired)
        {
            oldSession.UpdateLastPlayedAt();
            podcastStat.CalculateRetentionRate(oldSession.LastPlayedAt);

            return oldSession.ToResponse();
        }


        var newSession = new PlaybackSession(request.ListenerId, request.EpisodeId);

        if (oldSession is not null)
        {
            newSession.MarkProgress(oldSession.PlaybackPosition, 0, EpisodeDuration);
            podcastStat.CalculateRetentionRate(oldSession.LastPlayedAt);

            return newSession.ToResponse();
        }

        await IncreasePlayCount(request.EpisodeId, podcastStat);

        await _playbackSessionRepository.AddAsync(newSession);
        return newSession.ToResponse();
    }

    private async Task IncreasePlayCount(Guid episodeId, PodcastStat podcastStat)
    {
        var episodeStat = await _episodeStatRepository.AsTracking().GetByEpisodeIdAsync(episodeId);
        episodeStat.IncreasePlayCount();
        podcastStat.IncreaseTotalPlays();
    }

    private async Task<Result<(Guid PodcastId, long Duration)>> GetEpisodeDataAsync(Guid episodeId)
    {
        var episode = (await _episodeRepository.GetAllAsync())
            .Where(e => e.Id == episodeId)
            .Select(e => new { e.PodcastId, e.Duration })
            .FirstOrDefault();


        if (episode is null)
            return Error.NotFound(description: $"Episode with Id '{episodeId}' is not found");

        return (episode.PodcastId, episode.Duration);
    }
}