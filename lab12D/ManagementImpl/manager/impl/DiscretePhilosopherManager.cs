using philosophers.action;
using philosophers.action.impl;
using philosophers.objects.philosophers;

namespace ManagementImpl.manager.impl;

public class DiscretePhilosopherManager(Philosopher philosopher) : AbstractPhilosopherManager(philosopher)
{
    public override void SetAction(PhilosopherActionType philosopherAction)
    {
        Philosopher.PhilosopherAction = new DiscretePhilosopherAction(philosopherAction);
        if (philosopherAction == PhilosopherActionType.Eating)
        {
            Philosopher.IncreaseEating();
        }
    }
}