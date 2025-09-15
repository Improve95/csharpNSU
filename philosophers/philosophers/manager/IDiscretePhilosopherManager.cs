using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;

namespace philosophers.manager;

public interface IDiscretePhilosopherManager
{
    Fork GetLeftFork();
    Fork GetRightFork();
    DiscretePhilosopherAction GetAction();
    void SetAction(PhilosopherActionType philosopherAction);
    int GetPhilosopherId();
    string GetPhilosopherName();
    int GetTotalEating();
}