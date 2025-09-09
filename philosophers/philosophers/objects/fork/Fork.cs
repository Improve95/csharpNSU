namespace philosophers.objects.fork;

public class Fork
{
    private static int _nextId = 0;

    public int Id { get; }

    public ForkStatus Status { get; set; } = ForkStatus.Available;
    
    public Fork()
    {
        Id = _nextId;
        _nextId++;
    }
}