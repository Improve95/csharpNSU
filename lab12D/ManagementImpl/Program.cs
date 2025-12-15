namespace ManagementImpl;

public abstract class Program
{
    public static void Main()
    {
        bool enableLog = true;
        // DiningPhilosophers.SimulateDiscrete(enableLog);
        // DiningPhilosophers.SimulateDiscreteCoordinator(enableLog);
        DiningPhilosophers.SimulateConcurrent(enableLog);
    }
}
