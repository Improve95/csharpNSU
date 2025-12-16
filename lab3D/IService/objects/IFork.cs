using Utils.fork;

namespace IService.objects;

public interface IFork
{
    void SetOwner(IPhilosopher owner);
    void DropOwner();
    bool IsOwner(IPhilosopher philosopher);
    IPhilosopher? Owner { get; }
    Mutex Mutex { get; }
    int Id { get; }
    ForkStatus Status { get; }
}