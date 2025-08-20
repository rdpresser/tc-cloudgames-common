namespace TC.CloudGames.SharedKernel.Domain.Events
{
    public record BaseDomainEvent(Guid AggregateId, DateTime OccurredOn);
}
