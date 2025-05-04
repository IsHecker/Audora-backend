using Audora.Application.Common.Results;
using MediatR;

namespace Audora.Application.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}