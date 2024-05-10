#load "reference.csx"

#r "nuget: Polly, 7.2.2"
#r "nuget: Flurl.Http, 3.0.1"
#r "nuget: Dasync.Collections, 1.0.0"

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

const string QueriesFilePath = "path_to_queries_file";
const string MachinesFilePath = "path_to_machines_file";
const string ResultFolderPath = "path_to_result_folder";
const string ApiPath = "your_api_path_here"; // Assuming this is a placeholder
const int MaxDegreeOfParallelism = 32;

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

if (!ValidateFiles(QueriesFilePath, MachinesFilePath) || !EnsureDirectoryExists(ResultFolderPath))
{
    return;
}

var retryPolicy = GetRetryPolicy(logger);

var machineLines = await File.ReadAllLinesAsync(MachinesFilePath);
await ProcessQueriesInParallel(QueriesFilePath, machineLines, ResultFolderPath, retryPolicy, logger);

static bool ValidateFiles(string queriesFilePath, string machinesFilePath)
{
    if (!File.Exists(queriesFilePath) || !File.Exists(machinesFilePath))
    {
        logger.LogError("Files do not exist.");
        return false;
    }
    return true;
}

static bool EnsureDirectoryExists(string resultFolderPath)
{
    if (!Directory.Exists(resultFolderPath))
    {
        Directory.CreateDirectory(resultFolderPath);
    }
    return true;
}

static IAsyncPolicy GetRetryPolicy(ILogger logger) => Policy
    .Handle<Exception>()
    .WaitAndRetryAsync(new[]
    {
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(4),
        TimeSpan.FromSeconds(8),
        TimeSpan.FromSeconds(16),
        TimeSpan.FromSeconds(32),
    }, (exception, timeSpan, retryCount, context) =>
    {
        logger.LogWarning($"Retry {retryCount} due to {exception.Message}");
    });

static async Task ProcessQueriesInParallel(string queriesFilePath, string[] machineLines, string resultFolderPath, IAsyncPolicy retryPolicy, ILogger logger)
{
    var startTime = Stopwatch.StartNew();
    var random = new Random();

    var queryMarketLines = await File.ReadAllLinesAsync(queriesFilePath);
    await queryMarketLines.ParallelForEachAsync(async line =>
    {
        var columns = line.Split('\t');
        if (columns.Length < 2)
        {
            logger.LogWarning("Wrong line format: {@Line}", line);
            return;
        }
        var query = columns[0].Trim();
        var market = columns[1].Trim();

        foreach (var machine in machineLines)
        {
            var currMachine = machine.Trim();
            if (string.IsNullOrEmpty(currMachine))
            {
                continue;
            }

            foreach (var i in Enumerable.Range(0, 3))
            {
                logger.LogInformation($"Processing: {query}_{market}_{currMachine}_{i}");

                try
                {
                    var result = await retryPolicy.ExecuteAsync(() =>
                        $"{ApiPath}/search".SetQueryParams(new { q = query, mkt = market, atlahostname = currMachine }).GetStringAsync());

                    var outputFilePath = GetFilePath(query, market, currMachine, i, resultFolderPath, random);
                    await File.WriteAllTextAsync(outputFilePath, result);
                    logger.LogInformation($"Result written to {outputFilePath}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "HTTP request failed.");
                }
            }
        }
    }, maxDegreeOfParallelism: MaxDegreeOfParallelism);

    startTime.Stop();
    logger.LogInformation($"Total execution time: {startTime.Elapsed}");
}

static string GetFilePath(string query, string market, string machine, int times, string folder, Random random)
{
    var fileName = $"{ComputeSha256Hash($"{query}_{market}_{machine}_{times}")}_{random.Next(10000):0000}.html";
    return Path.Combine(folder, fileName);
}

static string ComputeSha256Hash(string rawData)
{
    using var sha256Hash = SHA256.Create();
    var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
    return string.Concat(bytes.Select(b => b.ToString("x2")));
}