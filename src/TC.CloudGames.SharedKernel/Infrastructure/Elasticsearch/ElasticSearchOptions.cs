namespace TC.CloudGames.SharedKernel.Infrastructure.Elasticsearch;

/// <summary>
/// Configuration options for Elasticsearch connection.
/// Supports local Docker instances, Elasticsearch Cloud, and Elasticsearch Cloud Serverless.
/// </summary>
public class ElasticSearchOptions
{
    /// <summary>
    /// Elasticsearch Cloud Project ID (for Serverless only).
    /// Leave empty for regular Elasticsearch Cloud.
    /// </summary>
    public string? ProjectId { get; set; }

    /// <summary>
    /// API Key for Elasticsearch Cloud authentication.
    /// Should be base64 encoded API key from Elasticsearch Cloud.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Complete Elasticsearch endpoint URL.
    /// For Elastic Cloud: https://your-project.es.region.provider.elastic.cloud:443
    /// For Serverless: Automatically constructed from ProjectId + Region
    /// For Custom: Any valid Elasticsearch URL
    /// </summary>
    public string? EndpointUrl { get; set; }

    /// <summary>
    /// Local Elasticsearch host (for Docker/local development).
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// Local Elasticsearch port (for Docker/local development).
    /// </summary>
    public int Port { get; set; } = 9200;

    /// <summary>
    /// Username for basic authentication (local development).
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for basic authentication (local development).
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Index prefix for multi-tenant scenarios.
    /// </summary>
    public string IndexPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Complete index name.
    /// </summary>
    public string IndexName => IndexPrefix;

    /// <summary>
    /// Request timeout in seconds.
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Maximum number of search results to return.
    /// </summary>
    public int MaxSearchSize { get; set; } = 1000;

    /// <summary>
    /// Determines if this is using Elasticsearch Cloud (with API Key).
    /// </summary>
    public bool IsElasticCloud => !string.IsNullOrEmpty(ApiKey) &&
                                  (!string.IsNullOrEmpty(EndpointUrl) || !string.IsNullOrEmpty(ProjectId));

    /// <summary>
    /// Determines if this is a Serverless configuration.
    /// </summary>
    public bool IsServerless => !string.IsNullOrEmpty(ProjectId) && !string.IsNullOrEmpty(ApiKey) && string.IsNullOrEmpty(EndpointUrl);

    /// <summary>
    /// Determines if this is a local development configuration.
    /// </summary>
    public bool IsLocal => !IsElasticCloud && !string.IsNullOrEmpty(Host);

    /// <summary>
    /// Gets the full connection URL for Elasticsearch.
    /// </summary>
    /// <returns>The connection URL</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration is invalid</exception>
    public string GetConnectionUrl()
    {
        // Direct endpoint URL (highest priority)
        if (!string.IsNullOrEmpty(EndpointUrl))
            return EndpointUrl;

        // Local configuration
        if (IsLocal)
            return $"http://{Host}:{Port}";

        throw new InvalidOperationException(
            "Invalid Elasticsearch configuration. Please provide either:" +
            "\n- EndpointUrl + ApiKey (for Elasticsearch Cloud)" +
            "\n- Host + Port (for local development)");
    }

    /// <summary>
    /// Validates the configuration.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when configuration is invalid</exception>
    public void Validate()
    {
        if (IsElasticCloud)
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for Elasticsearch Cloud");

            if (IsServerless)
            {
                if (string.IsNullOrWhiteSpace(ProjectId))
                    throw new InvalidOperationException("ProjectId is required for Serverless configuration");

            }
            else if (string.IsNullOrWhiteSpace(EndpointUrl))
            {
                throw new InvalidOperationException("EndpointUrl is required for regular Elasticsearch Cloud configuration");
            }
        }
        else if (IsLocal)
        {
            if (string.IsNullOrWhiteSpace(Host))
                throw new InvalidOperationException("Host is required for local configuration");
        }
        else
        {
            throw new InvalidOperationException("No valid Elasticsearch configuration found");
        }

        if (string.IsNullOrWhiteSpace(IndexPrefix))
            throw new InvalidOperationException("IndexPrefix cannot be empty");
    }
}