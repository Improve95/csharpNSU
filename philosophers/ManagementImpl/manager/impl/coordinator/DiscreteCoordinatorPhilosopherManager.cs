using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosophers;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.manager.impl.coordinator;

public class DiscreteCoordinatorPhilosopherManager: 
    AbstractDiscretePhilosopherManager, 
    IDiscreteCoordinatorPhilosopherManager
{
    public delegate void PhilosopherHungryEvent(DiscreteCoordinatorPhilosopherManager manager);
    
    public static event PhilosopherHungryEvent? PhilosopherHungryNotify;
    
    public DiscreteCoordinatorPhilosopherManager(Philosopher philosopher) : base(philosopher)
    {
        DiscreteCoordinator.GetForkNotify += OnGetForkEvent;
        DiscreteCoordinator.StartEatingNotify += OnStartEatingEvent;
        DiscreteCoordinator.ReleaseForkImmediatelyNotify += OnReleaseForkImmediatelyEvent;
        DiscreteCoordinator.StartThinkingNotify += OnStartThinkingEvent;
    }
    
    public void CheckPhilosopherHungry()
    {
        if (GetActionType() == Hungry)
        { 
            PhilosopherHungryNotify?.Invoke(this);
        }
    }

    private void OnGetForkEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        SetAction(Philosopher.LeftFork == fork
            ? PhilosopherActionType.GetLeftFork
            : PhilosopherActionType.GetRightFork);
    }
    
    private void OnStartEatingEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        SetAction(Eating);
    }

    private void OnReleaseForkImmediatelyEvent(DiscreteCoordinatorPhilosopherManager manager, Fork fork)
    {
        SetAction(Philosopher.LeftFork == fork 
            ? ReleaseLeftFork 
            : ReleaseRightFork);
    }

    private void OnStartThinkingEvent(DiscreteCoordinatorPhilosopherManager manager)
    {
        SetAction(Thinking);
    }
}
