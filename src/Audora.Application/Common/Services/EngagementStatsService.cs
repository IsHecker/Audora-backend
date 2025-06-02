using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.EngagementStats.Responses;
using Audora.Domain.Common.Enums;

namespace Audora.Application.Common.Services;

public class EngagementStatsService
{
    private readonly IReactionStatRepository _reactionStatRepository;
    private readonly ICommentStatRepository _commentStatRepository;

    public EngagementStatsService(
        IReactionStatRepository reactionStatRepository,
        ICommentStatRepository commentStatRepository)
    {
        _reactionStatRepository = reactionStatRepository;
        _commentStatRepository = commentStatRepository;
    }

    public async Task<Result<EngagementStatsResponse>> GetStatsAsync(Guid entityId, EntityType entityType)
    {
        var reactionStats = await _reactionStatRepository.GetByEntityAsync(entityId, entityType);

        // if (reactionStats is null)
        // {
        //     return Error.NotFound(description: $"ReactionStat with EntityId '{entityId}' is not found.");
        // }

        var commentCount = await _commentStatRepository.GetCommentCountAsync(entityId, entityType);

        return EngagementStatMapping.ToResponse(reactionStats, commentCount);
    }

    public async Task<Result<Dictionary<Guid, EngagementStatsResponse>>> GetStatsAsync(IEnumerable<Guid> entityIds, EntityType entityType)
    {
        var reactionStatsDict = await _reactionStatRepository.GetByEntitiesAsync(entityIds, entityType);

        var result = new Dictionary<Guid, EngagementStatsResponse>();

        if (reactionStatsDict is null || reactionStatsDict.Count == 0)
        {
            return result;
        }

        var commentCountDict = await _commentStatRepository.GetCommentCountByEntitiesAsync(entityIds, entityType);

        foreach (var entityId in entityIds)
        {
            reactionStatsDict.TryGetValue(entityId, out var reactions);
            commentCountDict.TryGetValue(entityId, out var commentCount);

            result[entityId] = EngagementStatMapping.ToResponse(reactions!, commentCount);
        }

        return result;
    }

    public async Task<Result> DeleteStatsAsync(Guid entityId, EntityType entityType)
    {
        var isDeleted = await _reactionStatRepository.DeleteByEntityAsync(entityId, entityType);

        if (!isDeleted)
        {
            return Error.NotFound(description: $"ReactionStat with EntityId '{entityId}' is not found.");
        }

        await _commentStatRepository.DeleteByEntityAsync(entityId, entityType);

        return Result.Success;
    }
}