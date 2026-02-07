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

public class Match
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public string User1Id { get; set; }
    public AppUser User1 { get; set; }
    public string User2Id { get; set; }
    public AppUser User2 { get; set; }
    public DateTime MatchDate { get; set; }
}
