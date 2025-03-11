namespace API.Controllers;

using API.Data;
using API.DataEntities;
using API.DTOs;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;

public class LikesController(ILikesRepository likesRepository) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();
        if (sourceUserId == targetUserId) { return BadRequest("You already like yourself! :D"); }

        var existingLike = await likesRepository.GetUserLikeAsync(sourceUserId, targetUserId);
        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };
            likesRepository.AddLike(like);
        }
        else { likesRepository.RemoveLike(existingLike); }

        if (await likesRepository.SaveChangesAsync()) { return Ok(); }
        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUSerLikeIds()
        => Ok(await likesRepository.GetCurrentUserLikeIdsAsync(User.GetUserId()));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetUserLikes(string predicate)
    {
        var users = await likesRepository.GetUserLikesAsync(predicate, User.GetUserId());

        return Ok(users);
    }
}