using System.Data;
using philosophers.objects.philosophers;
using static philosophers.objects.fork.ForkStatus;

namespace philosophers.objects.fork;

public class Fork
{
    private static int _nextId;
    
    public Philosopher? Owner { get; private set; }

    public int Id { get; }
    
    public ForkStatus Status { get; private set; }
    
    public void SetOwner(Philosopher owner)
    {
        Owner = owner ?? throw new NoNullAllowedException();
        Status = InUse;
    }

    public void DropOwner()
    {
        Owner = null;
        Status = Available;
    }
    
    public Fork()
    {
        Id = _nextId;
        _nextId++;
    }

    public bool IsOwner(Philosopher philosopher)
    {
        return Owner == philosopher;
    }
}