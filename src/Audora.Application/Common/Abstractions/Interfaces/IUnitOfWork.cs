namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}