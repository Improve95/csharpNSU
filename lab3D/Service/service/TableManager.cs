using IService.objects;
using IService.service;
using Service.objects.fork;

namespace Service.service;

public class TableManager : ITableManager
{
    public int PhilosopherCount { get; }

    public IFork[] Forks { get; }

    public IDictionary<int, IPhilosopher> Philosophers { get; } 
    
    public TableManager(int philosopherCount)
    {
        PhilosopherCount = philosopherCount;
        Forks = Enumerable.Range(0, philosopherCount)
            .Select(i => new Fork(i))
            .ToArray<IFork>();
        Philosophers = new Dictionary<int, IPhilosopher>();
    }

    public void RegisterPhilosopher(IPhilosopher philosopher)
    {
        Philosophers[philosopher.Id] = philosopher;
    }
    
    public IFork GetLeftFork(int philosopherId)
    {
        return Forks[philosopherId];
    }

    public IFork GetRightFork(int philosopherId)
    {
        return Forks[(philosopherId + 1) % PhilosopherCount];
    }
}