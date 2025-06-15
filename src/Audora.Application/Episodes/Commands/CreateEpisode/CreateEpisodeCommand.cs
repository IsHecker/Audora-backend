using System.Threading.Tasks;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Commands.CreateEpisode;

public record CreateEpisodeCommand(Guid PodcastId, IEnumerable<Episode> Episodes) : ICommand<IEnumerable<Guid>>;

public class CreateEpisodeCommandHandler : ICommandHandler<CreateEpisodeCommand, IEnumerable<Guid>>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IAudioFileRepository _audioFileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEpisodeCommandHandler(
        IPodcastRepository podcastRepository,
        IEpisodeStatRepository episodeStatRepository,
        IUnitOfWork unitOfWork,
        IAudioFileRepository audioFileRepository)
    {
        _podcastRepository = podcastRepository;
        _episodeStatRepository = episodeStatRepository;
        _unitOfWork = unitOfWork;
        _audioFileRepository = audioFileRepository;
    }

    public async Task<Result<IEnumerable<Guid>>> Handle(CreateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var episodes = request.Episodes.ToList();

        var podcast = await _podcastRepository.AsTracking().GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        podcast.AddEpisodes(episodes);

        // to access episode id.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var audioFilesDict = await _audioFileRepository.GetByIdsAsync(episodes.Select(ep => ep.AudioFileId));

        foreach (var episode in episodes)
        {
            var episodeStat = new EpisodeStat(episode.Id, podcast.Id, episode.Name);
            await _episodeStatRepository.AddAsync(episodeStat);

            episode.AttachAudioFile(audioFilesDict[episode.AudioFileId]);
        }

        return episodes.Select(ep => ep.Id).ToResult();
    }
}