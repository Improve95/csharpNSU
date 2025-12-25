using Utils.action;

namespace IService.objects;

public interface IPhilosopher
{
    int Id { get; }
    string Name { get; }
    PhilosopherAction Action { get; set; }
    IFork LeftFork { get; set; }
    IFork RightFork { get; set;  }
    int TotalEating { get; }
    int HungryStartTime { get; set; }
    bool ContinueWork { get; set; }
    void IncreaseEating();
    int GetStartHungryTime();
    void SetStartHungryTime(int startHungryTime);
    void WakeUp();
    void Stop();
    void SetAction(PhilosopherActionType philosopherAction);
    PhilosopherActionType GetActionType();
    bool PhilosopherIsOwnerBothFork();
}