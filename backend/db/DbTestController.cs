using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace txdemo.db;

[ApiController]
[Route("[controller]")]
public class DbTestController : ControllerBase
{

    public static TxMode mode = TxMode.NONE;

    private ILogger<DbTestController> logger;
    private readonly ApplicationDbContext context;

    public DbTestController(ILogger<DbTestController> logger, ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    [HttpGet]
    public List<TestEntity> Get()
    {
        using var m = magic();
        return context.Entities.OrderBy(x => x.Id).ToList();
    }

    [HttpPost]
    public List<TestEntity> SumAndInsert([FromQuery] bool delay)
    {
        using var m = magic();
        var sum = context.Entities.Select(x => x.Value).Sum();
        if (delay)
        {
            Thread.Sleep(5000);
        }
        context.Entities.Add(new() { Value = sum + 1 });
        context.SaveChanges();
        var result = context.Entities.OrderBy(x => x.Id).ToList();
        return result;
    }

    [HttpGet("mode")]
    public string GetTxMode()
    {
        return "" + DbTestController.mode;
    }
    [HttpPost("mode")]
    public void SetTxMode([FromQuery] TxMode mode)
    {
        DbTestController.mode = mode;
    }

    [HttpGet("doubleRead")]
    public List<int> DoubleRead([FromQuery] bool delay)
    {
        using var m = magic();
        var sum1 = context.Entities.Select(x => x.Value).Sum();
        Thread.Sleep(5000);
        var sum2 = context.Entities.Select(x => x.Value).Sum();
        return new() { sum1, sum2 };
    }

    [HttpDelete]
    public List<TestEntity> Clear()
    {
        using var m = magic();
        context.Entities.RemoveRange(context.Entities);
        context.SaveChanges();
        return context.Entities.OrderBy(x => x.Id).ToList();
    }

    /*




    ********************************************************





    */

    private IDisposable magic()
    {
        switch (mode)
        {
            case TxMode.REPEATABLE_READ:
                context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
                break;
            case TxMode.SERIALIZABLE:
                context.Database.BeginTransaction(IsolationLevel.Serializable);
                break;
            case TxMode.NONE:
            case TxMode.ACTION_FILTER:
                return new DummyDisposable();
        }
        return new CommitTransactionDisposable(context);
    }


    private class CommitTransactionDisposable : IDisposable
    {
        private readonly DbContext dbContext;

        public CommitTransactionDisposable(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            dbContext.Database.CommitTransaction();
        }
    }
    private class DummyDisposable : IDisposable
    {

        public void Dispose()
        {
        }
    }
}