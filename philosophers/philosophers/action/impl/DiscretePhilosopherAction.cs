using System;
using System.Collections.Generic;

namespace philosophers.action.impl;

public class DiscretePhilosopherAction : AbstractPhilosopherAction
{
    
    private static readonly Dictionary<PhilosopherActionType, TimeRemainBorder> TimeRemainBorders = new()
    {
        { PhilosopherActionType.Thinking, new TimeRemainBorder(3, 10) },
        { PhilosopherActionType.Eating, new TimeRemainBorder(4, 5) },
        { PhilosopherActionType.GetLeftFork, new TimeRemainBorder(2, 2) },
        { PhilosopherActionType.GetRightFork, new TimeRemainBorder(2, 2) },
        { PhilosopherActionType.ReleaseLeftFork, new TimeRemainBorder(1, 1) },
        { PhilosopherActionType.ReleaseRightFork, new TimeRemainBorder(1, 1) },
        { PhilosopherActionType.ReleaseForks, new TimeRemainBorder(1, 1) }
    };

    private static readonly Random Random = new();

    public DiscretePhilosopherAction(PhilosopherActionType actionType) : base(actionType)
    {
        TimeRemain = Random.Next(
            TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Down,
            TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Up
        );
    }
    
    public void ReduceTime()
    {
        TimeRemain--;
    }
}
