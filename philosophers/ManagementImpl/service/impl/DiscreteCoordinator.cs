using ManagementImpl.manager.impl.coordinator;
using philosophers.objects.fork;
using philosophers.objects.philosophers;
using strategy.service;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinator: IDiscreteCoordinator
{
    private readonly DiscreteCoordinatorPhilosopherManager[] _managers;

    private readonly Fork[] _forks;

    private DiscreteCoordinatorStrategy _strategy;

    public delegate void StartHungryEvent();
    public static event StartHungryEvent? StartHungryNotify;
    
    public delegate void GetForkEvent(ForkType forkType);
    public static event GetForkEvent? GetForkNotify;
    
    public delegate void StartEatingEvent();
    public static event StartEatingEvent? StartEatingNotify;

    public delegate void ReleaseForkImmediatelyEvent(ForkType forkType);
    public static event ReleaseForkImmediatelyEvent? ReleaseForkImmediatelyNotify;

    public delegate void StartThinkingEvent();
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

    public void NotifyStartHungry()
    {
        StartHungryNotify?.Invoke();
    }

    public bool NotifyGetFork(ForkType forkType)
    {
        GetForkNotify?.Invoke(forkType);
        return false;
    }

    public void NotifyStartEating()
    {
        StartEatingNotify?.Invoke();
    }

    public bool NotifyReleaseForkImmediately(ForkType forkType)
    {
        ReleaseForkImmediatelyNotify?.Invoke(forkType);
        return false;
    }

    public void NotifyStartThinking()
    {
        StartThinkingNotify?.Invoke();
    }
    
    private void OnPhilosopherHungryEvent(Philosopher philosopher) 
    {
        _strategy.ResolveHungryPhilosopher(philosopher);
    }
}