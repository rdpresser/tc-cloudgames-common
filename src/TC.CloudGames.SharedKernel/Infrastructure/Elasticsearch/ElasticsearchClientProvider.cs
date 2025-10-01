using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.Elasticsearch;

/// <summary>
/// Provider for creating and configuring Elasticsearch clients.
/// Supports both Elasticsearch Cloud Serverless and local development.
/// </summary>
public sealed class ElasticsearchClientProvider : IElasticsearchClientProvider
{
    private readonly ElasticSearchOptions _options;
    private readonly Lazy<ElasticsearchClient> _client;

    public ElasticsearchClientProvider(IOptions<ElasticSearchOptions> options)
    {
        _options = options.Value;
        _client = new Lazy<ElasticsearchClient>(() => CreateElasticsearchClient(_options));
    }

    /// <summary>
    /// Gets a configured Elasticsearch client based on the options.
    /// </summary>
    public ElasticsearchClient Client => _client.Value;

    /// <summary>
    /// Creates an Elasticsearch client based on the provided options.
    /// </summary>
    /// <param name="options">Elasticsearch configuration options</param>
    /// <returns>Configured Elasticsearch client</returns>
    private static ElasticsearchClient CreateElasticsearchClient(ElasticSearchOptions options)
    {
        options.Validate();

        var connectionUrl = options.GetConnectionUrl();
        var uri = new Uri(connectionUrl);

        var settings = new ElasticsearchClientSettings(uri)
            .DefaultIndex(options.IndexName)
            .RequestTimeout(TimeSpan.FromSeconds(options.RequestTimeoutSeconds))
            .PingTimeout(TimeSpan.FromSeconds(10))
            .DeadTimeout(TimeSpan.FromMinutes(2))
            .MaxDeadTimeout(TimeSpan.FromMinutes(5));

        // Configure authentication based on environment
        if (options.IsElasticCloud && !string.IsNullOrWhiteSpace(options.ApiKey))
        {
            // Use API Key authentication for Elasticsearch Cloud (both regular and serverless)
            settings = settings.Authentication(new ApiKey(options.ApiKey!));
        }
        else if (options.IsLocal && !string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
        {
            // Use Basic authentication for local development
            settings = settings.Authentication(new BasicAuthentication(options.Username!, options.Password!));
        }

        // Enable detailed diagnostics in development
        if (options.IsLocal)
        {
            settings = settings
                .DisableDirectStreaming()
                .PrettyJson()
                .EnableDebugMode();
        }

        return new ElasticsearchClient(settings);
    }

    /// <summary>
    /// Static factory method for creating Elasticsearch client from options.
    /// Maintains backward compatibility.
    /// </summary>
    /// <param name="options">Elasticsearch configuration options</param>
    /// <returns>Configured Elasticsearch client</returns>
    public static ElasticsearchClient Build(IOptions<ElasticSearchOptions> options)
    {
        return CreateElasticsearchClient(options.Value);
    }

    /// <summary>
    /// Static factory method for creating Elasticsearch client from configuration.
    /// </summary>
    /// <param name="configuration">Configuration containing Elasticsearch section</param>
    /// <returns>Configured Elasticsearch client</returns>
    public static ElasticsearchClient Build(IConfiguration configuration)
    {
        var options = configuration.GetSection("Elasticsearch").Get<ElasticSearchOptions>() ?? new ElasticSearchOptions();
        return CreateElasticsearchClient(options);
    }
}