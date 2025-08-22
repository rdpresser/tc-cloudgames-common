namespace TC.CloudGames.SharedKernel.Api.EndPoints
{
    public abstract class BaseApiEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
        where TRequest : notnull
    {
        private bool _responseSet;

        public override Task OnBeforeHandleAsync(TRequest req, CancellationToken ct)
        {
            EnsureResponse();
            return base.OnBeforeHandleAsync(req, ct);
        }

        private void EnsureResponse()
        {
            if (_responseSet) return;

            Response = CreateResponse();
            _responseSet = true;
        }

        private static TResponse CreateResponse()
        {
            try
            {
                return Activator.CreateInstance<TResponse>()!;
            }
            catch
            {
                return default!;
            }
        }

        /// <summary>
        /// Handles the result of a query or command execution and sends the appropriate response.
        /// </summary>
        protected async Task MatchResultAsync(Result<TResponse> response, CancellationToken ct = default)
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
