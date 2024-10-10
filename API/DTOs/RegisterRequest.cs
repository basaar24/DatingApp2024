namespace API.DTOs;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}