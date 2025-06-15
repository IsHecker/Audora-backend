using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Auth.Responses;

namespace Audora.Application.Auth.Queries;

public record GetGoogleAuthResultQuery(string State) : IQuery<AuthResponse>;

public class GetGoogleAuthResultQueryHandler : IQueryHandler<GetGoogleAuthResultQuery, AuthResponse>
{
    private readonly IAuthResultStore _authResultStore;

    public GetGoogleAuthResultQueryHandler(IAuthResultStore authResultStore)
    {
        _authResultStore = authResultStore;
    }

    public Task<Result<AuthResponse>> Handle(GetGoogleAuthResultQuery request, CancellationToken cancellationToken)
    {
        var authResult = _authResultStore.Get(request.State);

        if (authResult is null)
            return Task.FromResult<Result<AuthResponse>>(
                Error.NotFound(description: "Authentication result not found for the provided state."));

        if (authResult.Errors is not null)
            return Task.FromResult<Result<AuthResponse>>(authResult.Errors);

        return Task.FromResult<Result<AuthResponse>>(authResult.ToResponse());
    }
}