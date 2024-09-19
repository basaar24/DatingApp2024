using API.Entities;

namespace API.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);    
}