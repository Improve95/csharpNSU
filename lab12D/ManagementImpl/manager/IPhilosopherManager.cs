using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager;

public interface IPhilosopherManager
{
    Philosopher Philosopher { get; }
    int GetPhilosopherId();
    string GetPhilosopherName();
    int GetTotalEating();
    IFork GetLeftFork();
    IFork GetRightFork();
    IPhilosopherAction GetAction();
    void SetAction(PhilosopherActionType philosopherAction);
    bool PhilosopherIsOwnerBothFork();
}