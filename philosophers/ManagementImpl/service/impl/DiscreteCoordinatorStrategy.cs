using ManagementImpl.manager.impl.coordinator;
using strategy.service;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinatorStrategy(DiscreteCoordinator coordinator) : IDiscreteStrategy
{
    private readonly ISet<DiscreteCoordinatorPhilosopherManager> _hungryPhilosophers =
        new HashSet<DiscreteCoordinatorPhilosopherManager>();
    
    public void DoStep(int step)
    {
        foreach (var manager in coordinator.GetManagersIterator())
        {
            coordinator.CheckPhilosopherState(manager);
            ResolveHungryPhilosopher(manager);
            coordinator.ReduceTime(manager);
        }
        
        coordinator.CreateLog(step);
    }
    
    public void AddHungryPhilosopher(DiscreteCoordinatorPhilosopherManager manager)
    {
        _hungryPhilosophers.Add(manager);
    }

    public void CalculateNextPhilosopherState(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (manager.GetActionType() == Eating)
        {
            coordinator.NotifyReleaseForkImmediately(manager, manager.GetLeftFork());
            coordinator.NotifyReleaseForkImmediately(manager, manager.GetRightFork());
            coordinator.NotifyStartThinking(manager);
        }
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
            coordinator.NotifyStartEating(manager);
        }
    }
}
