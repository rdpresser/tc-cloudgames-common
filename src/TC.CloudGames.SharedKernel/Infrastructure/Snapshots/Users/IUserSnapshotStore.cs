namespace TC.CloudGames.SharedKernel.Infrastructure.Snapshots.Users
{
    public interface IUserSnapshotStore
    {
        Task SaveAsync(UserSnapshot snapshot, CancellationToken cancellationToken = default);
        Task<UserSnapshot?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
