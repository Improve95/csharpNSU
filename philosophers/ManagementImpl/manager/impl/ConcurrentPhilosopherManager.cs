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

    public void Process()
    {
        while (ContinueWork)
        {
            Thread.Sleep(GetAction().TimeRemain * 100);

            PhilosopherActionType? newAction = null;
            while (newAction == null)
            {
                // Console.WriteLine($"{Philosopher.Name} try get new act");
                newAction = strategy.GetNewAction(this);
                // Console.WriteLine($"{Philosopher.Name} got new act: {newAction}");
            }
            
            SetAction(newAction.Value);
        }
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
