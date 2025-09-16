using ManagementImpl.manager;
using philosophers.objects.fork;
using static philosophers.objects.fork.ForkStatus;

namespace ManagementImpl.metric;

public class PhilosopherMetricsCollector
{
    private readonly IDiscretePhilosopherManager[] _managers;

    private readonly Fork[] _forks;

    private readonly IDictionary<int, PhilosopherStat> _philosopherStats = new Dictionary<int, PhilosopherStat>();
    
    private readonly IDictionary<int, ForkStat> _forkStats = new Dictionary<int, ForkStat>();
    
    public PhilosopherMetricsCollector(IDiscretePhilosopherManager[] managers, Fork[] forks)
    {
        _managers = managers;
        _forks = forks;
        foreach (var manager in managers)
        {
            _philosopherStats[manager.GetPhilosopherId()] = new PhilosopherStat();
        }
        foreach (var fork in _forks)
        {
            _forkStats[fork.Id] = new ForkStat();
        }
    }

    public void Collect(int step)
    {
        foreach (var fork in _forks)
        {
            var forkStat = _forkStats[fork.Id];
            forkStat.ForkStatusStat[fork.Status]++;
        }
    }

    public string GetFinalStat()
    {
        return null;
    }
    
    private class PhilosopherStat
    {
        private int TotalEating { get; set; }
    }
    
    private class ForkStat
    {
        public IDictionary<ForkStatus, int> ForkStatusStat { get; } = new Dictionary<ForkStatus, int>()
        {
            { InUse, 0 },
            { Available, 0 }
        };
    }
    
    public record FinalStat(int res);
}