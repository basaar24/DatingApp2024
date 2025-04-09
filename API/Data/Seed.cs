namespace API.Data;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;
using API.DataEntities;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class Seed
{
    public static async Task SeedUsersAsync(DataContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null)
        {
            return;
        }

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}