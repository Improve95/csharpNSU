using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.config;
using Service.objects.philosopher;
using Service.service;
using Service.service.impl;

namespace Service;

internal abstract class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("/Users/improvemac/Documents/coding/csharp/csharpNSU/lab3D/Service/appsettings.json", false, true);
            })
            .ConfigureServices((context, services) => 
            {
                var configuration = context.Configuration;
                
                services.Configure<SimulationConfiguration>(
                    configuration.GetSection("Simulation"));

                services.AddSingleton<IForkFactory>(new ForkFactory(5));
                services.AddSingleton<Strategy>();

                services.AddHostedService<Aristotel>();
                services.AddHostedService<Dekart>();
                services.AddHostedService<Kant>();
                services.AddHostedService<Platon>();
                services.AddHostedService<Socrat>();

                services.AddHostedService<SimulationLifetimeService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        await host.RunAsync();
    }
}