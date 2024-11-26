namespace DataJam.TestSupport.EntityFrameworkCore;

using System.Transactions;

using NUnit.Framework;

public class TransactionalScenario<TTransaction>
{
    private readonly TransactionScope? _transactionScope;

    private readonly IUnitOfWork<TTransaction> _unitOfWork;

    public TransactionalScenario(IUnitOfWork<TTransaction> unitOfWork)
    {
        _unitOfWork = unitOfWork;

        if (_unitOfWork.SupportsTransactionScopes)
        {
            _transactionScope = new(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }
        else if (_unitOfWork.SupportsLocalTransactions)
        {
            _unitOfWork.BeginTransaction();
        }
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        _transactionScope?.Dispose();

        if (_unitOfWork.CurrentTransaction != null)
        {
            _unitOfWork.RollbackTransaction();
        }
    }
}
