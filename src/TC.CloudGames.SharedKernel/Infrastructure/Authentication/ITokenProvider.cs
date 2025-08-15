namespace TC.CloudGames.SharedKernel.Infrastructure.Authentication
{
    public interface ITokenProvider
    {
        string Create(UserTokenProvider user);
    }
}
