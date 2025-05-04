using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Episodes.Commands.UpdateEpisode;

public record UpdateEpisodeCommand(Episode UpdatedEpisode) : ICommand;

public class UpdateEpisodeCommandHandler : ICommandHandler<UpdateEpisodeCommand>
{
    private readonly IEpisodeRepository _episodeRepository;

    public UpdateEpisodeCommandHandler(IEpisodeRepository episodeRepository)
    {
        _episodeRepository = episodeRepository;
    }

    public async Task<Result> Handle(UpdateEpisodeCommand request, CancellationToken cancellationToken)
    {
        // TODO maybe return errors if update problem.
        await _episodeRepository.UpdateAsync(request.UpdatedEpisode);
        
        return Result.Success;
    }
}