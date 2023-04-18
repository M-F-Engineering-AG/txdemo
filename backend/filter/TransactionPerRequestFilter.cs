using System.Data;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using txdemo.db;

namespace txdemo.filter;

/// <summary>
/// Action filter creating a database transaction per HTTP request.
/// </summary>
public sealed class TransactionPerRequestFilter : IAsyncActionFilter
{
    private readonly DbContext dbContext;

    public TransactionPerRequestFilter(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isWriteRequest = IsWriteRequest(context);

        await using var transaction = await this.dbContext.Database.BeginTransactionAsync(
            isWriteRequest ? IsolationLevel.Serializable : IsolationLevel.Snapshot);

        var executedContext = await next();

        if (isWriteRequest && (WasSuccessfulExecution(executedContext) || HasCommitTransactionOnError(executedContext)))
        {
            await transaction.CommitAsync();
        }
    }

    private static bool WasSuccessfulExecution(ActionExecutedContext executedContext)
    {
        var statusCode = (executedContext.Result as IStatusCodeActionResult)?.StatusCode
            ?? (executedContext.Result as ObjectResult)?.StatusCode
            ?? (int)HttpStatusCode.OK;

        return executedContext.Exception == null
            && !executedContext.ExceptionHandled
            && statusCode < (int)HttpStatusCode.BadRequest;
    }

    private static bool HasCommitTransactionOnError(ActionExecutedContext executedContext)
    {
        var actionDescriptor = (executedContext.Controller as ControllerBase)?.ControllerContext.ActionDescriptor;
        if (actionDescriptor == null)
        {
            return false;
        }

        return actionDescriptor.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(CommitTransactionOnErrorAttribute));
    }


    private static readonly HashSet<string> WriteMethods = new HashSet<string>
    {
        "POST",
        "PUT",
        "DELETE",
    };

    public static bool IsWriteRequest(FilterContext actionContext)
        => WriteMethods.Contains(actionContext.HttpContext.Request.Method)
        || ((actionContext.ActionDescriptor as ControllerActionDescriptor)
            ?.MethodInfo
            .GetCustomAttributes<WriteableGetAttribute>()
            .Any() ?? false);

}
