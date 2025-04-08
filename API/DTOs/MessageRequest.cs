namespace API.DTOs;

public class MessageRequest
{
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }
}