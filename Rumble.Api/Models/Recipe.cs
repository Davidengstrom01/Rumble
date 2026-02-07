namespace Rumble.Api.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty; // For simplicity, will be a single string for now
    public string Instructions { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}
