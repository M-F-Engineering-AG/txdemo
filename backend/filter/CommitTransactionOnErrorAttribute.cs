namespace txdemo.filter;

/// <summary>
/// When placed on a controller method, the transaction is not rolled back when an exception is thrown.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CommitTransactionOnErrorAttribute : Attribute
{
}
