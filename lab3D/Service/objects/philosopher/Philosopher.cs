using IService.objects;
using IService.service;
using Microsoft.Extensions.Hosting;
using Utils.action;

namespace Service.objects.philosopher;

public class Philosopher : BackgroundService, IPhilosopher
{
    public int Id { get; }

    public string Name { get; }

    public PhilosopherAction Action { get; set; }
    
    public IFork LeftFork { get; set;  }

    public IFork RightFork { get; set; }

    public int TotalEating { get; private set; }
    
    public int HungryStartTime { get; set; }
    
    private static int _nextId;

    private readonly IStrategy _strategy;
    
    private readonly SemaphoreSlim _signal = new(0);

    public bool ContinueWork { get; set; } = true;

    public Philosopher(string name, ITableManager tableManager, IStrategy strategy)
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
                var res = _strategy.GetNewAction(this, stoppingToken);
                newAction = res.newAction;
                var canStartNewAction = res.canStartNewAction;
                if (newAction == PhilosopherActionType.Hungry) SetStartHungryTime(DateTimeOffset.Now.Millisecond);
                if (canStartNewAction) break;

                _strategy.AddWaitingForkRelease(LeftFork, this);
                _strategy.AddWaitingForkRelease(RightFork, this);

                await _signal.WaitAsync(stoppingToken);
            }
            if (newAction != null)
            {
                SetAction(newAction.Value);
            }
        }
    }

    public void IncreaseEating()
    {
        TotalEating++;
    }
    
    public int GetStartHungryTime()
    {
        return HungryStartTime;
    }

    public void SetStartHungryTime(int startHungryTime)
    {
        HungryStartTime = startHungryTime;
    }
    
    public void WakeUp()
    {
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

    public static bool PhilosopherIsOwnerBothFork(IPhilosopher philosopher)
    {
        return philosopher.LeftFork.IsOwner(philosopher) &&
               philosopher.RightFork.IsOwner(philosopher);
    }
    
    public static bool PhilosopherIsOwnerAtLeastOneFork(IPhilosopher philosopher)
    {
        return philosopher.LeftFork.IsOwner(philosopher) ||
               philosopher.RightFork.IsOwner(philosopher);
    }
}