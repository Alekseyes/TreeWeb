namespace TreeWeb.Models.DTO
{
    public record UserDTO
    {
        public string Username { get; init; }
        public string Role { get; init; } = "";

        public UserDTO (User user) {
            Username = user.Username;
            Role = user.Role;
        }
    }
}
