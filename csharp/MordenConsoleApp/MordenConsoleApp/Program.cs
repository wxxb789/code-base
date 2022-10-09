namespace MordenConsoleApp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Events;

    using Tomlyn.Extensions.Configuration;

    internal sealed class Program
    {
        private static async Task Main(string[] args) =>
            await CreateHostBuilder(args)
                .Build()
                .RunAsync()
                .ConfigureAwait(false);

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                // Host Config
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddTomlFile("appsettings.toml", optional: false, reloadOnChange: true)
                        .AddTomlFile(
                            $"appsettings.{Environment.GetEnvironmentVariable("MORDENCONSOLEAPP_ENVIRONMENT") ?? "Production"}.toml",
                            optional: true)
                        .AddEnvironmentVariables();
                })
                // Autofac Setup
                .ConfigureContainer<ContainerBuilder>((context, builder) => { })
                // App Config
                .ConfigureAppConfiguration(builder => { })
                // Servies Config
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                    // Set Host Default Service.
                    services.AddHostedService<GreetingService>();
                })
                // Default Logger
                .UseSerilog(new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger());
    }
}