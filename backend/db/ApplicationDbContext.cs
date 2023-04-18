using Microsoft.EntityFrameworkCore;

namespace txdemo.db;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connStr = "server=localhost;port=3306;user id=root;password=acsacs;database=db_txdemo";
        optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
    }

    public DbSet<TestEntity> Entities { get; set; } = null!;
}