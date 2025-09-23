using ManagementImpl.manager.impl;
using philosophers.action;
using philosophers.objects.fork;
using strategy.service;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.service.impl;

public class ConcurrentStrategy : IConcurrentStrategy
{
    public PhilosopherActionType? GetNewAction(ConcurrentPhilosopherManager manager)
    {
        var actionType = manager.GetActionType();
        switch (actionType)
        {
            case Thinking:
                return Hungry;
            case Hungry or GetRightFork:
                TakeFork((IConcurrentFork)manager.GetLeftFork(), manager);
                break;
            case GetLeftFork:
                TakeFork((IConcurrentFork)manager.GetRightFork(), manager);
                break;
            case Eating:
                var leftFork = (IConcurrentFork)manager.GetLeftFork();
                var rightFork = (IConcurrentFork)manager.GetRightFork();
            
                leftFork.DropOwner();
                leftFork.Mutex.ReleaseMutex();
            
                rightFork.DropOwner();
                rightFork.Mutex.ReleaseMutex();
                return Thinking;
        }
        
        if (manager.PhilosopherIsOwnerBothFork())
        {
            return Eating;
        }
        
        return null;
    }

    private void TakeFork(IConcurrentFork fork, ConcurrentPhilosopherManager manager)
    {
        fork.Mutex.WaitOne();
        fork.SetOwner(manager.Philosopher);
    }
}