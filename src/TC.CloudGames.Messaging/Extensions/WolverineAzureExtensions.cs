namespace TC.CloudGames.Messaging.Extensions;

public static class WolverineAzureExtensions
{
    public static AzureServiceBusConfiguration ConfigureAzureServiceBus(
        this WolverineOptions opts,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        var sb = AzureServiceBusHelper.Build(configuration);
        return ConfigureInternal(opts, sb, env);
    }

    public static AzureServiceBusConfiguration ConfigureAzureServiceBus(
        this WolverineOptions opts,
        AzureServiceBusOptions options,
        IWebHostEnvironment env)
    {
        return ConfigureInternal(opts, options, env);
    }

    // -------------------------------
    // 🔒 Centralized private method
    // -------------------------------
    private static AzureServiceBusConfiguration ConfigureInternal(
        WolverineOptions opts,
        AzureServiceBusOptions sb,
        IWebHostEnvironment env)
    {
        var identifier = env.ApplicationName ?? "WolverineApp";

        if (env.IsDevelopment() && !string.IsNullOrWhiteSpace(sb.ConnectionString))
        {
            return opts.UseAzureServiceBus(sb.ConnectionString, cfg =>
            {
                cfg.Identifier = identifier;
            });
        }

        if (string.IsNullOrWhiteSpace(sb.Namespace))
        {
            throw new ArgumentNullException(nameof(sb),
                "ServiceBus:Namespace configuration value is required for MSI authentication.");
        }

        return opts.UseAzureServiceBus(sb.Namespace, new DefaultAzureCredential(), cfg =>
        {
            cfg.Identifier = identifier;
        });
    }
}