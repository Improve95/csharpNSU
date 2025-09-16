using ManagementImpl.logger;
using ManagementImpl.manager;
using ManagementImpl.manager.impl.coordinator;
using Microsoft.Extensions.Logging;
using philosophers.objects.fork;
using strategy.service;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinator: IDiscreteCoordinator
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<DiscreteCoordinator>();
    
    private readonly DiscreteCoordinatorPhilosopherManager[] _managers;

    private readonly Fork[] _forks;

    private DiscreteCoordinatorStrategy _strategy;
    
    public delegate void GetForkEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork);
    public static event GetForkEvent? GetForkNotify;
    
    public delegate void StartEatingEvent(DiscreteCoordinatorPhilosopherManager manager);
    public static event StartEatingEvent? StartEatingNotify;
    
    public delegate void ReleaseForkImmediatelyEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork);
    public static event ReleaseForkImmediatelyEvent? ReleaseForkImmediatelyNotify;
    
    public delegate void StartThinkingEvent(DiscreteCoordinatorPhilosopherManager manager);
    public static event StartThinkingEvent? StartThinkingNotify;
    
    public DiscreteCoordinator(DiscreteCoordinatorPhilosopherManager[] managers, Fork[] forks) 
    {
        _managers = managers;
        _forks = forks;
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
        manager.GetAction().ReduceTime();
    }
    
    public void CheckPhilosopherState(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (!manager.GetAction().TimeIsRemain())
        {
            _strategy.CalculateNextPhilosopherState(manager);
        }
        manager.CheckPhilosopherHungry();
    }

    public bool TryGetFork(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        var whoTryGet = manager.Philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot == whoTryGet) return true;
        if (whoAlreadyGot != null && AbstractDiscretePhilosopherManager.PhilosopherIsOwnerBothFork(whoAlreadyGot)) return false;

        if (AbstractDiscretePhilosopherManager.PhilosopherIsOwnerBothFork(whoTryGet))
        {
            NotifyReleaseForkImmediately(_managers.First(manager => manager.Philosopher == whoAlreadyGot), fork);
        }
        
        NotifyGetFork(manager, fork);
        
        return true;
    }

    public bool NotifyGetFork(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        fork.Owner = manager.Philosopher;
        GetForkNotify?.Invoke(manager, fork);
        return true;
    }

    public void NotifyStartEating(DiscreteCoordinatorPhilosopherManager manager)
    {
        StartEatingNotify?.Invoke(manager);
    }

    public bool NotifyReleaseForkImmediately(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        ReleaseForkImmediatelyNotify?.Invoke(manager, fork);
        return false;
    }

    public void NotifyStartThinking(DiscreteCoordinatorPhilosopherManager manager)
    {
        StartThinkingNotify?.Invoke(manager);
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
