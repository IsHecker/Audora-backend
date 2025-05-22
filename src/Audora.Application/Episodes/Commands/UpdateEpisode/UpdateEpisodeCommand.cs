using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Commands.UpdateEpisode;

public record UpdateEpisodeCommand(Guid EpisodeId, Episode UpdatedEpisode) : ICommand;

public class UpdateEpisodeCommandHandler : ICommandHandler<UpdateEpisodeCommand>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;

    public UpdateEpisodeCommandHandler(
        IEpisodeRepository episodeRepository,
        IEpisodeStatRepository episodeStatRepository)
    {
        _episodeRepository = episodeRepository;
        _episodeStatRepository = episodeStatRepository;
    }

    public async Task<Result> Handle(UpdateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var updatedEpisode = request.UpdatedEpisode;
        var episode = await _episodeRepository.AsTracking().GetByIdAsync(request.EpisodeId);

        if (episode is null)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }

        if (episode.Name != updatedEpisode.Name)
        {
            var episodeStat = await ((IEpisodeStatRepository)_episodeStatRepository.AsTracking())
                .GetByEpisodeIdAsync(episode.Id);

            episodeStat.UpdateEpisodeName(updatedEpisode.Name);
        }

        episode.Update(updatedEpisode);

        return Result.Success;
    }
}