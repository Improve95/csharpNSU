using System.Text;
using IService.objects;
using IService.service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.config;
using Utils.fork;

namespace Service.service;

public class ConcurrentLogger(
    ITableManager tableManager,
    IOptions<SimulationConfiguration> options,
    ILogger<ConcurrentLogger> logger)
    : BackgroundService
{
    private readonly IFork[] _forks = tableManager.Forks;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation(CreateLog());
            await Task.Delay(TimeSpan.FromMilliseconds(options.Value.LogDelay), stoppingToken);
        }
    }
    
    private string CreateLog()
    {
        var sb = new StringBuilder();
        
        sb.Append($"Timestamp: {DateTime.Now.ToString("HH:mm:ss.fff")}\n");
        sb.Append("Philosophers:\n");

        for (var i = 0; i < tableManager.PhilosopherCount; i++)
        {
            var philosopher = tableManager.Philosophers[i];
            sb.Append(string.Format(
                "{0}: {1}, remain: {2}, eating: {3}\n",
                philosopher.Name,
                philosopher.Action.ActionType,
                philosopher.Action.TimeRemain,
                philosopher.TotalEating
            ));
        }
        
        sb.Append("\nForks:\n");
        foreach (var fork in _forks)
        {
            sb.Append(string.Format(
                "Fork-{0}: {1} {2}\n", 
                fork.Id, 
                fork.Status, 
                fork.Status == ForkStatus.Available ? "Available" : "In Use - " + fork.Owner?.Name
            ));
        }
        
        return sb.ToString();
    }
}