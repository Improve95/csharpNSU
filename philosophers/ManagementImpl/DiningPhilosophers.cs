using ManagementImpl.service.impl;
using philosophers.manager.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl;

public abstract class DiningPhilosophers
{

    public static void Simulate()
    {
        var names = File.ReadAllLines("names.txt");

        var philosopherManagers = new DiscretePhilosopherManager[names.Length];
        var leftFork = new Fork();
        for (var i = 0; i < names.Length; i++)
        {
            var rightFork = new Fork();
            philosopherManagers[i] = new DiscretePhilosopherManager(
                new Philosopher(names[i], leftFork, rightFork)
            );
            leftFork = rightFork;
        }

        philosopherManagers[names.Length - 1].SetRightFork(philosopherManagers[0].GetLeftFork());
        
        var strategy = new DiscreteStrategy(philosopherManagers);
        for (var i = 0; i < 1_000_000; i++)
        {
            strategy.DoStep(i);
        }
    }
}
