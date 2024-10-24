namespace API.Services;
using API.DataEntities;

public interface ITokenService
{
    string CreateToken(AppUser user);
}