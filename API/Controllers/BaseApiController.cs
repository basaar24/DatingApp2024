namespace API.Controllers;

using API.Helpers;
using Microsoft.AspNetCore.Mvc;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
}