using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace TC.CloudGames.SharedKernel.Infrastructure.Elasticsearch;

/// <summary>
/// Fake implementation of IElasticsearchClientProvider for when Elasticsearch is disabled.
/// Logs operations instead of executing them.
/// </summary>
public sealed class FakeElasticsearchClientProvider : IElasticsearchClientProvider
{
    private readonly ILogger<FakeElasticsearchClientProvider> _logger;
    private readonly Lazy<ElasticsearchClient> _client;

    public FakeElasticsearchClientProvider(ILogger<FakeElasticsearchClientProvider> logger)
    {
        _logger = logger;
        _client = new Lazy<ElasticsearchClient>(() =>
        {
            _logger.LogWarning("?? Elasticsearch is DISABLED - Creating fake client that will log operations instead of executing them");
            
            // Create a minimal fake client - it won't be used for actual operations
#pragma warning disable S1075 // URIs should not be hardcoded - This is intentional for fake client
            var fakeUri = new Uri("http://localhost:9200"); // Local fallback URI for fake client
#pragma warning restore S1075
            return new ElasticsearchClient(new ElasticsearchClientSettings(fakeUri));
        });
    }

    /// <summary>
    /// Gets a fake Elasticsearch client.
    /// Operations will be logged but not executed.
    /// </summary>
    public ElasticsearchClient Client
    {
        get
        {
            _logger.LogDebug("?? Elasticsearch DISABLED - Returning fake client");
            return _client.Value;
        }
    }

    /// <summary>
    /// Gets the fake index name.
    /// </summary>
    public string IndexName => "fake-index";

    /// <summary>
    /// Gets the fake max search size.
    /// </summary>
    public int MaxSearchSize => 1000;
}