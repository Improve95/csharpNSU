using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager;

public interface IDiscretePhilosopherManager
{
    Philosopher Philosopher { get; }
    int GetPhilosopherId();
    string GetPhilosopherName();
    int GetTotalEating();
    Fork GetLeftFork();
    Fork GetRightFork();
    DiscretePhilosopherAction GetAction();
    void SetAction(PhilosopherActionType philosopherAction);
    bool PhilosopherIsOwnerBothFork();
}