namespace Rumble.Api.Models;

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
