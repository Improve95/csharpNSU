using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;

namespace philosophers.objects.philosophers;

public class Philosopher
{
    public int Id { get; }

    public string Name { get; }
    
    public IPhilosopherAction PhilosopherAction { get; set; }

    public Fork LeftFork { get; }

    public Fork RightFork { get; set; }
    
    public int TotalEating { get; private set; }
    
    private static int _nextId;
    
    public Philosopher(string name, Fork leftFork, Fork rightFork)
    {
        Id = _nextId;
        Name = name;
        LeftFork = leftFork;
        RightFork = rightFork;
        PhilosopherAction = new DiscretePhilosopherAction(PhilosopherActionType.Thinking);
        _nextId++;
    }

    public void IncreaseEating()
    {
        TotalEating++;
    }
}
