namespace TC.CloudGames.SharedKernel.Infrastructure.Authentication
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string UserEmail { get; }
        string UserName { get; }
        string UserRole { get; }
    }
}
