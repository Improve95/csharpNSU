using ManagementImpl.service.impl;
using Microsoft.Extensions.Logging;
using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosophers;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.manager.impl.coordinator;

public class DiscreteCoordinatorPhilosopherManager: 
    AbstractDiscretePhilosopherManager, 
    IDiscreteCoordinatorPhilosopherManager
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<DiscreteCoordinatorPhilosopherManager>();
    
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
        if (GetActionType() == Thinking && !GetAction().TimeIsRemain())
        { 
            SetAction(Hungry);
            PhilosopherHungryNotify?.Invoke(this);
        }
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
