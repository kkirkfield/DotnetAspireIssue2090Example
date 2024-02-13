namespace RazorPagesApp.Models;

public class TodoItem
{
    public required Guid Id { get; set; }
    public required string Label { get; set; }
    public string? Detail { get; set; }
    public bool IsComplete { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset UpdatedAt { get; set; }
}
