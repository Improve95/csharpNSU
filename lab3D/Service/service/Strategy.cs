using System.Collections.Concurrent;
using IService.objects;
using Service.objects.philosopher;
using Utils.action;

namespace Service.service;

public class Strategy
{
    private readonly IDictionary<int, Philosopher> _waitingFork = 
        new ConcurrentDictionary<int, Philosopher>();
    
    public (PhilosopherActionType? newAction, bool canStartNewAction) GetNewAction(Philosopher philosopher, CancellationToken stoppingToken)
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
            if (TryGetFork2(philosopher.LeftFork, philosopher, stoppingToken))
            {
                return (PhilosopherActionType.GetLeftFork, true);
            }
            return (PhilosopherActionType.GetLeftFork, false);
        }
        
        if (actionType is PhilosopherActionType.Hungry or PhilosopherActionType.GetLeftFork)
        {
            if (TryGetFork2(philosopher.RightFork, philosopher, stoppingToken))
            {
                return (PhilosopherActionType.GetRightFork, true);
            }
            return (PhilosopherActionType.GetRightFork, false);
        }

        if (actionType == PhilosopherActionType.Eating)
        {
            var leftFork = philosopher.LeftFork;
            leftFork.Lock.WaitAsync(stoppingToken);
            leftFork.DropOwner();
            leftFork.Lock.Release();
            WakeUpSleepingPhilosophers(leftFork);
            
            var rightFork = philosopher.RightFork;
            rightFork.Lock.WaitAsync(stoppingToken);
            rightFork.DropOwner();
            rightFork.Lock.Release();
            WakeUpSleepingPhilosophers(rightFork);
            
            return (PhilosopherActionType.Thinking, true);
        }
        
        return (null, false);
    }

    public void AddWaitingForkRelease(IFork fork, Philosopher philosopher)
    {
        _waitingFork[fork.Id] = philosopher;
    }

    private void WakeUpSleepingPhilosophers(IFork fork)
    {
        if (_waitingFork.TryGetValue(fork.Id, out var philosopher))
        {
            philosopher.WakeUp();
            _waitingFork.Remove(fork.Id);
        }
    }
    
    private bool TryGetFork2(IFork fork, Philosopher philosopher, CancellationToken stoppingToken)
    {
        var forkMutex = fork.Lock;
        forkMutex.WaitAsync(stoppingToken);
        
        var whoTryGet = philosopher;
        var whoAlreadyGot = fork.Owner;
        if (whoAlreadyGot != null && Philosopher.PhilosopherIsOwnerBothFork(whoAlreadyGot))
        {
            forkMutex.Release();
            return false;
        }

        if (whoAlreadyGot != null && whoAlreadyGot.HungryStartTime < whoTryGet.HungryStartTime)
        {
            forkMutex.Release();
            return false;
        }

        fork.SetOwner(whoTryGet);
        
        forkMutex.Release();
        return true;
    }
}