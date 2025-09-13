using philosophers.manager.action;
using philosophers.manager.action.impl;
using philosophers.objects.philosophers;

namespace philosophers.manager.impl;

public class DiscretePhilosopherManager(Philosopher philosopher) : IPhilosopherManager
{
    
    public Philosopher Philosopher { get; } = philosopher;

    public void DoStep()
    {
        var philosopherAction = Philosopher.PhilosopherAction;
        ((DiscretePhilosopherAction) philosopherAction).ReduceTime();
        if (philosopherAction.TimeIsRemain())
        {
            // todo something
        }
    }
    
    public PhilosopherActionType GetAction()
    {
        return Philosopher.PhilosopherAction.ActionType;
    }

    public void SetAction(PhilosopherActionType philosopherAction)
    {
        Philosopher.PhilosopherAction = new DiscretePhilosopherAction(philosopherAction);
    }
}