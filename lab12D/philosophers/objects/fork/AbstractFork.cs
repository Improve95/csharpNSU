using System.Data;
using philosophers.objects.philosophers;
using static philosophers.objects.fork.ForkStatus;

namespace philosophers.objects.fork;

public abstract class AbstractFork
{
    private static int _nextId;
    
    public Philosopher? Owner { get; private set; }

    public int Id { get; }
    
    protected AbstractFork()
    {
        Id = _nextId;
        _nextId++;
    }
    
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

    public bool IsOwner(Philosopher philosopher)
    {
        return Owner == philosopher;
    }
}