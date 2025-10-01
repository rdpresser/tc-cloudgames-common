using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.Elasticsearch;

/// <summary>
/// Provider for creating and configuring Elasticsearch clients.
/// Supports both Elasticsearch Cloud Serverless and local development.
/// Reads configuration from environment variables first, then appsettings.
/// </summary>
public sealed class ElasticsearchClientProvider : IElasticsearchClientProvider
{
    private const string ElasticsearchSectionName = "Elasticsearch";

    private readonly ElasticSearchOptions _options;
    private readonly Lazy<ElasticsearchClient> _client;

    public string IndexName { get; private set; }
    public int MaxSearchSize { get; private set; }


    public ElasticsearchClientProvider(IOptions<ElasticSearchOptions> options)
    {
        // Start with options from IOptions<T> (appsettings.json)
        _options = new ElasticSearchOptions
        {
            ProjectId = options.Value.ProjectId,
            ApiKey = options.Value.ApiKey,
            EndpointUrl = options.Value.EndpointUrl,
            Host = options.Value.Host,
            Port = options.Value.Port,
            Username = options.Value.Username,
            Password = options.Value.Password,
            IndexPrefix = options.Value.IndexPrefix,
            RequestTimeoutSeconds = options.Value.RequestTimeoutSeconds,
            MaxSearchSize = options.Value.MaxSearchSize
        };

        // Always apply environment variables overrides (highest priority)
        ApplyEnvironmentVariables(_options);

        IndexName = _options.IndexPrefix;
        MaxSearchSize = _options.MaxSearchSize;

        _client = new Lazy<ElasticsearchClient>(() => CreateElasticsearchClient(_options));
    }

    /// <summary>
    /// Gets a configured Elasticsearch client based on the options.
    /// </summary>
    public ElasticsearchClient Client => _client.Value;

    /// <summary>
    /// Applies environment variable overrides to ElasticSearchOptions.
    /// Environment variables always take precedence over IOptions configuration.
    /// </summary>
    /// <param name="options">The options to modify</param>
    private static void ApplyEnvironmentVariables(ElasticSearchOptions options)
    {
        // Override string properties with environment variables if present
        options.EndpointUrl = GetEnvOrDefault("ELASTICSEARCH_ENDPOINTURL", options.EndpointUrl);
        options.ApiKey = GetEnvOrDefault("ELASTICSEARCH_APIKEY", options.ApiKey);
        options.ProjectId = GetEnvOrDefault("ELASTICSEARCH_PROJECTID", options.ProjectId);
        options.Host = GetEnvOrDefault("ELASTICSEARCH_HOST", options.Host);
        options.Username = GetEnvOrDefault("ELASTICSEARCH_USERNAME", options.Username);
        options.Password = GetEnvOrDefault("ELASTICSEARCH_PASSWORD", options.Password);

        // Handle non-nullable IndexPrefix separately
        var indexPrefix = GetEnvOrDefault("ELASTICSEARCH_INDEXPREFIX", options.IndexPrefix);
        if (!string.IsNullOrWhiteSpace(indexPrefix))
        {
            options.IndexPrefix = indexPrefix;
        }

        // Override int properties with environment variables if present
        options.Port = GetEnvIntOrDefault("ELASTICSEARCH_PORT", options.Port);
        options.RequestTimeoutSeconds = GetEnvIntOrDefault("ELASTICSEARCH_REQUEST_TIMEOUT_SECONDS", options.RequestTimeoutSeconds);
        options.MaxSearchSize = GetEnvIntOrDefault("ELASTICSEARCH_MAX_SEARCH_SIZE", options.MaxSearchSize);
    }

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
        var provider = new ElasticsearchClientProvider(options);
        return provider.Client;
    }

    /// <summary>
    /// Static factory method for creating Elasticsearch client from configuration.
    /// Includes environment variable support.
    /// </summary>
    /// <param name="configuration">Configuration containing Elasticsearch section</param>
    /// <returns>Configured Elasticsearch client</returns>
    public static ElasticsearchClient Build(IConfiguration configuration)
    {
        var options = BuildOptions(configuration);
        return CreateElasticsearchClient(options);
    }

    /// <summary>
    /// Static convenience method to get configured Elasticsearch options with environment variable support.
    /// Follows the same pattern as ConnectionStringProvider and RabbitMqHelper.
    /// </summary>
    /// <param name="configuration">Configuration containing Elasticsearch section</param>
    /// <returns>Configured ElasticSearchOptions</returns>
    public static ElasticSearchOptions BuildOptions(IConfiguration configuration)
    {
        // Start with configuration from appsettings
        var options = configuration.GetSection(ElasticsearchSectionName).Get<ElasticSearchOptions>()
                      ?? new ElasticSearchOptions();

        // Always apply environment variables overrides (highest priority)
        ApplyEnvironmentVariables(options);

        return options;
    }

    // --------------------------------------------------
    // Environment variable helper methods (same pattern as RabbitMqHelper)
    // --------------------------------------------------

    private static string? GetEnvOrDefault(string key, string? current) =>
        string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key))
            ? current
            : Environment.GetEnvironmentVariable(key);

    private static int GetEnvIntOrDefault(string key, int current)
    {
        var raw = Environment.GetEnvironmentVariable(key);
        return int.TryParse(raw, out var value) ? value : current;
    }
}