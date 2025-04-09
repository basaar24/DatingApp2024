namespace API.Controllers;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.DataEntities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

public class AccountController(
    DataContext context,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await UserExistsAsync(request.Username))
        {
            return BadRequest("Username already in use");
        }

        using var hmac = new HMACSHA512();
        var user = mapper.Map<AppUser>(request);
        user.UserName = request.Username.ToLowerInvariant();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        // user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await context.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.UserName.ToLowerInvariant() == request.Username.ToLowerInvariant());

        if (user == null || user.UserName == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return new UserResponse
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };
    }

    private async Task<bool> UserExistsAsync(string username) =>
        await context.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpper());
}