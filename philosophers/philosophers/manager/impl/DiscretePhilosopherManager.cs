using philosophers.manager.action;
using philosophers.manager.action.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace philosophers.manager.impl;

public class DiscretePhilosopherManager(Philosopher philosopher) : IPhilosopherManager
{
    
    public Philosopher Philosopher { get; } = philosopher;

    public Fork GetLeftFork()
    {
        return Philosopher.LeftFork;
    }
    
    public Fork GetRightFork()
    {
        return Philosopher.RightFork;
    }
    
    public void SetRightFork(Fork fork)
    {
        Philosopher.RightFork = fork;
    }
    
    public DiscretePhilosopherAction GetAction()
    {
        return (DiscretePhilosopherAction) Philosopher.PhilosopherAction;
    }

    public void SetAction(PhilosopherActionType philosopherAction)
    {
        Philosopher.PhilosopherAction = new DiscretePhilosopherAction(philosopherAction);
    }
}