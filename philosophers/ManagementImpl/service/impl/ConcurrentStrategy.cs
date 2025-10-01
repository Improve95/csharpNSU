using ManagementImpl.manager;
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
            case Hungry or GetLeftFork or GetRightFork:
                TryGetFork2(manager);
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

    private void CheckReleaseForkImmediately()
    {
        
    }

    private bool TryGetFork2(ConcurrentPhilosopherManager manager)
    {
        var whoTryGet = manager.Philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot == whoTryGet) return false;
        if (whoAlreadyGot != null && AbstractPhilosopherManager.PhilosopherIsOwnerBothFork(whoAlreadyGot)) 
            return false;
        
        return true;
    }
    
    private bool TryGetFork(IConcurrentFork fork, ConcurrentPhilosopherManager manager)
    {
        var whoTryGet = manager.Philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot == whoTryGet) return false;
        if (whoAlreadyGot != null && AbstractPhilosopherManager.PhilosopherIsOwnerBothFork(whoAlreadyGot)) 
            return false;
        
        if (AbstractPhilosopherManager.PhilosopherIsOwnerAtLeastOneFork(whoTryGet))
        {
            fork.DropOwner();
            fork.Mutex.ReleaseMutex();
        }

        fork.Mutex.WaitOne();
        fork.SetOwner(manager.Philosopher);
        
        return true;
    }
    
    private void TakeFork(IConcurrentFork fork, ConcurrentPhilosopherManager manager)
    {
        // Console.WriteLine($"try take {manager.GetPhilosopherName()} fork {fork.Id}");
        fork.Mutex.WaitOne();
        // Console.WriteLine($"taken {manager.GetPhilosopherName()} fork {fork.Id}");
        fork.SetOwner(manager.Philosopher);
    }
}
