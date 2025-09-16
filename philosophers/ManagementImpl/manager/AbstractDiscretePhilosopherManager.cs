using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager;

public abstract class AbstractDiscretePhilosopherManager(Philosopher philosopher): IDiscretePhilosopherManager
{
    public Philosopher Philosopher { get; } = philosopher;
    
    public int GetPhilosopherId()
    {
        return Philosopher.Id;
    }
    
    public string GetPhilosopherName()
    {
        return Philosopher.Name;
    }

    public int GetTotalEating()
    {
        return Philosopher.TotalEating;
    }
    
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

    public static DiscretePhilosopherAction GetAction(IDiscretePhilosopherManager manager)
    {
        return manager.GetAction();
    }
    
    public void SetAction(PhilosopherActionType philosopherAction)
    {
        Philosopher.PhilosopherAction = new DiscretePhilosopherAction(philosopherAction);
        if (philosopherAction == PhilosopherActionType.Eating)
        {
            Philosopher.IncreaseEating();
        }
    }

    public PhilosopherActionType GetActionType()
    {
        return GetAction().ActionType;
    }

    public static PhilosopherActionType GetActionType(IDiscretePhilosopherManager manager)
    {
        return manager.GetAction().ActionType;
    }
    
    public bool PhilosopherIsOwnerBothFork()
    {
        return Philosopher.LeftFork.Owner == Philosopher && 
               Philosopher.RightFork.Owner == Philosopher;
    }

    public static bool PhilosopherIsOwnerBothFork(Philosopher philosopher)
    {
        return philosopher.LeftFork.Owner == philosopher &&
               philosopher.RightFork.Owner == philosopher;
    }

    public static bool PhilosopherIsOwnerAtLeastOneFork(Philosopher philosopher)
    {
        return philosopher.LeftFork.Owner == philosopher ||
               philosopher.RightFork.Owner == philosopher;
    }
}