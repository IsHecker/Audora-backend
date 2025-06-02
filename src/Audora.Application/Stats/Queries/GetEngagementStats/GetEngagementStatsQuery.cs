using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Contracts.EngagementStats.Responses;
using Audora.Domain.Common.Enums;
using Audora.Application.Common.Services;

namespace Audora.Application.Stats.Queries.GetEngagementStats;

public record GetEngagementStatsQuery(Guid EntityId, EntityType EntityType) : IQuery<EngagementStatsResponse>;

public class GetEngagementStatsQueryHandler : IQueryHandler<GetEngagementStatsQuery, EngagementStatsResponse>
{
    private readonly EngagementStatsService _engagementStatsService;

    public GetEngagementStatsQueryHandler(EngagementStatsService engagementStatsService)
    {
        _engagementStatsService = engagementStatsService;
    }

    public async Task<Result<EngagementStatsResponse>> Handle(GetEngagementStatsQuery request, CancellationToken cancellationToken)
    {
        return await _engagementStatsService.GetStatsAsync(request.EntityId, request.EntityType);
    }
}