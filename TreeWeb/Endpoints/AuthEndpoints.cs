using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TreeWeb.Abstract;


namespace TreeWeb.Endpoints
{
    public static class AuthEndpoints 
    {
        public static void AddAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/api/auth", Login).WithName("Auth");
            app.MapPost("/api/register", Register).WithName("Register");
        }

        private static async Task<IResult> Login(string userName, string password, IAuthService authService, IAuthOptions options)
        {
           var (user,error) = await authService.Login(userName, password);

            if(error != null || user == null)
            {
                return TypedResults.Forbid();
            }

            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                };

            var creds = new SigningCredentials(options.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return TypedResults.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        private static async Task<IResult> Register(string userName, string password, string role, IAuthService authService, IAuthOptions options)
        {
            var (user, error) = await authService.Register(userName, password, role);
            if(error != null)
            {
                return TypedResults.BadRequest(error);
            }
            if(user != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                var creds = new SigningCredentials(options.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );
                return TypedResults.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return TypedResults.NotFound();
        }
    }
}
