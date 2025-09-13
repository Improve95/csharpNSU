namespace philosophers.manager.action;

public interface IPhilosopherAction
{
    PhilosopherActionType ActionType { get; }
    
    bool TimeIsRemain();
}