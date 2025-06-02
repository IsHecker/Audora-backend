using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Commands.CreateEpisode;

public record CreateEpisodeCommand(Guid PodcastId, Episode Episode) : ICommand<Episode>;

public class CreateEpisodeCommandHandler : ICommandHandler<CreateEpisodeCommand, Episode>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEpisodeCommandHandler(
        IPodcastRepository podcastRepository,
        IEpisodeStatRepository episodeStatRepository,
        IUnitOfWork unitOfWork)
    {
        _podcastRepository = podcastRepository;
        _episodeStatRepository = episodeStatRepository;
        _unitOfWork = unitOfWork;
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

        // to access episode id.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var episodeStat = new EpisodeStat(episode.Id, podcast.Id, episode.Name);
        await _episodeStatRepository.AddAsync(episodeStat);

        return request.Episode;
    }
}