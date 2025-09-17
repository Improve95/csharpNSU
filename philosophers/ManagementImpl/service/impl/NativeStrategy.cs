using ManagementImpl.logger;
using ManagementImpl.manager.impl;
using ManagementImpl.metric;
using Microsoft.Extensions.Logging;
using philosophers.action;
using strategy.service;
using static philosophers.objects.fork.ForkStatus;

namespace ManagementImpl.service.impl;

public class NativeStrategy : IDiscreteStrategy
{
    
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<NativeStrategy>();

    private readonly DiscretePhilosopherManager[] _managers;
    
    private readonly PhilosopherMetricsCollector _metricsCollector;

    public NativeStrategy(
        DiscretePhilosopherManager[] managers,
        PhilosopherMetricsCollector metricsCollector)
    {
        _managers = managers;
        _metricsCollector = metricsCollector;
    }

    public void DoStep(int step, bool enableLog)
    {
        var tooMuchWaitingCounter = 0;
        foreach (var manager in _managers)
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
            }
            
            philosopherAction.ReduceTime();
            if (manager.GetAction().TimeRemain < 0)
            {
                tooMuchWaitingCounter++;
            }
        }
        
        if (enableLog) CreateLog(step);
        _metricsCollector.Collect(step);
        if (tooMuchWaitingCounter >= _managers.Length)
        {
            throw new Exception($"deadlock on {step} step");
        }
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
            if (leftFork.Status == Available)
            {
                leftFork.SetOwner(manager.Philosopher);
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
            if (rightFork.Status == Available)
            {
                rightFork.SetOwner(manager.Philosopher);
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
            manager.GetLeftFork().DropOwner();
            manager.GetRightFork().DropOwner();
            manager.SetAction(PhilosopherActionType.Thinking);
            return true;
        }
        
        return false;
    }

    private void CreateLog(int step)
    {
        var log = PhilosopherLogger.CreateLog(step, _managers);
        Logger.LogInformation(log);
        Thread.Sleep(500);   
    }
}
