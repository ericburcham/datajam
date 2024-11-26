namespace DataJam;

public interface IDeclareTransactionSupport
{
    /// <summary>Gets a value indicating whether the instance supports local transactions.</summary>
    public bool SupportsLocalTransactions { get; }

    /// <summary>Gets a value indicating whether the instance supports transaction scope participation.</summary>
    public bool SupportsTransactionScopes { get; }
}
