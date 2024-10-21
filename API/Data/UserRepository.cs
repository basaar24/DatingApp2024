namespace API.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using API.DataEntities;
using Microsoft.EntityFrameworkCore;

public class UserRepository(DataContext context) : IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllAsync()
        => await context.Users
                .Include(u => u.Photos)
                .ToListAsync();

    public async Task<AppUser?> GetByIdAsync(int id)
        => await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == username);

    public async Task<bool> SaveAllAsync()
        => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user)
        => context.Entry(user).State = EntityState.Modified;
}