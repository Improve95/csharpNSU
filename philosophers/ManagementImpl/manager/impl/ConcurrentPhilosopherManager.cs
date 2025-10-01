using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.philosophers;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.manager.impl;

public class ConcurrentPhilosopherManager(Philosopher philosopher, ConcurrentStrategy strategy): 
    AbstractPhilosopherManager(philosopher), 
    IConcurrentPhilosopherManager
{
    private bool ContinueWork { get; set; } = true;
    
    private readonly AutoResetEvent _event = new(false);
    
    public void Process()
    {
        // V1();
        V2();
    }

    /*private void V1()
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
            // if (newAction == Hungry) HungryStartTime = DateTimeOffset.Now.Millisecond;
        }
    }*/

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
                
                if (newAction == Hungry) SetStartHungryTime(DateTimeOffset.Now.Millisecond);
                if (canStartNewAction) break;

                // strategy.AddWaitingForkRelease((IConcurrentFork)GetLeftFork(), this);
                // strategy.AddWaitingForkRelease((IConcurrentFork)GetRightFork(), this);
                
                // _event.WaitOne();
            }
            SetAction(newAction.Value);
        }
    }

    public int GetStartHungryTime()
    {
        return Philosopher.HungryStartTime;
    }
    
    private void SetStartHungryTime(int startHungryTime)
    {
        Philosopher.HungryStartTime = startHungryTime;
    }
    
    public void WakeUp()
    {
        // Console.WriteLine($"wake up {Philosopher.Name}");
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
