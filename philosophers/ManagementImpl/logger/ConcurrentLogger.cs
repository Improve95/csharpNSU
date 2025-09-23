using System.Text;
using ManagementImpl.manager;
using Microsoft.Extensions.Logging;
using philosophers.objects.fork;

namespace ManagementImpl.logger;

public class ConcurrentLogger(IPhilosopherManager[] philosopherManagers, IFork[] forks, int delay)
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(
        builder => builder.AddConsole()
    );
    
    private static readonly ILogger Logger = LoggerFactory.CreateLogger<ConcurrentLogger>();
    
    private bool ContinueWork { get; set; } = true;
    
    public void Logging()
    {
        while (ContinueWork)
        {
            Logger.LogInformation(CreateConcurrentLog());
            Thread.Sleep(delay);
        }
    }

    private string CreateConcurrentLog()
    {
        var sb = new StringBuilder();

        sb.Append($"Timestamp: {DateTime.Now.ToString("HH:mm:ss.fff")}\n");
        sb.Append("Philosophers:\n");

        var forks = new HashSet<IFork>();
        
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
                "Fork-{0}: {1} {2}\n", 
                fork.Id, 
                fork.Status, 
                fork.Status == ForkStatus.Available ? "Available" : "In Use - " + fork.Owner?.Name
            ));
        }
        
        return sb.ToString();
    }
    
    public void Stop()
    {
        ContinueWork = false;
    }
}