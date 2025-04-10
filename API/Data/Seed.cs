namespace API.Data;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;
using API.DataEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class Seed
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, ReadOptions);

        if (users == null)
        {
            return;
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLowerInvariant();
            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        AllowTrailingCommas = true
    };
}