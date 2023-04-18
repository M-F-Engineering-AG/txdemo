namespace txdemo.filter;

/// <summary>
/// Attribute marking GET requests handlers as writable.
/// </summary>
/// <seealso cref="TransactionPerRequestFilter" />
/// <remarks>
/// GET requests are usually not allowed to write to the database and and any changes performed are rolled back implicitly.
/// With this attribute, the database is committed as in PUT/POST/DELETE requests.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class WriteableGetAttribute : Attribute
{
}
