namespace API.Controllers;

using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth() => "secret text";

    [HttpGet("not-found")]
    public ActionResult<string> GetNotFound() => NotFound();

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var result = context.Users.Find(-1) ??
            throw new ArgumentException("Server error occured!");
        return "random text";
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest() => BadRequest("Bad request happened");
}