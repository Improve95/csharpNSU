using philosophers.objects.philosophers;

namespace philosophers.objects.fork;

public interface IFork
{
    public int Id { get; }
    public Philosopher? Owner { get; }
    public ForkStatus Status { get; }
    public void SetOwner(Philosopher owner);
    public void DropOwner();
    public bool IsOwner(Philosopher philosopher);
}