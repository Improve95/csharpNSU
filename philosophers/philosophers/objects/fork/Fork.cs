using philosophers.objects.philosophers;

namespace philosophers.objects.fork;

public class Fork
{
    public int Id { get; }

    public Philosopher? Owner { get; set; }

    private static int _nextId;
    
    public Fork()
    {
        Id = _nextId;
        _nextId++;
    }
}