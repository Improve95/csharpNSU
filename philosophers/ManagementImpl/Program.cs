namespace ManagementImpl;

public abstract class Program
{
    public static void Main()
    {
        bool enableLog = false;
        DiningPhilosophers.SimulateDiscrete(enableLog);
        // DiningPhilosophers.SimulateDiscreteCoordinator(enableLog);
    }
}