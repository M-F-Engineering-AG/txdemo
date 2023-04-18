using System.ComponentModel.DataAnnotations.Schema;

namespace txdemo.db;

public class TestEntity
{
    public long Id { get; set; }

    public int Value { get; set; }
}