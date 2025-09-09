namespace philosophers.objects.philosophers;

public class Philosopher
{
    public int Id { get; }

    public string Name { get; }

    public PhilosopherStatus Status { get; set; } = PhilosopherStatus.Thinking;

    public int TotalEating { get; private set; }

    private static int _nextId;
    
    public Philosopher(string name)
    {
        Id = _nextId;
        Name = name;
        _nextId++;
    }

    public void AddEating()
    {
        TotalEating++;
    }
}
