using IService.objects;
using Utils.action;

namespace IService.service;

public interface IStrategy
{
    (PhilosopherActionType? newAction, bool canStartNewAction) GetNewAction(IPhilosopher philosopher, CancellationToken stoppingToken);
    void AddWaitingForkRelease(IFork fork, IPhilosopher philosopher);
}