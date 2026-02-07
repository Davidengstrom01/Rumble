using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rumble.Api.Data;
using Rumble.Api.Models;

namespace Rumble.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConnectionsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ConnectionsController(DataContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // POST: api/connections/send-request/{recipientUsername}
    [HttpPost("send-request/{recipientUsername}")]
    public async Task<IActionResult> SendRequest(string recipientUsername)
    {
        var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipient = await _userManager.FindByNameAsync(recipientUsername);

        if (recipient == null)
        {
            return NotFound("User not found.");
        }
        
        if (requesterId == recipient.Id)
        {
            return BadRequest("You cannot send a friend request to yourself.");
        }

        var existingConnection = await _context.UserConnections
            .FirstOrDefaultAsync(c => 
                (c.RequesterId == requesterId && c.RecipientId == recipient.Id) ||
                (c.RequesterId == recipient.Id && c.RecipientId == requesterId));

        if (existingConnection != null)
        {
            return BadRequest("A connection or pending request already exists with this user.");
        }

        var connection = new UserConnection
        {
            RequesterId = requesterId,
            RecipientId = recipient.Id,
            Status = ConnectionStatus.Pending
        };

        _context.UserConnections.Add(connection);
        await _context.SaveChangesAsync();

        return Ok("Friend request sent.");
    }

    // GET: api/connections/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<ConnectionDto>>> GetPendingRequests()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var requests = await _context.UserConnections
            .Where(c => c.RecipientId == currentUserId && c.Status == ConnectionStatus.Pending)
            .Select(c => new ConnectionDto
            {
                UserId = c.Requester.Id,
                Username = c.Requester.UserName
            })
            .ToListAsync();

        return Ok(requests);
    }

    // POST: api/connections/accept-request/{requesterId}
    [HttpPost("accept-request/{requesterId}")]
    public async Task<IActionResult> AcceptRequest(string requesterId)
    {
        var recipientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var connection = await _context.UserConnections
            .FirstOrDefaultAsync(c => c.RequesterId == requesterId && c.RecipientId == recipientId && c.Status == ConnectionStatus.Pending);

        if (connection == null)
        {
            return NotFound("Pending request not found.");
        }

        connection.Status = ConnectionStatus.Accepted;
        await _context.SaveChangesAsync();

        return Ok("Friend request accepted.");
    }

    // GET: api/connections
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConnectionDto>>> GetFriends()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var friends = await _context.UserConnections
            .Where(c => (c.RequesterId == currentUserId || c.RecipientId == currentUserId) && c.Status == ConnectionStatus.Accepted)
            .Select(c => new ConnectionDto
            {
                UserId = c.RequesterId == currentUserId ? c.Recipient.Id : c.Requester.Id,
                Username = c.RequesterId == currentUserId ? c.Recipient.UserName : c.Requester.UserName
            })
            .ToListAsync();

        return Ok(friends);
    }
}
