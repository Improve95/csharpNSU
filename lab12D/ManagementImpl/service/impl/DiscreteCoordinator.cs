using ManagementImpl.logger;
using ManagementImpl.manager;
using ManagementImpl.manager.impl;
using ManagementImpl.metric;
using Microsoft.Extensions.Logging;
using philosophers.action.impl;
using philosophers.objects.fork;
using strategy.service;
using static ManagementImpl.metric.DiscretePhilosopherMetricsCollector;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinator: IDiscreteCoordinator
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<DiscreteCoordinator>();
    
    private readonly DiscreteCoordinatorPhilosopherManager[] _managers;

    private readonly DiscreteFork[] _forks;

    private DiscreteCoordinatorStrategy _strategy;

    private readonly DiscretePhilosopherMetricsCollector _metricsCollector;
    
    public delegate void StartHungryEvent(DiscreteCoordinatorPhilosopherManager manager);
    public static event StartHungryEvent? StartHungryNotify;
    
    public delegate void GetForkEvent(DiscreteCoordinatorPhilosopherManager manager, IFork fork);
    public static event GetForkEvent? GetForkNotify;
    
    public delegate void StartEatingEvent(DiscreteCoordinatorPhilosopherManager manager);
    public static event StartEatingEvent? StartEatingNotify;
    
    public delegate void ReleaseForkImmediatelyEvent(DiscreteCoordinatorPhilosopherManager manager, IFork fork);
    public static event ReleaseForkImmediatelyEvent? ReleaseForkImmediatelyNotify;
    
    public delegate void StartThinkingEvent(DiscreteCoordinatorPhilosopherManager manager);
    public static event StartThinkingEvent? StartThinkingNotify;
    
    public DiscreteCoordinator(DiscreteCoordinatorPhilosopherManager[] managers, DiscreteFork[] forks) 
    {
        _managers = managers;
        _forks = forks;
        _metricsCollector = new DiscretePhilosopherMetricsCollector(managers, forks);
        DiscreteCoordinatorPhilosopherManager.PhilosopherHungryNotify += OnPhilosopherHungryEvent;
    }
    
    public void SetStrategy(DiscreteCoordinatorStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public IEnumerable<DiscreteCoordinatorPhilosopherManager> GetManagersIterator()
    {
        return _managers;
    }

    public void ReduceTime(DiscreteCoordinatorPhilosopherManager manager)
    {
        ((DiscretePhilosopherAction)manager.GetAction()).ReduceTime();
    }
    
    public void CheckPhilosopherState(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (!manager.GetAction().TimeIsRemain())
        {
            _strategy.CalculateNextPhilosopherState(manager);
        }
        manager.CheckPhilosopherHungry();
    }

    public bool TryGetFork(DiscreteCoordinatorPhilosopherManager manager, IFork fork)
    {
        if (manager.GetAction().TimeIsRemain()) return false;
        
        var whoTryGet = manager.Philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot == whoTryGet) return true;
        if (whoAlreadyGot != null && AbstractPhilosopherManager.PhilosopherIsOwnerBothFork(whoAlreadyGot)) return false;
        if (AbstractPhilosopherManager.PhilosopherIsOwnerBothFork(whoTryGet))
        {
            NotifyReleaseForkImmediately(
                _managers.First(man => man.Philosopher == whoAlreadyGot),
                fork);
        }
        
        NotifyGetFork(manager, fork);
        
        return true;
    }

    public void NotifyStartHungry(DiscreteCoordinatorPhilosopherManager manager)
    {
        StartHungryNotify?.Invoke(manager);
    }
    
    public bool NotifyGetFork(DiscreteCoordinatorPhilosopherManager manager, IFork fork)
    {
        fork.SetOwner(manager.Philosopher);
        GetForkNotify?.Invoke(manager, fork);
        return true;
    }

    public void NotifyStartEating(DiscreteCoordinatorPhilosopherManager manager)
    {
        StartEatingNotify?.Invoke(manager);
    }

    public bool NotifyReleaseForkImmediately(DiscreteCoordinatorPhilosopherManager manager, IFork fork)
    {
        fork.DropOwner();
        ReleaseForkImmediatelyNotify?.Invoke(manager, fork);
        return false;
    }

    public void NotifyStartThinking(DiscreteCoordinatorPhilosopherManager manager)
    {
        StartThinkingNotify?.Invoke(manager);
    }

    public void CheckDeadlock(int step)
    {
        var tooMuchWaitingCounter = 0;
        foreach (var manager in _managers)
        {
            if (manager.GetAction().TimeRemain < 0)
            {
                tooMuchWaitingCounter++;
            }
        }

        if (tooMuchWaitingCounter >= _managers.Length)
        {
            throw new Exception($"deadlock on {step} step");
        }
    }

    public void CollectMetrics(int step)
    {
        _metricsCollector.Collect(step);
    }

    public FinalStat GetFinalMetrics()
    {
        return _metricsCollector.GetFinalStat();
    }
    
    public void CreateLog(int step)
    {
        var log = PhilosopherLogger.CreateLog(step, _managers);
        Logger.LogInformation(log);
        Thread.Sleep(500);
    }
    
    private void OnPhilosopherHungryEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        _strategy.AddHungryPhilosopher(manager);
    }
}
