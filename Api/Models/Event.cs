namespace Api.Models;

public class Event
{
    public int Id { get; set; }
    public string Type {get; set;}
    public double Value {get; set;}
    public DateTime Timestamp {get; set;} = DateTime.UtcNow;
}