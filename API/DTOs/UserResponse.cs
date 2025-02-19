namespace API.DTOs;

public class UserResponse
{
    public required string Username { get; set; }
    public required string KnownAs { get; set; }
    public required string Token { get; set; }
    public string? PhotoUrl { get; set; }
}