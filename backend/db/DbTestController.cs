using Microsoft.AspNetCore.Mvc;

namespace txdemo.db;

[ApiController]
[Route("[controller]")]
public class DbTestController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
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
        return context.Entities.OrderBy(x => x.Id).ToList();
    }

    [HttpPost]
    public List<TestEntity> SumAndInsert([FromQuery] bool delay)
    {
        var sum = context.Entities.Select(x => x.Value).Sum();
        if (delay)
        {
            Thread.Sleep(2000);
        }
        context.Entities.Add(new() { Value = sum + 1 });
        context.SaveChanges();
        return context.Entities.OrderBy(x => x.Id).ToList();
    }

    [HttpGet("doubleRead")]
    public List<int> DoubleRead([FromQuery] bool delay)
    {
        var sum1 = context.Entities.Select(x => x.Value).Sum();
        Thread.Sleep(2000);
        var sum2 = context.Entities.Select(x => x.Value).Sum();
        return new() { sum1, sum2 };
    }

    [HttpDelete]
    public List<TestEntity> Clear()
    {
        context.Entities.RemoveRange(context.Entities);
        context.SaveChanges();
        return context.Entities.OrderBy(x => x.Id).ToList();
    }
}
