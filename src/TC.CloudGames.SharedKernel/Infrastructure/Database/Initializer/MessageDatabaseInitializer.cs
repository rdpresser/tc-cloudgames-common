namespace TC.CloudGames.SharedKernel.Infrastructure.Database.Initializer
{
    public class MessageDatabaseInitializer : IMessageDatabaseInitializer
    {
        private readonly IConnectionStringProvider _connProvider;

        public MessageDatabaseInitializer(IConnectionStringProvider connProvider)
        {
            _connProvider = connProvider;
        }

        public async Task CreateAsync(CancellationToken cancellationToken)
        {
            await PostgresDatabaseHelper.EnsureDatabaseExists(_connProvider);
        }
    }
}
