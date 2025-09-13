namespace philosophers.objects.fork;

public class Fork
{

    public int Id { get; }

    public ForkStatus Status { get; set; } = ForkStatus.Available;
    
    private static int _nextId;
    
    public Fork()
    {
        Id = _nextId;
        _nextId++;
    }
}