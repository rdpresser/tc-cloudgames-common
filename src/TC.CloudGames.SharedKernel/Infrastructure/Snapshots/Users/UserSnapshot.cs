namespace TC.CloudGames.SharedKernel.Infrastructure.Snapshots.Users
{
    /// <summary>
    /// Represents a snapshot of the User entity as known by the Games service.
    /// This is not an aggregate, but a projection of user data from the Users service.
    /// </summary>
    public class UserSnapshot
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
