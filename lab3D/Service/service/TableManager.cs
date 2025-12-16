using IService.service;
using Service.objects.fork;
using Service.objects.philosopher;

namespace Service.service.impl;

public class TableManager : ITableManager
{
    public int PhilosopherCount { get; }

    public Fork[] Forks { get; }

    public IDictionary<int, Philosopher> Philosophers { get; } 
    
    public TableManager(int philosopherCount)
    {
        PhilosopherCount = philosopherCount;
        Forks = Enumerable.Range(0, philosopherCount)
            .Select(i => new Fork(i))
            .ToArray();
        Philosophers = new Dictionary<int, Philosopher>();
    }

    public void RegisterPhilosopher(Philosopher philosopher)
    {
        Philosophers[philosopher.Id] = philosopher;
    }
    
    public Fork GetLeftFork(int philosopherId)
    {
        return Forks[philosopherId];
    }

    public Fork GetRightFork(int philosopherId)
    {
        return Forks[(philosopherId + 1) % PhilosopherCount];
    }
}