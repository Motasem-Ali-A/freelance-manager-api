using FreelanceManager.Core.Models;

namespace FreelanceManager.Core.interfaces
{
    public interface ITokenService
    {
        string CreateToken (AppUser user);
    }
}