using philosophers.objects.philosophers;
using strategy.service;

namespace ManagementImpl.service.impl;

public class DiscreteCoordinatorStrategy(DiscreteCoordinator coordinator) : IDiscreteStrategy
{
    public void DoStep(int step)
    {
        foreach (var manager in coordinator.GetManagersIterator())
        {
            var philosopherAction = manager.GetAction();
            if (!philosopherAction.TimeIsRemain())
            {
                
            }
            
            philosopherAction.ReduceTime();
        }
    }
    
    public void ResolveHungryPhilosopher(Philosopher philosopher)
    {
        
    }
}