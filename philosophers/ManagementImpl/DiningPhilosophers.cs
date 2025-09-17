using ManagementImpl.logger;
using ManagementImpl.manager.impl;
using ManagementImpl.metric;
using ManagementImpl.service.impl;
using Microsoft.Extensions.Logging;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl;

public abstract class DiningPhilosophers
{

    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<DiningPhilosophers>();
    
    public static void SimulateDiscrete(bool enableLog)
    {
        var names = File.ReadAllLines("names.txt");

        var philosopherManagers = new DiscretePhilosopherManager[names.Length];
        var forks = new Fork[names.Length];
        var leftFork = new Fork();
        for (var i = 0; i < names.Length; i++)
        {
            var rightFork = new Fork();
            philosopherManagers[i] = new DiscretePhilosopherManager(
                new Philosopher(names[i], leftFork, rightFork)
            );
            forks[i % names.Length] = leftFork;
            leftFork = rightFork;
        }

        philosopherManagers[names.Length - 1].SetRightFork(philosopherManagers[0].GetLeftFork());

        var metricsCollector = new PhilosopherMetricsCollector(philosopherManagers, forks);
        var strategy = new DiscreteStrategy(philosopherManagers, metricsCollector);
        try
        {
            for (var i = 0; i < 1_000_000; i++)
            {
                strategy.DoStep(i, enableLog);
            }
        }
        finally
        {
            var metrics = metricsCollector.GetFinalStat();
            var statLog = PhilosopherLogger.CreateStatLog(metrics);
            Logger.LogInformation(statLog);
        }
        
    }

    public static void SimulateDiscreteCoordinator(bool enableLog)
    {
        var names = File.ReadAllLines("names.txt");

        var philosopherManagers = new DiscreteCoordinatorPhilosopherManager[names.Length];
        var forks = new Fork[names.Length];
        var leftFork = new Fork();
        for (var i = 0; i < names.Length; i++)
        {
            var rightFork = new Fork();
            philosopherManagers[i] = new DiscreteCoordinatorPhilosopherManager(
                new Philosopher(names[i], leftFork, rightFork)
            );
            forks[i % names.Length] = leftFork;
            leftFork = rightFork;
        }

        philosopherManagers[names.Length - 1].SetRightFork(philosopherManagers[0].GetLeftFork());

        var coordinator = new DiscreteCoordinator(philosopherManagers, forks);
        var strategy = new DiscreteCoordinatorStrategy(coordinator);
        coordinator.SetStrategy(strategy);
        
        try
        {
            for (var i = 0; i < 100000; i++)
            {
                strategy.DoStep(i, enableLog);
            }
        }
        finally 
        {
            var metrics = coordinator.GetFinalMetrics();
            var statLog = PhilosopherLogger.CreateStatLog(metrics);
            Logger.LogInformation(statLog);
        }
    }
}
