namespace API.Extensions;

using System.Globalization;
using System.Security.Claims;

public static class ClaimsPrincipleExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ArgumentException("Cannot get the userId from token"), CultureInfo.InvariantCulture);

        return userId;
    }

    public static string GetUserName(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name)
            ?? throw new ArgumentException("Cannot get the username from token");

        return username;
    }
}