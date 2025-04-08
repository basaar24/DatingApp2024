namespace API.Data;

using API.DataEntities;
using API.DTOs;
using API.Helpers;

public interface IUserRepository
{
    public void Update(AppUser user);
    public Task<bool> SaveAllAsync();
    public Task<IEnumerable<AppUser>> GetAllAsync();
    public Task<AppUser?> GetByIdAsync(int id);
    public Task<AppUser?> GetByUsernameAsync(string username);
    public Task<PagedList<MemberResponse>> GetMembersAsync(UserParams userParams);
    public Task<MemberResponse?> GetMemberAsync(string username);
}