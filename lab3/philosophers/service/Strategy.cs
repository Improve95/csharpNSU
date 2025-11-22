using System.Collections.Concurrent;
using philosophers.action;
using philosophers.objects.fork;
using philosophers.objects.philosopher;

namespace philosophers.service;

public class Strategy
{
    private readonly IDictionary<int, Philosopher> _waitingFork = 
        new ConcurrentDictionary<int, Philosopher>();
    
    public (PhilosopherActionType? newAction, bool canStartNewAction) GetNewAction(Philosopher philosopher)
    {
        var actionType = philosopher.GetActionType();
        if (actionType == PhilosopherActionType.Thinking)
        {
            return (PhilosopherActionType.Hungry, true);
        }

        if (philosopher.PhilosopherIsOwnerBothFork() && 
            actionType is 
                PhilosopherActionType.Hungry or 
                PhilosopherActionType.GetLeftFork or 
                PhilosopherActionType.GetRightFork)
        {
            return (PhilosopherActionType.Eating, true);
        }
        
        if (actionType is PhilosopherActionType.Hungry or PhilosopherActionType.GetRightFork)
        {
            if (TryGetFork2(philosopher.LeftFork, philosopher))
            {
                return (PhilosopherActionType.GetLeftFork, true);
            }

            // if (TryGetFork2((ConcurrentFork)manager.GetRightFork(), manager))
            // {
            //     return (GetRightFork, true);
            // }

            return (PhilosopherActionType.GetLeftFork, false);
        }
        
        if (actionType is PhilosopherActionType.Hungry or PhilosopherActionType.GetLeftFork)
        {
            if (TryGetFork2(philosopher.RightFork, philosopher))
            {
                return (PhilosopherActionType.GetRightFork, true);
            }
            
            // if (TryGetFork2((ConcurrentFork)manager.GetLeftFork(), manager))
            // {
            //     return (GetLeftFork, true);
            // }
            
            return (PhilosopherActionType.GetRightFork, false);
        }

        if (actionType == PhilosopherActionType.Eating)
        {
            var leftFork = philosopher.LeftFork;
            leftFork.Mutex.WaitOne();
            leftFork.DropOwner();
            leftFork.Mutex.ReleaseMutex();
            WakeUpSleepingPhilosophers(leftFork);
            
            var rightFork = philosopher.RightFork;
            rightFork.Mutex.WaitOne();
            rightFork.DropOwner();
            rightFork.Mutex.ReleaseMutex();
            WakeUpSleepingPhilosophers(rightFork);
            
            return (PhilosopherActionType.Thinking, true);
        }
        
        return (null, false);
    }

    public void AddWaitingForkRelease(Fork fork, Philosopher philosopher)
    {
        _waitingFork[fork.Id] = philosopher;
    }

    private void WakeUpSleepingPhilosophers(Fork fork)
    {
        if (_waitingFork.TryGetValue(fork.Id, out var philosopher))
        {
            /*if (manager.GetTotalEating() > 15)
            {
                foreach (var el in _waitingFork)
                {
                    Console.WriteLine($"{el.Key}: {el.Value.GetPhilosopherName()}");
                }
            }*/
            
            philosopher.WakeUp();
            _waitingFork.Remove(fork.Id);
        }
    }
    
    private bool TryGetFork2(Fork fork, Philosopher philosopher)
    {
        var forkMutex = fork.Mutex;
        forkMutex.WaitOne();
        
        var whoTryGet = philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot != null && Philosopher.PhilosopherIsOwnerBothFork(whoAlreadyGot))
        {
            forkMutex.ReleaseMutex();
            return false;
        }

        if (whoAlreadyGot != null && whoAlreadyGot.HungryStartTime < whoTryGet.HungryStartTime)
        {
            forkMutex.ReleaseMutex();
            return false;
        }

        fork.SetOwner(whoTryGet);
        
        forkMutex.ReleaseMutex();
        return true;
    }
}