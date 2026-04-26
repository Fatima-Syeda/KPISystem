namespace Worker.Models;

public class EventRecord
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}