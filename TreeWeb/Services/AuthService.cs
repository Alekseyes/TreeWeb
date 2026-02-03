using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TreeWeb.Abstract;
using TreeWeb.AppContext;
using TreeWeb.Enums;
using TreeWeb.Models;
using TreeWeb.Models.DTO;

namespace TreeWeb.Services
{
    public class AuthService: IAuthService
    {
        private readonly TreeWebDbContext _dbContext;

        private readonly ILogger<AuthService> _logger;

        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(TreeWebDbContext dbContext, ILogger<AuthService> logger, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<(UserDTO? user, string? error)> Login(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null)
            {
                return (null,"Нет такого пользователя");
            }
            
            var hashedPassword = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash, password);
            if (hashedPassword != PasswordVerificationResult.Success)
            {
                return (null, "Неверный пароль");
            }
            return (new UserDTO(user), null);
        }

         public async Task<(UserDTO? user, string? error)> Register(string username, string password, string role)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user != null)
            {
                return (null, "Такое имя пользователя уже занято");
            }
            var t1 = new IdentityUser(username);


            if(TryGetRole(role, out string roleName))
            {
                user = new User { Role = roleName, Username = username };
            }
            else
            {
                return (null, "Такой роли не существует");
            }

            var hashedPassword = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return (new UserDTO(user), null);
        }

        private bool TryGetRole(string role, out string roleName)
        {
            roleName = "";
            foreach (var nameRole in typeof(RoleEnum).GetEnumNames())
            {
                if (nameRole == role)
                {
                    roleName = nameRole;
                    return true;
                }
            }
            return false;
        }
    }
}
