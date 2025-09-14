using System.Text;
using philosophers.manager;
using philosophers.objects.fork;

namespace ManagementImpl.logger;

public abstract class PhilosopherLogger
{

    public static string CreateLog(
        int step,
        IPhilosopherManager[] philosopherManagers
    )
    {
        var sb = new StringBuilder();
        sb.Append($"===== STEP {step} =====\n");
        sb.Append("Philosophers:\n");

        var forks = new HashSet<Fork>();
        
        foreach (var philosopherManager in philosopherManagers)
        {
            sb.Append(string.Format(
                "{0}: {1}, action time remain: {2}, eating: {3}\n",
                philosopherManager.Philosopher.Name,
                philosopherManager.GetAction().ActionType,
                philosopherManager.GetAction().TimeIsRemain(),
                philosopherManager.Philosopher.TotalEating
            ));
            forks.Add(philosopherManager.GetRightFork());
        }
        
        sb.Append("\nForks:\n");
        foreach (var fork in forks)
        {
            sb.Append(string.Format(
                "Fork-{0}: {1}\n",
                fork.Id,
                fork.Owner == null ? "Available" : "In Use -" + fork.Owner.Name
            ));
        }
        
        return sb.ToString();
    }
}