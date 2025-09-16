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
        
        coordinator.CollectMetrics(step);
        coordinator.CreateLog(step);
        coordinator.CheckDeadlock(step);
    }
    
    public void AddHungryPhilosopher(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (!_hungryPhilosophers.Add(manager)) return;
        coordinator.NotifyStartHungry(manager);
    }

    public void CalculateNextPhilosopherState(DiscreteCoordinatorPhilosopherManager manager)
    {
        if ((manager.GetActionType() == GetLeftFork ||
             manager.GetActionType() == GetRightFork) && 
            manager.PhilosopherIsOwnerBothFork())
        {
            coordinator.NotifyStartEating(manager);
            _hungryPhilosophers.Remove(manager);
            return;
        }
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
        
        var leftFork = manager.GetLeftFork();
        var rightFork = manager.GetRightFork();
        var isGetLeftFork = coordinator.TryGetFork(manager, leftFork);
        var isGetRightFork = coordinator.TryGetFork(manager, rightFork);
    }
}
