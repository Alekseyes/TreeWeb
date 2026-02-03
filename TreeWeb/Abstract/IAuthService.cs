using TreeWeb.Models.DTO;

namespace TreeWeb.Abstract
{
    public interface IAuthService
    {
        Task<(UserDTO? user,string? error)> Login(string username, string password);

        Task<(UserDTO? user, string? error)> Register(string username, string password, string role);
    }
}
