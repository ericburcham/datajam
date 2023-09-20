namespace DataJam.TestSupport;

using System.Transactions;

using NUnit.Framework;

public class TransactionalScenario
{
    private readonly TransactionScope? _transactionScope;

    public TransactionalScenario(bool useAmbientTransaction)
    {
        if (useAmbientTransaction)
        {
            _transactionScope = new(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        _transactionScope?.Dispose();
    }
}
