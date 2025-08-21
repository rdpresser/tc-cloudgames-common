using Wolverine;
using Wolverine.Runtime;
using Wolverine.Runtime.Routing;

namespace TC.CloudGames.SharedKernel.Infrastructure.Messaging
{
    public class EventContextRoutingConvention : IMessageRoutingConvention
    {
        public static string? DetermineRoutingKey(Type messageType, Envelope envelope)
        {
            if (!messageType.IsGenericType || messageType.GetGenericTypeDefinition() != typeof(EventContext<,>))
                return null;

            if (envelope.Message is null)
                return null;

            dynamic context = envelope.Message;
            var aggregateType = context.AggregateType.Replace("Aggregate", "").ToLowerInvariant();
            var eventType = context.EventType.Replace("Event", "").ToLowerInvariant();
            return $"{aggregateType}.{eventType}";
        }

        public void DiscoverListeners(IWolverineRuntime runtime, IReadOnlyList<Type> handledMessageTypes)
        {
            // Not needed for outbound-only routing keys
        }

        public IEnumerable<Wolverine.Configuration.Endpoint> DiscoverSenders(Type messageType, IWolverineRuntime runtime)
        {
            // Not needed for outbound-only routing keys
            return Enumerable.Empty<Wolverine.Configuration.Endpoint>();
        }
    }
}
