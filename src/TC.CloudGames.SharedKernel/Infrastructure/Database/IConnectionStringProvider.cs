namespace TC.CloudGames.SharedKernel.Infrastructure.Database
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
        string MaintenanceConnectionString { get; }
        string OutboxConnectionString { get; }
    }
}
