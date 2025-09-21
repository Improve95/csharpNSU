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
            Thread.Sleep(GetAction().TimeRemain);

            PhilosopherActionType? newAction = null;
            while (newAction == null)
            {
                newAction = strategy.GetNewAction(this);
                if (newAction == null)
                {
                    
                }
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
