using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.config;

namespace Service.service.impl;

public class SimulationLifetimeService(
    IHostApplicationLifetime lifetime,
    IOptions<SimulationConfiguration> options,
    ILogger<SimulationLifetimeService> logger)
    : BackgroundService
{
    private readonly SimulationConfiguration _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Simulation started. Duration = {DurationSeconds} seconds", _options.DurationSeconds);

        try 
        {
            await Task.Delay(TimeSpan.FromSeconds(_options.DurationSeconds), stoppingToken);
        }
        catch (TaskCanceledException)
        {
            
            return;
        }
        
        logger.LogInformation("Simulation time elapsed. Stopping application...");
        lifetime.StopApplication();
    }
}
