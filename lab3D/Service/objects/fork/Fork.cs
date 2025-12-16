using System.Data;
using IService.objects;
using Utils.fork;

namespace Service.objects.fork;

public class Fork(int id) : IFork
{
    public IPhilosopher? Owner { get; private set; }

    public int Id { get; } = id;

    public ForkStatus Status { get; private set; }
    
    public void SetOwner(IPhilosopher owner)
    {
        Owner = owner ?? throw new NoNullAllowedException();
        Status = ForkStatus.InUse;
    }

    public void DropOwner()
    {
        Owner = null;
        Status = ForkStatus.Available;
    }

    public bool IsOwner(IPhilosopher philosopher)
    {
        return Owner == philosopher;
    }
    
    public SemaphoreSlim Lock { get; } = new(1, 1);
}