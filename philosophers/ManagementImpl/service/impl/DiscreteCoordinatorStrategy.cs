using ManagementImpl.manager.impl.coordinator;
using strategy.service;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinatorStrategy(DiscreteCoordinator coordinator) : IDiscreteStrategy
{
    private readonly ISet<DiscreteCoordinatorPhilosopherManager> _hungryPhilosophers =
        new HashSet<DiscreteCoordinatorPhilosopherManager>();
    
    public void DoStep(int step)
    {
        foreach (var manager in coordinator.GetManagersIterator())
        {
            coordinator.CheckPhilosopherHungry(manager);
            ResolveHungryPhilosopher(manager);
            coordinator.ReduceTime(manager);
        }
    }
    
    public void AddHungryPhilosopher(DiscreteCoordinatorPhilosopherManager manager)
    {
        _hungryPhilosophers.Add(manager);
    }

    private void ResolveHungryPhilosopher(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (!_hungryPhilosophers.Contains(manager)) return;
        
        var leftFok = manager.GetLeftFork();
        var isGetLeftFork = coordinator.TryGetFork(manager, leftFok);
        var rightFork = manager.GetRightFork(); 
        var isGetRightFork = coordinator.TryGetFork(manager, rightFork);
        if (isGetLeftFork && isGetRightFork)
        {
            // todo Notify начало обеда
        }
    }
}
