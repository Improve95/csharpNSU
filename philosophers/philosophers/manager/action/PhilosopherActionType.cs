namespace philosophers.manager.action;

public enum PhilosopherActionType
{
    Thinking,
    
    Hungry,

    GetLeftFork,
    
    TakenLeftFork,
    
    GetRightFork,
    
    TakenRightFork,

    Eating,

    ReleaseRightFork,
    
    ReleaseLeftFork,
    
    ReleaseForks
}