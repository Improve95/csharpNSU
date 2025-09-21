namespace philosophers.action;

public interface IDiscretePhilosopherAction
{
    PhilosopherActionType ActionType { get; }
    bool TimeIsRemain();
}