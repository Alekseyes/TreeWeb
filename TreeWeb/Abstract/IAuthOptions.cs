using Microsoft.IdentityModel.Tokens;

namespace TreeWeb.Abstract
{
    public interface IAuthOptions
    {
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}
