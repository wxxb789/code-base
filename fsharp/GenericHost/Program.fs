module GenericHostSample

open System
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open System.IO
open TimedHostedService
open LifeTimeEventsHostedService

[<EntryPoint>]
let main argv =

    let hostConfig (hostCfg:IConfigurationBuilder) =
        hostCfg.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hostsettings", optional = true)
            .AddEnvironmentVariables(prefix = "PREFIX_")
            .AddCommandLine(argv)
            |> ignore

    let appConfig (host: HostBuilderContext) (appCfg: IConfigurationBuilder) =
        appCfg.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional = true)
            .AddJsonFile(sprintf "appsettings.%s.json" (host.HostingEnvironment.EnvironmentName), optional = true)
            .AddEnvironmentVariables(prefix =  "PREFIX_")
            .AddCommandLine(argv)
            |> ignore

    let serviceConfig (host: HostBuilderContext) (serviceCfg: IServiceCollection) =
        serviceCfg.AddLogging()
            .AddHostedService<LifetimeEventsHostedServices>()
            .AddHostedService<TimedHostedService>()
            |> ignore

    let loggingConfig (host: HostBuilderContext) (loggingCfg: ILoggingBuilder) =
        loggingCfg.AddConsole()
            .AddDebug()
            |> ignore

    let host =
        HostBuilder()
            .ConfigureHostConfiguration(Action<IConfigurationBuilder> hostConfig)
            .ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> appConfig)
            .ConfigureServices(Action<HostBuilderContext, IServiceCollection> serviceConfig)
            .ConfigureLogging(Action<HostBuilderContext, ILoggingBuilder> loggingConfig)
            .Build()

    host.RunAsync() |> Async.AwaitTask |> Async.RunSynchronously

    0 // return an integer exit code