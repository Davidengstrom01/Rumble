using Microsoft.AspNetCore.Identity;

namespace Rumble.Api.Models;

public class AppUser : IdentityUser
{
    // You can add additional properties here later if needed
    // For example: public string DisplayName { get; set; }

    public ICollection<UserConnection> SentRequests { get; set; } = new List<UserConnection>();
    public ICollection<UserConnection> ReceivedRequests { get; set; } = new List<UserConnection>();
}
