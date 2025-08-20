using TC.CloudGames.SharedKernel.Infrastructure.UserClaims;

namespace TC.CloudGames.SharedKernel.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseCommandHandler<TCommand, TResponse, TAggregate, TRepository> : CommandHandler<TCommand, Result<TResponse>>
        where TCommand : IBaseCommand<TResponse>
        where TResponse : class
        where TAggregate : BaseAggregateRoot
        where TRepository : IBaseRepository<TAggregate>
    {
        protected TRepository Repository { get; }
        protected IUserContext UserContext { get; }

        private FastEndpoints.ValidationContext<TCommand> ValidationContext { get; } = Instance;

        protected BaseCommandHandler(TRepository repository, IUserContext userContext)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository), "Repository cannot be null");
            UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext), "User context cannot be null");
        }

        /// <summary>
        /// Executes the command asynchronously and returns a result of type Result&lt;TResponse&gt;.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public abstract override Task<Result<TResponse>> ExecuteAsync(TCommand command, CancellationToken ct = default);

        /// <summary>
        /// Adds a validation error to the context for a specific property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="severity"></param>
        protected new void AddError(Expression<Func<TCommand, object?>> property, string errorMessage,
            string? errorCode = null, Severity severity = Severity.Error)
        {
            ValidationContext.AddError(property, errorMessage, errorCode, severity);
        }

        /// <summary>
        /// Adds a validation error to the context for a specific property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorCode"></param>
        /// <param name="severity"></param>
        protected void AddError(string property, string errorMessage, string? errorCode = null,
            Severity severity = Severity.Error)
        {
            ValidationContext.AddError(new ValidationFailure
            {
                PropertyName = property,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                Severity = severity
            });
        }

        /// <summary>
        /// Adds a list of validation errors to the context.
        /// </summary>
        /// <param name="validations"></param>
        protected void AddErrors(IEnumerable<ValidationError> validations)
        {
            ValidationContext.ValidationFailures.AddRange(validations.Select(validation =>
                new ValidationFailure
                {
                    PropertyName = validation.Identifier,
                    ErrorMessage = validation.ErrorMessage,
                    ErrorCode = validation.ErrorCode,
                    Severity = (Severity)validation.Severity
                }));
        }

        /// <summary>
        /// Adds a list of validation failures to the context.
        /// </summary>
        /// <param name="validations"></param>
        protected void AddErrors(IEnumerable<ValidationFailure> validations)
        {
            ValidationContext.ValidationFailures.AddRange(validations);
        }

        /// <summary>
        /// Creates a result with validation errors to Result<typeparamref name="TResponse"/>.
        /// </summary>
        /// <returns></returns>
        protected Result<TResponse> ValidationErrorsInvalid()
        {
            if (ValidationContext.ValidationFailures.Count == 0)
            {
                return Result<TResponse>.Success(default!);
            }

            List<ValidationError> validationErrors = [];

            validationErrors.AddRange(ValidationContext.ValidationFailures
                .Select(x => new ValidationError
                {
                    Identifier = x.PropertyName,
                    ErrorCode = x.ErrorCode,
                    ErrorMessage = x.ErrorMessage,
                    Severity = (ValidationSeverity)x.Severity
                }));

            return Result<TResponse>.Invalid(validationErrors);
        }

        /// <summary>
        /// Creates a result with not found error messages.
        /// </summary>
        /// <returns></returns>
        protected Result<TResponse> ValidationErrorNotFound()
        {
            if (ValidationContext.ValidationFailures.Count == 0)
            {
                return Result<TResponse>.Success(default!);
            }

            return Result<TResponse>.NotFound([.. ValidationContext.ValidationFailures.Select(x => x.ErrorMessage)]);
        }
    }
}
