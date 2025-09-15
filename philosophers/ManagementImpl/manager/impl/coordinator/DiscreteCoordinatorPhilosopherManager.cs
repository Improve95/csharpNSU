using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager.impl.coordinator;

public class DiscreteCoordinatorPhilosopherManager: AbstractDiscretePhilosopherManager
{
    public delegate void PhilosopherHungryEvent(Philosopher philosopher);
    
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
        if (GetActionType() == PhilosopherActionType.Hungry)
        { 
            PhilosopherHungryNotify?.Invoke(Philosopher);
        }
    }

    private void OnGetForkEvent(ForkType forkType)
    {
        
    }

    private void OnStartHungryEvent()
    {
        
    }
    
    private void OnStartEatingEvent()
    {
        
    }

    private void OnReleaseForkImmediatelyEvent(ForkType forkType)
    {
        
    }

    private void OnStartThinkingEvent()
    {
        
    }
}
