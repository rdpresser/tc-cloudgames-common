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

    /// <summary>
    /// Gets the index name for operations.
    /// </summary>
    string IndexName { get; }

    /// <summary>
    /// Gets the maximum search size allowed.
    /// </summary>
    int MaxSearchSize { get; }
}