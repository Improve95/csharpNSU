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
    
    private static ILogger _logger = LoggerFactory.CreateLogger<PhilosopherDiscreteStrategy>();
    
    public void DoStep(int step)
    {
        var hungryCount = 0; 
        foreach (var philosopherManager in philosopherManagers)
        {
            var philosopherAction = philosopherManager.GetAction();
            philosopherAction.ReduceTime();
            if (philosopherAction.TimeIsRemain()) continue;

            /*switch (philosopherManager.GetAction().ActionType)
            {
                case PhilosopherActionType.Thinking:
                {
                    philosopherManager.SetAction(PhilosopherActionType.Hungry);
                    break;
                }
                case PhilosopherActionType.Hungry or PhilosopherActionType.TakenRightFork:
                {
                    var leftFork = philosopherManager.GetLeftFork();
                    if (leftFork.Owner == null)
                    {
                        leftFork.Owner = philosopherManager.Philosopher;
                        philosopherManager.SetAction(PhilosopherActionType.TakenLeftFork);
                    } 
                    break;
                }
                case PhilosopherActionType.Hungry or PhilosopherActionType.TakenLeftFork:
                { 
                    var rightFork = philosopherManager.GetRightFork();
                    if (rightFork.Owner == null)
                    {
                        rightFork.Owner = philosopherManager.Philosopher;
                        philosopherManager.SetAction(PhilosopherActionType.TakenRightFork);
                    }
                    break;   
                }
                case PhilosopherActionType.Eating:
                {
                    philosopherManager.SetAction(PhilosopherActionType.ReleaseForks);
                    break;
                }
                case PhilosopherActionType.ReleaseForks:
                {
                    philosopherManager.GetLeftFork().Owner = null;
                    philosopherManager.GetRightFork().Owner = null;
                    philosopherManager.SetAction(PhilosopherActionType.Thinking);
                    break;
                }
            }*/
            
            /*var philosopher = philosopherManager.Philosopher;
            if (philosopher.LeftFork.Owner == philosopher &&
                philosopher.RightFork.Owner == philosopher)
            {
                philosopherManager.SetAction(PhilosopherActionType.Eating);
            }*/
            
            StartHungry(philosopherManager);
            TakeLeftFork(philosopherManager);
            TakeRightFork(philosopherManager);
            StartEating(philosopherManager);
            ReleaseForks(philosopherManager);
            StartThinking(philosopherManager);

            if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry)
            {
                hungryCount++;
            }
        }

        if (hungryCount >= philosopherManagers.Length)
        {
            throw new Exception("deadlock");
        }
        
        var log = PhilosopherLogger.CreateLog(step, philosopherManagers);
        _logger.LogInformation(log);
    }

    private void StartHungry(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Thinking)
        {
            philosopherManager.SetAction(PhilosopherActionType.Hungry);
        }
    }
    
    private void TakeLeftFork(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry ||
            philosopherManager.GetAction().ActionType == PhilosopherActionType.TakenRightFork)
        {
            var leftFork = philosopherManager.GetLeftFork();
            if (leftFork.Owner == null)
            {
                leftFork.Owner = philosopherManager.Philosopher;
                philosopherManager.SetAction(PhilosopherActionType.TakenLeftFork);
            } 
        }
    }

    private void TakeRightFork(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Hungry ||
            philosopherManager.GetAction().ActionType == PhilosopherActionType.TakenLeftFork)
        {
            var rightFork = philosopherManager.GetRightFork();
            if (rightFork.Owner == null)
            {
                rightFork.Owner = philosopherManager.Philosopher;
                philosopherManager.SetAction(PhilosopherActionType.TakenRightFork);
            }
        }
    }

    private void StartEating(DiscretePhilosopherManager philosopherManager)
    {
        var philosopher = philosopherManager.Philosopher;
        if (philosopher.LeftFork.Owner == philosopher &&
            philosopher.RightFork.Owner == philosopher)
        {
            philosopherManager.SetAction(PhilosopherActionType.Eating);
        }
    }

    private void ReleaseForks(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.Eating)
        {
            philosopherManager.SetAction(PhilosopherActionType.ReleaseForks);
        }
    }
    
    private void StartThinking(DiscretePhilosopherManager philosopherManager)
    {
        if (philosopherManager.GetAction().ActionType == PhilosopherActionType.ReleaseForks)
        {
            philosopherManager.GetLeftFork().Owner = null;
            philosopherManager.GetRightFork().Owner = null;
            philosopherManager.SetAction(PhilosopherActionType.Thinking);
        }
    }
    
    private void ProcessOnHungryPhilosopher(DiscretePhilosopherManager philosopherManager)
    {
        /*var leftFork = philosopherManager.GetLeftFork();
        var rightFork = philosopherManager.GetRightFork();

        bool canEat = true;

        if (leftFork.Status == ForkStatus.Available)
        {
            leftFork.Status = ForkStatus.InUse;
        }
        else
        {
            canEat = false;
        }

        if (rightFork.Status == ForkStatus.Available)
        {
            rightFork.Status = ForkStatus.InUse;
        }
        else
        {
            canEat = false;
        }

        if (canEat)
        {
            philosopherManager.SetAction(PhilosopherActionType.Eating);
        }*/
    }
    
    private void ReduceActionRemainTime(DiscretePhilosopherManager philosopherManager)
    {
        var philosopherAction = philosopherManager.GetAction();
        philosopherAction.ReduceTime();
        
        // if (philosopherAction.ActionType == PhilosopherActionType.Hungry) return;
        if (!philosopherAction.TimeIsRemain()) return;
        
        // if (philosopherAction.ActionType == PhilosopherActionType.Thinking)
        // {
        //     philosopherManager.SetAction(PhilosopherActionType.Hungry);
        // }
    }
}
