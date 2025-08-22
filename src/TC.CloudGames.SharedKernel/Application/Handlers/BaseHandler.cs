namespace TC.CloudGames.SharedKernel.Application.Handlers
{
    /// <summary>
    /// Common base handler providing shared functionality for validation and error handling.
    /// Both commands and queries can inherit from this to reduce code duplication.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class BaseHandler<TRequest, TResponse> : CommandHandler<TRequest, Result<TResponse>>
        where TRequest : ICommand<Result<TResponse>>
        where TResponse : class
    {
        protected FastEndpoints.ValidationContext<TRequest> ValidationContext { get; } = Instance;

        #region Validation Helpers

        /// <summary>
        /// Adds a validation error for a specific property.
        /// </summary>
        protected new void AddError(Expression<Func<TRequest, object?>> property, string errorMessage,
            string? errorCode = null, Severity severity = Severity.Error)
        {
            ValidationContext.AddError(property, errorMessage, errorCode, severity);
        }

        /// <summary>
        /// Adds a validation error for a property name as string.
        /// </summary>
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
        /// Adds multiple validation errors.
        /// </summary>
        protected void AddErrors(IEnumerable<ValidationError> validations)
        {
            ValidationContext.ValidationFailures.AddRange(validations.Select(v => new ValidationFailure
            {
                PropertyName = v.Identifier,
                ErrorMessage = v.ErrorMessage,
                ErrorCode = v.ErrorCode,
                Severity = (Severity)v.Severity
            }));
        }

        /// <summary>
        /// Adds multiple validation failures directly.
        /// </summary>
        protected void AddErrors(IEnumerable<ValidationFailure> validations)
        {
            ValidationContext.ValidationFailures.AddRange(validations);
        }

        /// <summary>
        /// Builds a result containing validation errors, ready to return from handler.
        /// </summary>
        protected Result<TResponse> BuildValidationErrorResult()
        {
            if (!ValidationContext.ValidationFailures.Any())
                return Result<TResponse>.Success(default!);

            var errors = ValidationContext.ValidationFailures
                .Select(f => new ValidationError
                {
                    Identifier = f.PropertyName,
                    ErrorCode = f.ErrorCode,
                    ErrorMessage = f.ErrorMessage,
                    Severity = (ValidationSeverity)f.Severity
                }).ToList();

            return Result<TResponse>.Invalid(errors);
        }

        /// <summary>
        /// Builds a NotFound result based on validation failures.
        /// </summary>
        protected Result<TResponse> BuildNotFoundResult()
        {
            if (!ValidationContext.ValidationFailures.Any())
                return Result<TResponse>.Success(default!);

            return Result<TResponse>.NotFound([..ValidationContext.ValidationFailures
                .Select(f => f.ErrorMessage)]);
        }

        /// <summary>
        /// Builds an Unauthorized result based on accumulated validation failures.
        /// </summary>
        protected Result<TResponse> BuildNotAuthorizedResult()
        {
            if (!ValidationContext.ValidationFailures.Any())
                return Result<TResponse>.Success(default!);

            return Result<TResponse>.Unauthorized([..ValidationContext.ValidationFailures
                .Select(f => f.ErrorMessage)]);
        }

        #endregion
    }
}
