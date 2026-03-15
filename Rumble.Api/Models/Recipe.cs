namespace Rumble.Api.Models;

public class Recipe : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Ingredients { get; set; } = []; // For simplicity, will be a single string for now
    public string[] Instructions { get; set; } = [];
    public string[]? ImageUrl { get; set; } = null;
    public bool Private  { get; set; } = false;
}
