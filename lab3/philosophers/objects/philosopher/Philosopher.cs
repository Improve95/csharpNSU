using philosophers.action;
using philosophers.objects.fork;

namespace philosophers.objects.philosopher;

public class Philosopher
{
    private static int _nextId;
    
    public int Id { get; }

    public string Name { get; }

    public PhilosopherAction PhilosopherAction { get; set; }

    public Fork LeftFork { get; }

    public Fork RightFork { get; set; }

    public int TotalEating { get; private set; }
    
    private bool ContinueWork { get; set; } = true;
    
    private readonly AutoResetEvent _event = new(false);
    
    public int HungryStartTime { get; set; }
    
    public Philosopher(string name, Fork leftFork, Fork rightFork)
    {
        Id = _nextId;
        Name = name;
        LeftFork = leftFork;
        RightFork = rightFork;
        PhilosopherAction = new PhilosopherAction(PhilosopherActionType.Thinking);
        _nextId++;
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
        PhilosopherAction = new PhilosopherAction(philosopherAction);
        if (philosopherAction == PhilosopherActionType.Eating)
        {
            IncreaseEating();
        }
    }
    
    public PhilosopherActionType GetActionType()
    {
        return PhilosopherAction.ActionType;
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