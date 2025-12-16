using IService.objects;
using IService.service;
using Microsoft.Extensions.Hosting;
using Service.action;
using Service.objects.fork;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Philosopher : BackgroundService, IPhilosopher
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
    
    private readonly SemaphoreSlim _signal = new(0);
    // private readonly AutoResetEvent _event = new(false);
    
    private bool ContinueWork { get; set; } = true;

    protected Philosopher(string name, ITableManager tableManager, Strategy strategy)
    {
        Id = _nextId;
        Name = name;
        LeftFork = tableManager.GetLeftFork(Id);
        RightFork = tableManager.GetRightFork(Id);
        tableManager.RegisterPhilosopher(this);
        Action = new PhilosopherAction(PhilosopherActionType.Thinking);
        _strategy = strategy;
        _nextId++;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(Action.TimeRemain * 5), stoppingToken);
            
            PhilosopherActionType? newAction = null;
            while (newAction == null && !stoppingToken.IsCancellationRequested)
            {
                // Console.WriteLine($"{Philosopher.Name} try get new act");
                var res = _strategy.GetNewAction(this);
                newAction = res.newAction;
                var canStartNewAction = res.canStartNewAction;
                // Console.WriteLine($"{Philosopher.Name} got new act: {newAction}, status: {canStartNewAction}");
                
                if (newAction == PhilosopherActionType.Hungry) SetStartHungryTime(DateTimeOffset.Now.Millisecond);
                if (canStartNewAction) break;

                _strategy.AddWaitingForkRelease(LeftFork, this);
                _strategy.AddWaitingForkRelease(RightFork, this);

                await _signal.WaitAsync(stoppingToken);
                // _event.WaitOne();
            }
            if (newAction != null)
            {
                SetAction(newAction.Value);
            }
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
        _signal.Release();
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