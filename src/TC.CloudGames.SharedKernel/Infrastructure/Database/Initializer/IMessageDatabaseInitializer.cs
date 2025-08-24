namespace TC.CloudGames.SharedKernel.Infrastructure.Database.Initializer
{
    public interface IMessageDatabaseInitializer
    {
        Task CreateAsync(CancellationToken cancellationToken);
    }

}
