using Microsoft.Extensions.Configuration;

namespace TC.CloudGames.SharedKernel.Infrastructure.Telemetry
{
    /// <summary>
    /// Helper for building Grafana configuration from appsettings and environment variables
    /// Priority: Environment Variables > appsettings.json > Defaults
    /// </summary>
    public sealed class GrafanaHelper
    {
        private const string GrafanaSectionName = "Grafana";

        public GrafanaOptions GrafanaSettings { get; }

        public GrafanaHelper(IConfiguration configuration)
        {
            // Bind section "Grafana" ? GrafanaOptions
            GrafanaSettings = configuration.GetSection(GrafanaSectionName).Get<GrafanaOptions>()
                               ?? new GrafanaOptions();

            // Override with environment variables (priority)
            ApplyEnvironmentOverrides();
        }

        private void ApplyEnvironmentOverrides()
        {
            // ============================================
            // OTLP Configuration
            // ============================================
            GrafanaSettings.Otlp.Endpoint = GetEnvOrDefault(
                "OTEL_EXPORTER_OTLP_ENDPOINT",
                GrafanaSettings.Otlp.Endpoint) ?? GrafanaSettings.Otlp.Endpoint;

            GrafanaSettings.Otlp.Protocol = GetEnvOrDefault(
                "OTEL_EXPORTER_OTLP_PROTOCOL",
                GrafanaSettings.Otlp.Protocol) ?? GrafanaSettings.Otlp.Protocol;

            GrafanaSettings.Otlp.Headers = GetEnvOrDefault(
                "OTEL_EXPORTER_OTLP_HEADERS",
                GrafanaSettings.Otlp.Headers);

            GrafanaSettings.Otlp.TimeoutSeconds = GetEnvIntOrDefault(
                "OTEL_EXPORTER_OTLP_TIMEOUT",
                GrafanaSettings.Otlp.TimeoutSeconds);

            GrafanaSettings.Otlp.Insecure = GetEnvBoolOrDefault(
                "OTEL_EXPORTER_OTLP_INSECURE",
                GrafanaSettings.Otlp.Insecure);

            // ============================================
            // Agent Configuration
            // ============================================
            GrafanaSettings.Agent.Host = GetEnvOrDefault(
                "GRAFANA_AGENT_HOST",
                GrafanaSettings.Agent.Host) ?? GrafanaSettings.Agent.Host;

            GrafanaSettings.Agent.OtlpGrpcPort = GetEnvIntOrDefault(
                "GRAFANA_AGENT_OTLP_GRPC_PORT",
                GrafanaSettings.Agent.OtlpGrpcPort);

            GrafanaSettings.Agent.OtlpHttpPort = GetEnvIntOrDefault(
                "GRAFANA_AGENT_OTLP_HTTP_PORT",
                GrafanaSettings.Agent.OtlpHttpPort);

            GrafanaSettings.Agent.MetricsPort = GetEnvIntOrDefault(
                "GRAFANA_AGENT_METRICS_PORT",
                GrafanaSettings.Agent.MetricsPort);

            GrafanaSettings.Agent.Enabled = GetEnvBoolOrDefault(
                "GRAFANA_AGENT_ENABLED",
                GrafanaSettings.Agent.Enabled);

            // ============================================
            // Build OTLP Endpoint if using Agent
            // ============================================
            if (GrafanaSettings.Agent.Enabled)
            {
                // Override OTLP endpoint to point to local Grafana Agent
                var protocol = (GrafanaSettings.Otlp.Protocol ?? "grpc").ToLowerInvariant();
                var port = protocol == "grpc"
                    ? GrafanaSettings.Agent.OtlpGrpcPort
                    : GrafanaSettings.Agent.OtlpHttpPort;

                var scheme = GrafanaSettings.Otlp.Insecure ? "http" : "https";
                GrafanaSettings.Otlp.Endpoint = $"{scheme}://{GrafanaSettings.Agent.Host}:{port}";
            }
        }

        /// <summary>
        /// Static factory method for convenience
        /// </summary>
        public static GrafanaOptions Build(IConfiguration configuration) =>
            new GrafanaHelper(configuration).GrafanaSettings;

        // ============================================
        // Helper Methods
        // ============================================

        private static string? GetEnvOrDefault(string key, string? current)
        {
            var envValue = Environment.GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(envValue) ? current : envValue;
        }

        private static int GetEnvIntOrDefault(string key, int current)
        {
            var raw = Environment.GetEnvironmentVariable(key);
            return int.TryParse(raw, out var value) ? value : current;
        }

        private static bool GetEnvBoolOrDefault(string key, bool current)
        {
            var raw = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(raw)) return current;

            return raw.Trim().ToLowerInvariant() switch
            {
                "1" or "true" or "yes" or "y" => true,
                "0" or "false" or "no" or "n" => false,
                _ => bool.TryParse(raw, out var parsed) ? parsed : current
            };
        }
    }
}
