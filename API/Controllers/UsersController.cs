namespace API.Controllers;

using System.Security.Claims;
using API.Data;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync()
    {
        var members = await _repository.GetMembersAsync();
        return Ok(members);
    }

    [HttpGet("{username}")] // api/users/Calamardo
    public async Task<ActionResult<MemberResponse>> GetByUsernameAsync(string username)
    {
        var member = await _repository.GetMemberAsync(username);

        if (member == null)
        {
            return NotFound();
        }

        return member;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateRequest request)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null)
        {
            return BadRequest("No username found in token");
        }

        var user = await _repository.GetByUsernameAsync(username);

        if (user == null)
        {
            return BadRequest("Could not find user");
        }

        _mapper.Map(request, user);
        _repository.Update(user);

        if (await _repository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Update user failed!");
    }
}