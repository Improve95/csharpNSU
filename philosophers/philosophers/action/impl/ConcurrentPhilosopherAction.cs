using System;
using System.Collections.Generic;

namespace philosophers.action.impl;

public class ConcurrentPhilosopherAction: AbstractPhilosopherAction, IConcurrentPhilosopherAction
{
    private static readonly Dictionary<PhilosopherActionType, TimeRemainBorder> TimeRemainBorders = new()
    {
        { PhilosopherActionType.Thinking, new TimeRemainBorder(30, 100) },
        { PhilosopherActionType.Eating, new TimeRemainBorder(40, 50) },
        { PhilosopherActionType.GetLeftFork, new TimeRemainBorder(20, 20) },
        { PhilosopherActionType.GetRightFork, new TimeRemainBorder(20, 20) },
        { PhilosopherActionType.ReleaseLeftFork, new TimeRemainBorder(0, 0) },
        { PhilosopherActionType.ReleaseRightFork, new TimeRemainBorder(0, 0) },
        { PhilosopherActionType.ReleaseForks, new TimeRemainBorder(0, 0) }
    };
    
    public ConcurrentPhilosopherAction(PhilosopherActionType actionType) : base(actionType)
    {
        TimeRemain = Random.Next(
            TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Down,
            TimeRemainBorders.GetValueOrDefault(actionType, new TimeRemainBorder(0, 0)).Up
        );
    }
    
    private static readonly Random Random = new();

    public void ReduceTime(int subtractionTime)
    {
        TimeRemain -= subtractionTime;
    }
}