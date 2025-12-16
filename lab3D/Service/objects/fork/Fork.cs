using System.Data;
using IService.objects;
using Service.objects.philosopher;

namespace Service.objects.fork;

public class Fork(int id) : IFork
{
    // private static int _nextId;
    
    public Philosopher? Owner { get; private set; }

    public int Id { get; } = id;

    // Id = _nextId;
    // _nextId++;

    public ForkStatus Status { get; private set; }
    
    public void SetOwner(Philosopher owner)
    {
        Owner = owner ?? throw new NoNullAllowedException();
        Status = ForkStatus.InUse;
    }

    public void DropOwner()
    {
        Owner = null;
        Status = ForkStatus.Available;
    }

    public bool IsOwner(Philosopher philosopher)
    {
        return Owner == philosopher;
    }
    
    public Mutex Mutex { get; } = new();
}