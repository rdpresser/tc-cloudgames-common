namespace TC.CloudGames.SharedKernel.Infrastructure.UserClaims
{
    public interface IUserContext
    {
        Guid Id { get; }
        string Name { get; }
        string Email { get; }
        string Username { get; }
        string Role { get; }
    }
}
