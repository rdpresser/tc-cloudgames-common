namespace TC.CloudGames.Messaging.Extensions
{
    public static class MessageNameHelper
    {
        public static string DefaultFlattenedMessageName(Type messageType)
        {
            if (!messageType.IsGenericType) return messageType.Name;

            var generic = messageType.GetGenericTypeDefinition().Name;
            var backtick = generic.IndexOf('`');
            if (backtick >= 0) generic = generic[..backtick];

            var inner = messageType.GetGenericArguments()[0].Name;
            return $"{generic}{inner}"; // ex: EventContext + UserCreatedIntegrationEvent => EventContextUserCreatedIntegrationEvent
        }
    }
}
