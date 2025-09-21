using ManagementImpl.manager.impl;
using philosophers.action;
using strategy.service;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.service.impl;

public class ConcurrentStrategy : IConcurrentStrategy
{
    public PhilosopherActionType? GetNewAction(ConcurrentPhilosopherManager manager)
    {
        var actionType = manager.GetActionType();
        switch (actionType)
        {
            case Thinking:
                return Hungry;
            case Hungry or GetRightFork:
                // возьми левую вилку если можешь
                break;
            case GetLeftFork:
                // возьми правую вилку если можешь
                break;
            case Eating:
                return Thinking;
        }

        if (manager.PhilosopherIsOwnerBothFork())
        {
            return Eating;
        }
        
        return null;
    }
}