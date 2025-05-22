using System.Transactions;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using MediatR;

namespace Audora.Application.Common.Behaviours;

public class UnitOfWorkBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
    {
        // enable async-flow so the scope can cross awaits
        var scopeOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        };

        using (var scope = new TransactionScope(
            TransactionScopeOption.Required,
            scopeOptions,
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var response = await next(cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
            return response;
        }
    }
}