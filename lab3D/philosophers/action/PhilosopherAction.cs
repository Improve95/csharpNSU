namespace philosophers.action;

public class PhilosopherAction(PhilosopherActionType actionType)
{
    public PhilosopherActionType ActionType { get; } = actionType;

    private static readonly Random Random = new();

    public int TimeRemain { get; protected set; } = Random.Next(
        TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Down,
        TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Up
    );

    private static readonly Dictionary<PhilosopherActionType, TimeRemainBorder> TimeRemainBorders = new()
    {
        { PhilosopherActionType.Thinking, new TimeRemainBorder(30, 200) },
        { PhilosopherActionType.Eating, new TimeRemainBorder(40, 50) },
        { PhilosopherActionType.GetLeftFork, new TimeRemainBorder(20, 20) },
        { PhilosopherActionType.GetRightFork, new TimeRemainBorder(20, 20) },
        { PhilosopherActionType.ReleaseLeftFork, new TimeRemainBorder(0, 0) },
        { PhilosopherActionType.ReleaseRightFork, new TimeRemainBorder(0, 0) },
        { PhilosopherActionType.ReleaseForks, new TimeRemainBorder(0, 0) }
    };

    public bool TimeIsRemain()
    {
        return TimeRemain > 0;
    }

    protected record TimeRemainBorder(int Down, int Up);

    public void ReduceTime(int subtractionTime)
    {
        TimeRemain -= subtractionTime;
    }
}