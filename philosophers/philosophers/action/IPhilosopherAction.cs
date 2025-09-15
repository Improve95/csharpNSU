namespace philosophers.action;

public interface IPhilosopherAction
{
    PhilosopherActionType ActionType { get; }
    
    bool TimeIsRemain();
}