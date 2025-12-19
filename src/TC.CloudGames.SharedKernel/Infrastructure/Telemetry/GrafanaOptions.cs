namespace TC.CloudGames.SharedKernel.Infrastructure.Telemetry
{
    /// <summary>
    /// Grafana configuration options for OTLP export via Grafana Agent
    /// </summary>
    public sealed class GrafanaOptions
    {
        /// <summary>
        /// OTLP (OpenTelemetry Protocol) configuration for traces
        /// </summary>
        public OtlpSettings Otlp { get; set; } = new();

        /// <summary>
        /// Grafana Agent configuration (for local agent)
        /// </summary>
        public AgentSettings Agent { get; set; } = new();

        public sealed class OtlpSettings
        {
            /// <summary>
            /// OTLP endpoint URL (default: Grafana Agent on localhost:4317)
            /// </summary>
            public string Endpoint { get; set; } = "http://localhost:4317";

            /// <summary>
            /// OTLP protocol (grpc or http/protobuf)
            /// </summary>
            public string Protocol { get; set; } = "grpc";

            /// <summary>
            /// Authorization headers (format: "key1=value1,key2=value2")
            /// </summary>
            public string? Headers { get; set; }

            /// <summary>
            /// Timeout for OTLP exports (in seconds)
            /// </summary>
            public int TimeoutSeconds { get; set; } = 10;

            /// <summary>
            /// Use insecure connection (no TLS)
            /// </summary>
            public bool Insecure { get; set; } = true;
        }

        public sealed class AgentSettings
        {
            /// <summary>
            /// Grafana Agent hostname
            /// </summary>
            public string Host { get; set; } = "localhost";

            /// <summary>
            /// Grafana Agent OTLP gRPC port (default: 4317)
            /// </summary>
            public int OtlpGrpcPort { get; set; } = 4317;

            /// <summary>
            /// Grafana Agent OTLP HTTP port (default: 4318)
            /// </summary>
            public int OtlpHttpPort { get; set; } = 4318;

            /// <summary>
            /// Grafana Agent metrics port (default: 12345)
            /// </summary>
            public int MetricsPort { get; set; } = 12345;

            /// <summary>
            /// Use Grafana Agent (true) or disable OTLP export (false)
            /// </summary>
            public bool Enabled { get; set; } = true;
        }
    }
}
