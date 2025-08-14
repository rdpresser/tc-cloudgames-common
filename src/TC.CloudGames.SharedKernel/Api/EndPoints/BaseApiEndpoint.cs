namespace TC.CloudGames.SharedKernel.Api.EndPoints
{
    public abstract class BaseApiEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
        where TRequest : notnull
    {
        /// <summary>
        /// Handles the result of a query or command execution and sends the appropriate response.
        /// </summary>
        /// <param name="response">The result to handle.</param>
        /// <param name="ct">The cancellation token.</param>
        protected async Task MatchResultAsync(
            Result<TResponse> response,
            CancellationToken ct = default)
        {
            if (response.IsSuccess)
            {
                await Send.OkAsync(response.Value, cancellation: ct).ConfigureAwait(false);
                return;
            }

            if (response.IsNotFound())
            {
                await Send.ErrorsAsync((int)HttpStatusCode.NotFound, ct).ConfigureAwait(false);
                return;
            }

            if (response.IsUnauthorized())
            {
                await Send.ErrorsAsync((int)HttpStatusCode.Unauthorized, ct).ConfigureAwait(false);
                return;
            }

            await Send.ErrorsAsync(cancellation: ct).ConfigureAwait(false);
        }
    }
}
