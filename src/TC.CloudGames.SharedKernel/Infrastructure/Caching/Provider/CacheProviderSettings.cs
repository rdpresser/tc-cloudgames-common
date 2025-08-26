namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Provider
{
    public class CacheProviderSettings
    {
        public required string Host { get; init; }
        public required string Port { get; init; }
        public required string Password { get; init; }
        public bool Secure { get; init; } = true;
        public required string InstanceName { get; init; }
    }
}
