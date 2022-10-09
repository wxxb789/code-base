namespace MordenConsoleApp
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class GreetingService : IGreetingService, IHostedService
    {
        private readonly ILogger<GreetingService> _logger;
        private readonly IConfiguration _config;
        private readonly IHostApplicationLifetime _appLifetime;

        public GreetingService(ILogger<GreetingService> logger, IConfiguration config,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _config = config;
            _appLifetime = appLifetime;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                _logger.LogError("Run number {runNumber}", i);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Not started, caused by cancellation requested.");
                return Task.CompletedTask;
            }

            Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task End.");
            return Task.CompletedTask;
        }
    }
}