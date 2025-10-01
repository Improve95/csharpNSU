using ManagementImpl.service.impl;
using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.philosophers;

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
                newAction = strategy.GetNewAction(this);
                Console.WriteLine($"{Philosopher.Name} got new act: {newAction}");
            }
            
            SetAction(newAction.Value);
            if (newAction == PhilosopherActionType.Hungry) _hungryStartTime = DateTimeOffset.Now.Millisecond;
        }
    }

    private void V2()
    {
        while (ContinueWork)
        {
            Thread.Sleep(GetAction().TimeRemain * 5);
            
            PhilosopherActionType? newAction = null;
            while (newAction == null)
            {
                _event.WaitOne();
                
                Console.WriteLine($"{Philosopher.Name} try get new act");
                newAction = strategy.GetNewAction(this);
                Console.WriteLine($"{Philosopher.Name} got new act: {newAction}");
            }
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
        if (philosopherAction == PhilosopherActionType.Eating)
        {
            Philosopher.IncreaseEating();
        }
    }
}
