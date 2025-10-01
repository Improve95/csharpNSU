using System.Collections.Concurrent;
using ManagementImpl.manager;
using ManagementImpl.manager.impl;
using philosophers.action;
using philosophers.objects.fork;
using strategy.service;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.service.impl;

public class ConcurrentStrategy : IConcurrentStrategy
{
    private readonly IDictionary<int, ConcurrentPhilosopherManager> _waitingFork = 
        new ConcurrentDictionary<int, ConcurrentPhilosopherManager>();
    
    public (PhilosopherActionType? newAction, bool canStartNewAction) GetNewAction(ConcurrentPhilosopherManager manager)
    {
        var actionType = manager.GetActionType();
        if (actionType == Thinking)
        {
            return (Hungry, true);
        }

        if (manager.PhilosopherIsOwnerBothFork() && actionType is Hungry or GetLeftFork or GetRightFork)
        {
            return (Eating, true);
        }
        
        if (actionType is Hungry or GetRightFork)
        {
            if (TryGetFork2((ConcurrentFork)manager.GetLeftFork(), manager))
            {
                return (GetLeftFork, true);
            }

            // if (TryGetFork2((ConcurrentFork)manager.GetRightFork(), manager))
            // {
            //     return (GetRightFork, true);
            // }

            return (GetLeftFork, false);
        }
        
        if (actionType is Hungry or GetLeftFork)
        {
            if (TryGetFork2((ConcurrentFork)manager.GetRightFork(), manager))
            {
                return (GetRightFork, true);
            }
            
            // if (TryGetFork2((ConcurrentFork)manager.GetLeftFork(), manager))
            // {
            //     return (GetLeftFork, true);
            // }
            
            return (GetRightFork, false);
        }

        if (actionType == Eating)
        {
            var leftFork = (IConcurrentFork)manager.GetLeftFork();
            leftFork.Mutex.WaitOne();
            leftFork.DropOwner();
            leftFork.Mutex.ReleaseMutex();
            WakeUpSleepingPhilosophers(leftFork);
            
            var rightFork = (IConcurrentFork)manager.GetRightFork();
            rightFork.Mutex.WaitOne();
            rightFork.DropOwner();
            rightFork.Mutex.ReleaseMutex();
            WakeUpSleepingPhilosophers(rightFork);
            
            return (Thinking, true);
        }
        
        return (null, false);
    }

    public void AddWaitingForkRelease(IConcurrentFork fork, ConcurrentPhilosopherManager manager)
    {
        _waitingFork[fork.Id] = manager;
    }

    private void WakeUpSleepingPhilosophers(IConcurrentFork fork)
    {
        if (_waitingFork.TryGetValue(fork.Id, out var manager))
        {
            /*if (manager.GetTotalEating() > 15)
            {
                foreach (var el in _waitingFork)
                {
                    Console.WriteLine($"{el.Key}: {el.Value.GetPhilosopherName()}");
                }
            }*/
            
            manager.WakeUp();
            _waitingFork.Remove(fork.Id);
        }
    }
    
    private bool TryGetFork2(ConcurrentFork fork, ConcurrentPhilosopherManager manager)
    {
        var forkMutex = fork.Mutex;
        forkMutex.WaitOne();
        
        var whoTryGet = manager.Philosopher;
        var whoAlreadyGot = fork.Owner;
        // if (whoAlreadyGot == whoTryGet) return true;
        if (whoAlreadyGot != null && AbstractPhilosopherManager.PhilosopherIsOwnerBothFork(whoAlreadyGot))
        {
            forkMutex.ReleaseMutex();
            return false;
        }

        if (whoAlreadyGot != null && whoAlreadyGot.HungryStartTime < whoTryGet.HungryStartTime)
        {
            forkMutex.ReleaseMutex();
            return false;
        }
        
        /*if (AbstractPhilosopherManager.PhilosopherIsOwnerAtLeastOneFork(whoTryGet))
        {
            fork.DropOwner();
        }*/
        /*else
        {
            // Console.WriteLine("here2");
            forkMutex.ReleaseMutex();
            return false;
        }*/

        fork.SetOwner(whoTryGet);
        
        forkMutex.ReleaseMutex();
        return true;
    }
    
    /*private bool TryGetFork(IConcurrentFork fork, ConcurrentPhilosopherManager manager)
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
    }*/
}
