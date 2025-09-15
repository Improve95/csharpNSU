using ManagementImpl.logger;
using ManagementImpl.manager.impl;
using Microsoft.Extensions.Logging;
using philosophers.action;
using strategy.service;

namespace ManagementImpl.service.impl;

public class DiscreteStrategy(DiscretePhilosopherManager[] philosopherManagers) : IDiscreteStrategy
{
    
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<DiscreteStrategy>();
    

    public void DoStep(int step)
    {
        var tooMuchWaitingCounter = 0;
        foreach (var manager in philosopherManagers)
        {

            var philosopherAction = manager.GetAction();
            if (!philosopherAction.TimeIsRemain())
            {
                if (StartHungry(manager)) continue;
                if (GetLeftFork(manager)) continue;
                if (GetRightFork(manager)) continue;
                if (StartEating(manager)) continue;
                if (ReleaseForks(manager)) continue;
                if (StartThinking(manager)) continue;
            };
            
            philosopherAction.ReduceTime();

            if (manager.GetAction().TimeRemain < 0)
            {
                tooMuchWaitingCounter++;
            }
        }
        
        var log = PhilosopherLogger.CreateLog(step, philosopherManagers);
        // Logger.LogInformation(log);

        if (tooMuchWaitingCounter >= philosopherManagers.Length)
        {
            throw new Exception($"deadlock on {step} step");
        }
        // Thread.Sleep(300);
    }

    private bool StartHungry(DiscretePhilosopherManager manager)
    {
        if (manager.GetActionType() == PhilosopherActionType.Thinking)
        {
            manager.SetAction(PhilosopherActionType.Hungry);
            return true;
        }

        return false;
    }
    
    private bool GetLeftFork(DiscretePhilosopherManager manager)
    {
        if (manager.GetActionType() == PhilosopherActionType.Hungry ||
            manager.GetActionType() == PhilosopherActionType.GetRightFork)
        {
            var leftFork = manager.GetLeftFork();
            if (leftFork.Owner == null)
            {
                leftFork.Owner = manager.Philosopher;
                manager.SetAction(PhilosopherActionType.GetLeftFork);
                return true;
            }
        }

        return false;
    }

    private bool GetRightFork(DiscretePhilosopherManager manager)
    {
        if (manager.GetActionType() == PhilosopherActionType.Hungry ||
            manager.GetActionType() == PhilosopherActionType.GetLeftFork)
        {
            var rightFork = manager.GetRightFork();
            if (rightFork.Owner == null)
            {
                rightFork.Owner = manager.Philosopher;
                manager.SetAction(PhilosopherActionType.GetRightFork);
                return true;
            }
        }

        return false;
    }

    private bool StartEating(DiscretePhilosopherManager manager)
    {
        if ((manager.GetActionType() == PhilosopherActionType.GetLeftFork || 
             manager.GetActionType() == PhilosopherActionType.GetRightFork) && 
            manager.PhilosopherIsOwnerBothFork())
        {
            manager.SetAction(PhilosopherActionType.Eating);
            return true;
        }

        return false;
    }

    private bool ReleaseForks(DiscretePhilosopherManager manager)
    {
        if (manager.GetActionType() == PhilosopherActionType.Eating)
        {
            manager.SetAction(PhilosopherActionType.ReleaseForks);
            return true;
        }

        return false;
    }
    
    private bool StartThinking(DiscretePhilosopherManager manager)
    {
        if (manager.GetActionType() == PhilosopherActionType.ReleaseForks)
        {
            manager.GetLeftFork().Owner = null;
            manager.GetRightFork().Owner = null;
            manager.SetAction(PhilosopherActionType.Thinking);
            return true;
        }

        return false;
    }
}
