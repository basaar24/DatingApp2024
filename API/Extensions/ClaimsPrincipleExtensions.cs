namespace API.Extensions;

using System.Security.Claims;

public static class ClaimsPrincipleExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ArgumentException("Cannot get the username from token");

        return username;
    }
}