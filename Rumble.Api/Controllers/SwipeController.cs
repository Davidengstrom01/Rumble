using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rumble.Api.Data;
using Rumble.Api.Models;

namespace Rumble.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SwipeController : ControllerBase
{
    private readonly DataContext _context;

    public SwipeController(DataContext context)
    {
        _context = context;
    }

    // GET: api/swipe/daily-deck
    [HttpGet("daily-deck")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetDailyDeck()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.UtcNow.Date;

        var swipedRecipeIds = await _context.Swipes
            .Where(s => s.UserId == userId && s.SwipeDate.Date == today)
            .Select(s => s.RecipeId)
            .ToListAsync();

        var recipes = await _context.Recipes
            .Where(r => !swipedRecipeIds.Contains(r.Id))
            .Take(20) // Limit the number of recipes per day for now
            .ToListAsync();

        return Ok(recipes);
    }

    // POST: api/swipe/{recipeId}/{swipedYes}
    [HttpPost("{recipeId}/{swipedYes}")]
    public async Task<IActionResult> PostSwipe(int recipeId, bool swipedYes)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var today = DateTime.UtcNow.Date;

        var recipe = await _context.Recipes.FindAsync(recipeId);
        if (recipe == null)
        {
            return NotFound("Recipe not found.");
        }

        // 1. Save the user's swipe
        var swipe = new Swipe
        {
            UserId = userId,
            RecipeId = recipeId,
            SwipedYes = swipedYes,
            SwipeDate = DateTime.UtcNow
        };
        _context.Swipes.Add(swipe);
        await _context.SaveChangesAsync();

        // 2. If they swiped yes, check for matches
        if (swipedYes)
        {
            // Find all accepted friends
            var friendIds = await _context.UserConnections
                .Where(c => (c.RequesterId == userId || c.RecipientId == userId) && c.Status == ConnectionStatus.Accepted)
                .Select(c => c.RequesterId == userId ? c.RecipientId : c.RequesterId)
                .ToListAsync();

            // Check if any of those friends swiped yes on the same recipe today
            var matchingSwipe = await _context.Swipes
                .FirstOrDefaultAsync(s => friendIds.Contains(s.UserId) && s.RecipeId == recipeId && s.SwipedYes && s.SwipeDate.Date == today);

            if (matchingSwipe != null)
            {
                // 3. Create a Match record
                var match = new Match
                {
                    RecipeId = recipeId,
                    User1Id = userId,
                    User2Id = matchingSwipe.UserId,
                    MatchDate = DateTime.UtcNow
                };
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                
                // Return a special status or object to indicate a match
                return Ok(new { matchFound = true, recipeName = recipe.Title });
            }
        }

        return Ok(new { matchFound = false });
    }
}
