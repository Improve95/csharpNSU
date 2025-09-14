using ManagementImpl.logger;
using Microsoft.Extensions.Logging;
using philosophers.manager.action;
using philosophers.manager.impl;
using strategy.service;

namespace ManagementImpl.service.impl;

public class PhilosopherDiscreteStrategy(DiscretePhilosopherManager[] philosopherManagers) : IDiscreteStrategy
{
    
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<PhilosopherDiscreteStrategy>();
    

    public void DoStep(int step)
    {
        var tooMuchWaitingCounter = 0;
        foreach (var philosopherManager in philosopherManagers)
        {

            var philosopherAction = philosopherManager.GetAction();
            if (!philosopherAction.TimeIsRemain())
            {
                if (StartHungry(philosopherManager)) continue;
                if (GetLeftFork(philosopherManager)) continue;
                if (GetRightFork(philosopherManager)) continue;
                if (StartEating(philosopherManager)) continue;
                if (ReleaseForks(philosopherManager)) continue;
                if (StartThinking(philosopherManager)) continue;
            };
            
            philosopherAction.ReduceTime();

            if (philosopherManager.GetAction().TimeRemain < 0)
            {
                tooMuchWaitingCounter++;
            }
        }
        
        var log = PhilosopherLogger.CreateLog(step, philosopherManagers);
        Logger.LogInformation(log);

        if (tooMuchWaitingCounter >= philosopherManagers.Length)
        {
            throw new Exception($"deadlock on {step} step");
        }
    }

    private bool StartHungry(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Thinking)
        {
            philosopherManager.SetAction(PhilosopherActionType.Hungry);
            return true;
        }

        return false;
    }
    
    private bool GetLeftFork(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry ||
            philosopherManager.GetAction().ActionType == PhilosopherActionType.GetRightFork)
        {
            var leftFork = philosopherManager.GetLeftFork();
            if (leftFork.Owner == null)
            {
                leftFork.Owner = philosopherManager.Philosopher;
                philosopherManager.SetAction(PhilosopherActionType.GetLeftFork);
                return true;
            }
        }

        return false;
    }

    private bool GetRightFork(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry ||
            philosopherManager.GetAction().ActionType == PhilosopherActionType.GetLeftFork)
        {
            var rightFork = philosopherManager.GetRightFork();
            if (rightFork.Owner == null)
            {
                rightFork.Owner = philosopherManager.Philosopher;
                philosopherManager.SetAction(PhilosopherActionType.GetRightFork);
                return true;
            }
        }

        return false;
    }

    private bool StartEating(DiscretePhilosopherManager philosopherManager)
    {
        var philosopher = philosopherManager.Philosopher;
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry &&
            philosopher.LeftFork.Owner == philosopher &&
            philosopher.RightFork.Owner == philosopher)
        {
            philosopherManager.SetAction(PhilosopherActionType.Eating);
            return true;
        }

        return false;
    }

    private bool ReleaseForks(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Eating)
        {
            philosopherManager.SetAction(PhilosopherActionType.ReleaseForks);
            return true;
        }

        return false;
    }
    
    private bool StartThinking(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.ReleaseForks)
        {
            philosopherManager.GetLeftFork().Owner = null;
            philosopherManager.GetRightFork().Owner = null;
            philosopherManager.SetAction(PhilosopherActionType.Thinking);
            return true;
        }

        return false;
    }

    private void CheckDeadlock()
    {
        foreach (var philosopherManager in philosopherManagers)
        {
            
        }
    }
}
