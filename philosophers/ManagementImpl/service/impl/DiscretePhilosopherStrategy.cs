using philosophers.manager.action;
using philosophers.manager.impl;
using strategy.service;

namespace ManagementImpl.service.impl;

public class PhilosopherDiscreteStrategy(DiscretePhilosopherManager[] philosopherManagers) : IDiscreteStrategy
{
    
    public void DoStep()
    {
        foreach (var philosopherManager in philosopherManagers)
        {
            switch (philosopherManager.GetAction())
            {
                case PhilosopherActionType.Hungry:
                {
                    var leftFork = philosopherManager.Philosopher.LeftFork;
                    var rightFork = philosopherManager.Philosopher.RightFork;
                    break;
                }
            }
            philosopherManager.DoStep();
        }
    }
}
