namespace TC.CloudGames.SharedKernel.Infrastructure.Snapshots.Users
{
    public interface IUserSnapshotStore
    {
        Task SaveAsync(UserSnapshot snapshot);
        Task<UserSnapshot?> LoadAsync(Guid id);
    }
}
