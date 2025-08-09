namespace TC.CloudGames.SharedKernel.Infrastructure.Caching.Provider
{
    public class CacheProviderSettings
    {
        public required string Host { get; init; }
        public required string Port { get; init; }
        public string? Password { get; init; } = null;
        public required string InstanceName { get; init; }
    }
}
