namespace API.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
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

    public async Task<MemberResponse?> GetMemberAsync(string username)
        => await context.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

    public async Task<PagedList<MemberResponse>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users.AsQueryable();

        query = query.Where(u => u.UserName != userParams.CurrentUsername);

        if (userParams.Gender != null)
        {
            query = query.Where(u => u.Gender == userParams.Gender);
        }

        var minBDay = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxBDay = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(u => u.BirthDay >= minBDay && u.BirthDay <= maxBDay);

        return await PagedList<MemberResponse>.CreateAsync(
            query.ProjectTo<MemberResponse>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
    }

    public async Task<bool> SaveAllAsync()
        => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user)
        => context.Entry(user).State = EntityState.Modified;
}