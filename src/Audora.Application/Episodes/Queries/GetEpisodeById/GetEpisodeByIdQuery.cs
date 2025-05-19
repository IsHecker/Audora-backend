using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Episodes.Responses;

namespace Audora.Application.Episodes.Queries.GetEpisodeById;

public record GetEpisodeByIdQuery(Guid ListenerId, Guid EpisodeId) : IQuery<EpisodeResponse>;

public class GetEpisodeByIdQueryHandler : IQueryHandler<GetEpisodeByIdQuery, EpisodeResponse>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly EpisodeResponseAttacher _episodeResponseAttacher;

    public GetEpisodeByIdQueryHandler(IEpisodeRepository episodeRepository,
        EpisodeResponseAttacher episodeResponseAttacher)
    {
        _episodeRepository = episodeRepository;
        _episodeResponseAttacher = episodeResponseAttacher;
    }

    public async Task<Result<EpisodeResponse>> Handle(GetEpisodeByIdQuery request, CancellationToken cancellationToken)
    {
        var episode = await _episodeRepository.GetByIdAsync(request.EpisodeId);
        if (episode is null)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }

        var response = episode.ToResponse();


        return _episodeResponseAttacher.AttachTo(response)
            .AttachEpisodeStats()
            .AttachListenerReactions(request.ListenerId)
            .GetSingleResponse();
    }
}