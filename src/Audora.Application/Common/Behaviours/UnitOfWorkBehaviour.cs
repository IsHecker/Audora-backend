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
        using (var transactionScope = new TransactionScope())
        {
            var response = await next(cancellationToken);
            await _unitOfWork.CommitChangesAsync();

            transactionScope.Complete();

            return response;
        }
    }
}