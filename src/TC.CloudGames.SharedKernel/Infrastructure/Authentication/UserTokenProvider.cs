namespace TC.CloudGames.SharedKernel.Infrastructure.Authentication
{
    public sealed record UserTokenProvider(Guid Id, string Name, string Email, string Username, string Role);
}
