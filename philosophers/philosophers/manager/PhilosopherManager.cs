using philosophers.manager.action;
using philosophers.manager.action.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace philosophers.manager;

public interface IPhilosopherManager
{
    Fork GetLeftFork();
    Philosopher Philosopher { get; }
    Fork GetRightFork();
    DiscretePhilosopherAction GetAction();
    void SetAction(PhilosopherActionType philosopherAction);
}