using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.manager.impl;

public class ConcurrentPhilosopherManager(Philosopher philosopher, ConcurrentStrategy strategy): 
    AbstractPhilosopherManager(philosopher), 
    IConcurrentPhilosopherManager
{
    private bool ContinueWork { get; set; } = true;

    private int _hungryStartTime = DateTimeOffset.Now.Millisecond;

    private readonly AutoResetEvent _event = new(false);
    
    public void Process()
    {
        // V1();
        V2();
    }

    private void V1()
    {
        while (ContinueWork)
        {
            Thread.Sleep(GetAction().TimeRemain * 5);
            // Thread.Sleep(GetAction().TimeRemain * 25);
            
            PhilosopherActionType? newAction = null;
            while (newAction == null)
            {
                Console.WriteLine($"{Philosopher.Name} try get new act");
                // newAction = strategy.GetNewAction(this);
                Console.WriteLine($"{Philosopher.Name} got new act: {newAction}");
            }
            
            SetAction(newAction.Value);
            if (newAction == Hungry) _hungryStartTime = DateTimeOffset.Now.Millisecond;
        }
    }

    private void V2()
    {
        while (ContinueWork)
        {
            Thread.Sleep(GetAction().TimeRemain * 5);
            
            PhilosopherActionType? newAction = null;
            bool canStartNewAction;
            while (newAction == null)
            {
                // Console.WriteLine($"{Philosopher.Name} try get new act");
                var res = strategy.GetNewAction(this);
                newAction = res.newAction;
                canStartNewAction = res.canStartNewAction;
                // Console.WriteLine($"{Philosopher.Name} got new act: {newAction}, status: {canStartNewAction}");
                
                if (newAction == Hungry) _hungryStartTime = DateTimeOffset.Now.Millisecond;
                if (canStartNewAction) break;

                if (newAction == PhilosopherActionType.GetLeftFork)
                {
                    strategy.AddWaitingForkRelease((IConcurrentFork)GetLeftFork(), this);
                }
                else if (newAction == PhilosopherActionType.GetRightFork)
                {
                    strategy.AddWaitingForkRelease((IConcurrentFork)GetRightFork(), this);
                }
                
                _event.WaitOne();
            }
            SetAction(newAction.Value);
        }
    }
    
    public void WakeUp()
    {
        _event.Set();
    }
    
    public void Stop()
    {
        ContinueWork = false;
    }
    
    public override void SetAction(PhilosopherActionType philosopherAction)
    {
        Philosopher.PhilosopherAction = new ConcurrentPhilosopherAction(philosopherAction);
        if (philosopherAction == Eating)
        {
            Philosopher.IncreaseEating();
        }
    }
}
