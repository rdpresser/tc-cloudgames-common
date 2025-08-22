using TC.CloudGames.SharedKernel.Infrastructure.UserClaims;

namespace TC.CloudGames.SharedKernel.Application.Handlers
{
    /// <summary>
    /// Base class for command handlers, adding aggregate persistence, domain events, and outbox hooks.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class BaseCommandHandler<TCommand, TResponse, TAggregate, TRepository>
        : BaseHandler<TCommand, TResponse>
        where TCommand : IBaseCommand<TResponse>
        where TResponse : class
        where TAggregate : BaseAggregateRoot
        where TRepository : IBaseRepository<TAggregate>
    {
        protected TRepository Repository { get; }
        protected IUserContext UserContext { get; }

        protected BaseCommandHandler(TRepository repository, IUserContext userContext)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        public abstract override Task<Result<TResponse>> ExecuteAsync(TCommand command, CancellationToken ct = default);

        #region Optional Hooks for Derived Command Handlers

        protected abstract Task<Result<TAggregate>> MapCommandToAggregateAsync(TCommand command);
        protected virtual Task<Result> ValidateAggregateAsync(TAggregate aggregate) => Task.FromResult(Result.Success());
        protected virtual Task PublishIntegrationEventsAsync(TAggregate aggregate) => Task.CompletedTask;

        #endregion
    }
}
