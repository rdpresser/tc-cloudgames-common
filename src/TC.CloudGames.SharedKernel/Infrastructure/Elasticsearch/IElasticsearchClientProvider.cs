using Elastic.Clients.Elasticsearch;

namespace TC.CloudGames.SharedKernel.Infrastructure.Elasticsearch;

/// <summary>
/// Interface for Elasticsearch client provider.
/// </summary>
public interface IElasticsearchClientProvider
{
    /// <summary>
    /// Gets a configured Elasticsearch client.
    /// </summary>
    ElasticsearchClient Client { get; }
}