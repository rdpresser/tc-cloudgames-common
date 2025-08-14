using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace TC.CloudGames.SharedKernel.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseQueryHandler<TQuery, TResponse> : CommandHandler<TQuery, Result<TResponse>>
            where TQuery : IBaseQuery<TResponse>
            where TResponse : class
    {
        private FastEndpoints.ValidationContext<TQuery> ValidationContext { get; } = Instance;

        /// <summary>
        /// Executes the query asynchronously and returns a result of type Result&lt;TResponse&gt;.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public abstract override Task<Result<TResponse>> ExecuteAsync(TQuery command, CancellationToken ct = default);

        protected Result<TResponse> ValidationErrorNotFound()
        {
            if (ValidationContext.ValidationFailures.Count == 0)
            {
                return Result<TResponse>.Success(default!);
            }

            return Result<TResponse>.NotFound([.. ValidationContext.ValidationFailures.Select(x => x.ErrorMessage)]);
        }

        protected Result<TResponse> ValidationErrorNotAuthorized()
        {
            if (ValidationContext.ValidationFailures.Count == 0)
            {
                return Result<TResponse>.Success(default!);
            }

            return Result<TResponse>.Unauthorized([.. ValidationContext.ValidationFailures.Select(x => x.ErrorMessage)]);
        }

        protected new void AddError(Expression<Func<TQuery, object?>> property, string errorMessage,
            string? errorCode = null, Severity severity = Severity.Error)
        {
            ValidationContext.AddError(property, errorMessage, errorCode, severity);
        }

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
    }
}
