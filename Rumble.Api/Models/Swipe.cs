namespace Rumble.Api.Models;

public class Swipe
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public bool SwipedYes { get; set; }
    public DateTime SwipeDate { get; set; }
}