namespace Rumble.Api.Models;

public enum ConnectionStatus
{
    Pending,
    Accepted,
    Declined
}

public class UserConnection
{
    public int Id { get; set; }
    public string RequesterId { get; set; }
    public AppUser Requester { get; set; }
    public string RecipientId { get; set; }
    public AppUser Recipient { get; set; }
    public ConnectionStatus Status { get; set; }
}
