using System.Text;
using philosophers.manager;
using philosophers.objects.fork;

namespace ManagementImpl.logger;

public abstract class PhilosopherLogger
{

    public static string CreateLog(int step, IDiscretePhilosopherManager[] philosopherManagers) 
    {
        var sb = new StringBuilder();
        sb.Append($"===== STEP {step} =====\n");
        sb.Append("Philosophers:\n");

        var forks = new HashSet<Fork>();
        
        foreach (var manager in philosopherManagers)
        {
            sb.Append(string.Format(
                "{0}: {1}, remain: {2}, eating: {3}\n",
                manager.GetPhilosopherName(),
                manager.GetAction().ActionType,
                manager.GetAction().TimeRemain,
                manager.GetTotalEating()
            ));
            forks.Add(manager.GetLeftFork());
        }
        
        sb.Append("\nForks:\n");
        foreach (var fork in forks)
        {
            sb.Append(string.Format(
                "Fork-{0}: {1}\n",
                fork.Id,
                fork.Owner == null ? "Available" : "In Use - " + fork.Owner.Name
            ));
        }
        
        return sb.ToString();
    }
}