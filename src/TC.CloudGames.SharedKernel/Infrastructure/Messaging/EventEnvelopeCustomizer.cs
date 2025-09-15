using Wolverine;
using Wolverine.Runtime;

namespace TC.CloudGames.SharedKernel.Infrastructure.Messaging
{
    public class GenericEventContextEnvelopeCustomizer : IEnvelopeCustomizer
    {
        public void Customize(Envelope envelope, IWolverineRuntime runtime)
        {
            if (envelope.Message is null) return;

            var messageType = envelope.Message.GetType();
            if (!messageType.IsGenericType || messageType.GetGenericTypeDefinition() != typeof(EventContext<>))
                return;

            dynamic context = envelope.Message;

            // Headers dictionary is readonly, but always non-null in Wolverine
            // So just set values directly
            void SetHeader(string key, object? value)
            {
                if (value is not null)
                    envelope.Headers[key] = value!.ToString()!;
            }

            SetHeader("aggregate-type", context.AggregateType);
            SetHeader("aggregate-id", context.AggregateId);
            SetHeader("event-type", context.EventType);
            SetHeader("event-version", context.Version);
            SetHeader("correlation-id", context.CorrelationId ?? string.Empty);
            SetHeader("source", context.Source ?? "Unknown");
            SetHeader("user-id", context.UserId);
            SetHeader("is-authenticated", context.IsAuthenticated);
            SetHeader("occurred-at", context.OccurredAt.ToString("O"));
        }
    }
}
