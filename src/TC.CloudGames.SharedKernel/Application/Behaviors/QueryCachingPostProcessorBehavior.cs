namespace TC.CloudGames.SharedKernel.Application.Behaviors
{
    [ExcludeFromCodeCoverage]
    public class QueryCachingPostProcessorBehavior
    {
        // This class is intended to process the results of queries after they have been executed.
        // It can be used to apply additional transformations, caching, or logging to the results.
        // Implement methods as needed to handle post-processing logic for query results.

        //
        //IMPLEMENTAR A MESMA COISA QUE O COMMAND POST PROCESSOR E INCLUIR ADICIONAR RESULTADO NO CACHE USANDO A CACHEKEY
        //
        // Example method:
        public void ProcessResults<T>(IEnumerable<T> results)
        {
            // Implement your post-processing logic here
            // For example, you could log the results or apply some transformations
        }
    }
}
