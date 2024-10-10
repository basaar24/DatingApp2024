namespace API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();

        return users;
    }

    [Authorize]
    [HttpGet("{id:int}")] // api/users/2
    public async Task<ActionResult<AppUser>> GetUsersByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound();

        return user;
    }

    [HttpGet("{name}")] // api/users/Calamardo
    public ActionResult<string> Ready(string name) => $"Hi {name}";
}