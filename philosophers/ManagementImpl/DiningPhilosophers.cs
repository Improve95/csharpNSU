using ManagementImpl.manager.impl;
using ManagementImpl.manager.impl.coordinator;
using ManagementImpl.service.impl;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace ManagementImpl;

public abstract class DiningPhilosophers
{

    public static void SimulateDiscrete()
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

    public static void SimulateDiscreteCoordinator()
    {
        var names = File.ReadAllLines("names.txt");

        var philosopherManagers = new DiscreteCoordinatorPhilosopherManager[names.Length];
        var forks = new Fork[names.Length];
        var leftFork = new Fork();
        for (var i = 0; i < names.Length; i++)
        {
            var rightFork = new Fork();
            philosopherManagers[i] = new DiscreteCoordinatorPhilosopherManager(
                new Philosopher(names[i], leftFork, rightFork)
            );
            forks[i % names.Length] = rightFork;
            leftFork = rightFork;
        }

        philosopherManagers[names.Length - 1].SetRightFork(philosopherManagers[0].GetLeftFork());

        var coordinator = new DiscreteCoordinator(philosopherManagers, forks);
        var strategy = new DiscreteCoordinatorStrategy(coordinator);
        coordinator.SetStrategy(strategy);
        
        for (var i = 0; i < 1_000_000; i++)
        {
            strategy.DoStep(i);
        }
    }
}
