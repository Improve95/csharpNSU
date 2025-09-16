using ManagementImpl.manager;
using ManagementImpl.manager.impl.coordinator;
using philosophers.objects.fork;
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

    public void ReduceTime(DiscreteCoordinatorPhilosopherManager manager)
    {
        manager.GetAction().ReduceTime();
    }
    
    public void CheckPhilosopherHungry(DiscreteCoordinatorPhilosopherManager manager)
    {
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
            // todo Notify release fork
            // todo присвоить вилку другому
        }
        
        return true;
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
    
    private void OnPhilosopherHungryEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        _strategy.AddHungryPhilosopher(manager);
    }
}
