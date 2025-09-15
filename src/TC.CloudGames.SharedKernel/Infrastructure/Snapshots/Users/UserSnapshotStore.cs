namespace TC.CloudGames.SharedKernel.Infrastructure.Snapshots.Users
{
    /// <summary>
    /// Provides persistence for UserSnapshot using Marten.
    /// Can be used by multiple services that consume User integration events.
    /// </summary>
    public class UserSnapshotStore : IUserSnapshotStore
    {
        private readonly IDocumentSession _session;

        public UserSnapshotStore(IDocumentSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async Task SaveAsync(UserSnapshot snapshot)
        {
            _session.Store(snapshot);
            await _session.SaveChangesAsync();
        }

        public async Task<UserSnapshot?> LoadAsync(Guid id)
        {
            return await _session.LoadAsync<UserSnapshot>(id);
        }
    }
}
