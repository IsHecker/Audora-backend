using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Commands.CreateEpisode;

public record CreateEpisodeCommand(Guid PodcastId, Episode Episode) : ICommand<Episode>;

public class CreateEpisodeCommandHandler : ICommandHandler<CreateEpisodeCommand, Episode>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEpisodeCommandHandler(
        IPodcastRepository podcastRepository,
        IEpisodeStatRepository episodeStatRepository,
        IUnitOfWork unitOfWork,
        IEngagementStatRepository engagementStatRepository)
    {
        _podcastRepository = podcastRepository;
        _episodeStatRepository = episodeStatRepository;
        _unitOfWork = unitOfWork;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result<Episode>> Handle(CreateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var episode = request.Episode;

        var podcast = await _podcastRepository.AsTracking().GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        podcast.AddEpisode(episode);

        await _unitOfWork.SaveChangesAsync(cancellationToken);


        var episodeStat = new EpisodeStat(episode.Id, podcast.Id, episode.Name);
        await _episodeStatRepository.AddAsync(episodeStat);

        var engagementStat = new EngagementStat(episode.Id, EntityType.Episode);
        await _engagementStatRepository.AddAsync(engagementStat);

        return request.Episode;
    }
}