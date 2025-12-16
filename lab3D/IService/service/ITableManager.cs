

using IService.objects;

namespace IService.service;

public interface ITableManager
{
    IFork GetLeftFork(int philosopherId);
    IFork GetRightFork(int philosopherId);
    IFork[] Forks { get; }
    IDictionary<int, IPhilosopher> Philosophers { get; }
    int PhilosopherCount { get; }
    void RegisterPhilosopher(IPhilosopher philosopher);
}