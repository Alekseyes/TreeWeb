using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TreeWeb.Abstract;

namespace TreeWeb
{
    public class AuthOptions : IAuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        private string _jwtKey { get; init; } = ""; // = "mysuperpuper_secretsecretsecretkey!123";   // ключ для шифрования

        public AuthOptions(IConfiguration config)
        {
           _jwtKey = config["Jwt:Key"];
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
           return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
           return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
