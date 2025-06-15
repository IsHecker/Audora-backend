using System.Security.Cryptography;
using Audora.Application.Auth.Configurations;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;

namespace Audora.Application.Auth.Queries
{
    public record GenerateGoogleUrlQuery(GoogleAuthOptions Options) : IQuery<string>;

    public class GenerateGoogleUrlQueryHandler : IQueryHandler<GenerateGoogleUrlQuery, string>
    {
        private readonly IAuthResultStore _authResultStore;

        public GenerateGoogleUrlQueryHandler(IAuthResultStore authResultStore)
        {
            _authResultStore = authResultStore;
        }

        public Task<Result<string>> Handle(GenerateGoogleUrlQuery request, CancellationToken cancellationToken)
        {
            var _googleOptions = request.Options;

            var signinUrl = _googleOptions.SignInUrl;
            var scope = _googleOptions.Scope;
            var _state = _authResultStore.GenerateState();

            return Task.FromResult<Result<string>>(signinUrl +
                $"?client_id={_googleOptions.ClientId}&" +
                $"redirect_uri={_googleOptions.RedirectUri}&" +
                $"response_type=code&" +
                $"scope={scope}&" +
                $"state={_state}&" +
                $"access_type=offline&" +
                $"prompt=consent");
        }
    }
}