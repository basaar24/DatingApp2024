using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterRequest
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}