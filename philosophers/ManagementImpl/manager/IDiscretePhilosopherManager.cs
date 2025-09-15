using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;

namespace ManagementImpl.manager;

public interface IDiscretePhilosopherManager
{
    int GetPhilosopherId();
    string GetPhilosopherName();
    int GetTotalEating();
    Fork GetLeftFork();
    Fork GetRightFork();
    DiscretePhilosopherAction GetAction();
    void SetAction(PhilosopherActionType philosopherAction);
    bool PhilosopherIsOwnerBothFork();
}