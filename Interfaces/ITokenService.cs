using FinSharkBackEnd.Model;

namespace FinSharkBackEnd.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}