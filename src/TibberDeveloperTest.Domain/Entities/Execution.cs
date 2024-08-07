using System.ComponentModel.DataAnnotations.Schema;

namespace TibberDeveloperTest.Domain.Entities;

[Table("Executions", Schema = "cleaning_robot")]
public class Execution
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Commands { get; set; }
    public int Result { get; set; }
    public decimal DurationS { get; set; }
}