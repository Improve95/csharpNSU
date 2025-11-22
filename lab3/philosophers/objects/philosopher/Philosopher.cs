using Microsoft.Extensions.Hosting;
using philosophers.action;
using philosophers.objects.fork;
using philosophers.service;

namespace philosophers.objects.philosopher;

public class Philosopher : BackgroundService
{
    
    public int Id { get; }

    public string Name { get; }

    public PhilosopherAction Action { get; set; }
    
    public Fork LeftFork { get; }

    public Fork RightFork { get; set; }

    public int TotalEating { get; private set; }
    
    public int HungryStartTime { get; set; }
    
    private static int _nextId;

    private readonly Strategy _strategy;
    
    private readonly AutoResetEvent _event = new(false);
    
    private bool ContinueWork { get; set; } = true;
    
    public Philosopher(string name, Fork leftFork, Fork rightFork, Strategy strategy)
    {
        Id = _nextId;
        Name = name;
        LeftFork = leftFork;
        RightFork = rightFork;
        Action = new PhilosopherAction(PhilosopherActionType.Thinking);
        _strategy = strategy;
        _nextId++;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Thread.Sleep(Action.TimeRemain * 5);
            
            PhilosopherActionType? newAction = null;
            bool canStartNewAction;
            while (newAction == null)
            {
                // Console.WriteLine($"{Philosopher.Name} try get new act");
                var res = _strategy.GetNewAction(this);
                newAction = res.newAction;
                canStartNewAction = res.canStartNewAction;
                // Console.WriteLine($"{Philosopher.Name} got new act: {newAction}, status: {canStartNewAction}");
                
                if (newAction == PhilosopherActionType.Hungry) SetStartHungryTime(DateTimeOffset.Now.Millisecond);
                if (canStartNewAction) break;

                _strategy.AddWaitingForkRelease(LeftFork, this);
                _strategy.AddWaitingForkRelease(RightFork, this);
                
                _event.WaitOne();
            }
            SetAction(newAction.Value);
        }
    }

    private void IncreaseEating()
    {
        TotalEating++;
    }
    
    public int GetStartHungryTime()
    {
        return HungryStartTime;
    }
    
    private void SetStartHungryTime(int startHungryTime)
    {
        HungryStartTime = startHungryTime;
    }
    
    public void WakeUp()
    {
        // Console.WriteLine($"wake up {Philosopher.Name}");
        _event.Set();
    }
    
    public void Stop()
    {
        ContinueWork = false;
    }
    
    public void SetAction(PhilosopherActionType philosopherAction)
    {
        Action = new PhilosopherAction(philosopherAction);
        if (philosopherAction == PhilosopherActionType.Eating)
        {
            IncreaseEating();
        }
    }
    
    public PhilosopherActionType GetActionType()
    {
        return Action.ActionType;
    }
    
    public bool PhilosopherIsOwnerBothFork()
    {
        return LeftFork.IsOwner(this) &&
               RightFork.IsOwner(this);
    }

    public static bool PhilosopherIsOwnerBothFork(Philosopher philosopher)
    {
        return philosopher.LeftFork.IsOwner(philosopher) &&
               philosopher.RightFork.IsOwner(philosopher);
    }
    
    public static bool PhilosopherIsOwnerAtLeastOneFork(Philosopher philosopher)
    {
        return philosopher.LeftFork.IsOwner(philosopher) ||
               philosopher.RightFork.IsOwner(philosopher);
    }
}