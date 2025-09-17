using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosophers;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.manager.impl;

public class DiscreteCoordinatorPhilosopherManager: 
    AbstractDiscretePhilosopherManager, 
    IDiscreteCoordinatorPhilosopherManager
{
    public delegate void PhilosopherHungryEvent(DiscreteCoordinatorPhilosopherManager manager);
    
    public static event PhilosopherHungryEvent? PhilosopherHungryNotify;
    
    public DiscreteCoordinatorPhilosopherManager(Philosopher philosopher) : base(philosopher)
    {
        DiscreteCoordinator.StartHungryNotify += OnStartHungryEvent;
        DiscreteCoordinator.GetForkNotify += OnGetForkEvent;
        DiscreteCoordinator.StartEatingNotify += OnStartEatingEvent;
        DiscreteCoordinator.ReleaseForkImmediatelyNotify += OnReleaseForkImmediatelyEvent;
        DiscreteCoordinator.StartThinkingNotify += OnStartThinkingEvent;
    }
    
    public void CheckPhilosopherHungry()
    {
        if (GetActionType() == Thinking && !GetAction().TimeIsRemain())
        { 
            PhilosopherHungryNotify?.Invoke(this);
        }
    }
    
    private void OnStartHungryEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (manager != this) { return; }
        SetAction(Hungry);
    }
    
    private void OnGetForkEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        if (manager != this) { return; }
        SetAction(Philosopher.LeftFork == fork
            ? PhilosopherActionType.GetLeftFork
            : PhilosopherActionType.GetRightFork);
    }
    
    private void OnStartEatingEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (manager != this) { return; }
        SetAction(Eating);
    }

    private void OnReleaseForkImmediatelyEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        if (manager != this) { return; }
        SetAction(Philosopher.LeftFork == fork 
            ? ReleaseLeftFork 
            : ReleaseRightFork);
    }

    private void OnStartThinkingEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        if (manager != this) { return; }
        SetAction(Thinking);
    }
}
