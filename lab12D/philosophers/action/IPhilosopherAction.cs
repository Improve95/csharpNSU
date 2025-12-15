namespace philosophers.action;

public interface IPhilosopherAction
{
    int TimeRemain { get; }
    PhilosopherActionType ActionType { get; }
    bool TimeIsRemain();
}