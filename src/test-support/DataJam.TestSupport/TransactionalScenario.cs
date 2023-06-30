namespace DataJam.TestSupport;

using System.Transactions;

using NUnit.Framework;

public class TransactionalScenario
{
    private readonly TransactionScope _transactionScope = new(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        _transactionScope.Dispose();
    }
}
