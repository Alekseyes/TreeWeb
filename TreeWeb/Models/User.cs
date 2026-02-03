using Microsoft.EntityFrameworkCore;
using static TreeWeb.AppContext.TreeWebDbContext;

namespace TreeWeb.Models
{
    [EntityTypeConfiguration(typeof(UserConfiguration))]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin" или "User"
    }
}
