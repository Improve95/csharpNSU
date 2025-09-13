namespace philosophers.manager.action;

public abstract class AbstractPhilosopherAction(PhilosopherActionType actionType) : IPhilosopherAction
{
    public PhilosopherActionType ActionType { get; } = actionType;

    protected int TimeRemain { get; set; }
    
    public bool TimeIsRemain()
    {
        return TimeRemain > 0;
    }

    protected record TimeRemainBorder(int Down, int Up);
}