namespace API.DataEntities;

public class Message
{
    public int Id { get; set; }
    public required string SenderUsername { get; set; }
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

    // EF Navigation properties
    public AppUser Sender { get; set; } = null!;
    public int SenderId { get; set; }
    public AppUser Recipient { get; set; } = null!;
    public int RecipientId { get; set; }
}