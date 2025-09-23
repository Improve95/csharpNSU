using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager;

public abstract class AbstractPhilosopherManager(Philosopher philosopher): IPhilosopherManager
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
    
    public IFork GetLeftFork()
    {
        return Philosopher.LeftFork;
    }
    
    public IFork GetRightFork()
    {
        return Philosopher.RightFork;
    }
    
    public void SetRightFork(IFork fork)
    {
        Philosopher.RightFork = fork;
    }
    
    public IPhilosopherAction GetAction()
    {
        return Philosopher.PhilosopherAction;
    }

    public static IPhilosopherAction GetAction(IPhilosopherManager manager)
    {
        return manager.GetAction();
    }

    public PhilosopherActionType GetActionType()
    {
        return GetAction().ActionType;
    }

    public static PhilosopherActionType GetActionType(IPhilosopherManager manager)
    {
        return manager.GetAction().ActionType;
    }
    
    public bool PhilosopherIsOwnerBothFork()
    {
        return Philosopher.LeftFork.IsOwner(Philosopher) &&
               Philosopher.RightFork.IsOwner(Philosopher);
    }

    public static bool PhilosopherIsOwnerBothFork(Philosopher philosopher)
    {
        return philosopher.LeftFork.IsOwner(philosopher) &&
               philosopher.RightFork.IsOwner(philosopher);
    }

    public abstract void SetAction(PhilosopherActionType philosopherAction);
}