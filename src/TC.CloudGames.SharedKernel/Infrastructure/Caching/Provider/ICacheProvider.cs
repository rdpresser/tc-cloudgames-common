namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Provider
{
    public interface ICacheProvider
    {
        string InstanceName { get; }
        string ConnectionString { get; }
    }
}
