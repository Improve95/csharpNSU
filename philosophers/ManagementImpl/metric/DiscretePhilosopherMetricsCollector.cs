using ManagementImpl.manager;
using philosophers.action;
using philosophers.objects.fork;
using static philosophers.action.PhilosopherActionType;

namespace ManagementImpl.metric;

public class DiscretePhilosopherMetricsCollector
{
    private readonly IDiscretePhilosopherManager[] _managers;

    private readonly Fork[] _forks;

    private readonly IDictionary<int, PhilosopherStat> _philosopherStats = new Dictionary<int, PhilosopherStat>();
    
    private readonly IDictionary<int, ForkStat> _forkStats = new Dictionary<int, ForkStat>();

    private const int StepForSpeed = 1000;

    public DiscretePhilosopherMetricsCollector(IDiscretePhilosopherManager[] managers, Fork[] forks)
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
        CollectPhilosophersStat(step);
        CollectForksStat();
    }

    public FinalStat GetFinalStat()
    {
        var middleEatingSpeeds = new List<double>();
        var hungryTimes = new List<double>();
        foreach (var manager in _managers)
        {
            var stat = _philosopherStats[manager.GetPhilosopherId()];
            middleEatingSpeeds.Add(stat.MiddleEatingCount);
            hungryTimes.Add(stat.MiddleHungryTime);
        }

        var forksStatusesTimes = new List<List<ForkStatusesTime>>();
        foreach (var fork in _forks)
        {
            var stat = _forkStats[fork.Id];
            var forkStatusesTimes = stat.ForkStatusStat
                .Select(keyValuePair => new ForkStatusesTime(keyValuePair.Key, keyValuePair.Value))
                .ToList();
            forksStatusesTimes.Add(forkStatusesTimes);
        }
        
        return new FinalStat(
            _managers,
            _forks,
            middleEatingSpeeds,
            PhilosopherStat.MiddleEatingSpeedByAll,
            hungryTimes,
            PhilosopherStat.MiddleHungryTimeByAll,
            PhilosopherStat.MaxHungryWaitingTime,
            PhilosopherStat.TheMostHungry,
            forksStatusesTimes);
    }
    
    private void CollectPhilosophersStat(int step)
    {
        PhilosopherStat.MiddleHungryTimeByAll = PhilosopherStat.TotalHungryTimeByAll / 
                                                (double)PhilosopherStat.TotalHungryCountByAll;
        if (step % StepForSpeed == 0)
        {
            PhilosopherStat.MiddleEatingSpeedByAll = PhilosopherStat.TotalEatingSpeedByAll 
                                                     / (double)step;
        }
        
        foreach (var manager in _managers)
        {
            var philosopherStat = _philosopherStats[manager.GetPhilosopherId()];
            var currentAction = manager.GetAction().ActionType;

            if (step % StepForSpeed == 0)
            {
                philosopherStat.MiddleHungryTime = philosopherStat.CurrentHungryTime / (double)StepForSpeed;
                philosopherStat.MiddleEatingCount = philosopherStat.CurrentEatingCount / (double)StepForSpeed;
                philosopherStat.CurrentEatingCount = 0;
                philosopherStat.CurrentHungryTime = 0;
            }
            
            if (currentAction == Hungry)
            {
                philosopherStat.CurrentHungryTime++;
                if (philosopherStat.CurrentHungryTime > PhilosopherStat.MaxHungryWaitingTime)
                {
                    PhilosopherStat.MaxHungryWaitingTime = philosopherStat.CurrentHungryTime;
                    PhilosopherStat.TheMostHungry = manager;
                }
            }
            else
            {
                if (philosopherStat.PrevAction == Hungry)
                {
                    PhilosopherStat.TotalHungryCountByAll++;
                    PhilosopherStat.TotalHungryTimeByAll += philosopherStat.CurrentHungryTime;
                }
            }

            if (currentAction == Eating && philosopherStat.PrevAction != Eating)
            {
                philosopherStat.CurrentEatingCount++;
                PhilosopherStat.TotalEatingSpeedByAll++;
            }
            
            philosopherStat.PrevAction = currentAction;
        }
    }

    private void CollectForksStat()
    {
        foreach (var fork in _forks)
        {
            var forkStat = _forkStats[fork.Id];
            forkStat.ForkStatusStat[fork.Status]++;
        }
    }
    
    public record FinalStat(
        IDiscretePhilosopherManager[] Managers,
        Fork[] Forks,
        List<double> MiddleEatingSpeeds,
        double MiddleEatingSpeedsByAll,
        List<double> MiddleHungryTimes,
        double MiddleHungryTimesByAll,
        int MaxHungryWaitingTime,
        IDiscretePhilosopherManager? TheMostHungry,
        List<List<ForkStatusesTime>> ForksStatusesTimes);
    
    private class PhilosopherStat
    {
        public static int MaxHungryWaitingTime { get; set; }
        public static IDiscretePhilosopherManager? TheMostHungry { get; set; }
        public static int TotalHungryTimeByAll { get; set; }
        public static int TotalHungryCountByAll { get; set; }
        public static double MiddleHungryTimeByAll { get; set; }

        public static int TotalEatingSpeedByAll { get; set; }
        public static double MiddleEatingSpeedByAll { get; set; }

        public double MiddleHungryTime { get; set; }
        public int CurrentHungryTime { get; set; }

        public double MiddleEatingCount { get; set; }
        public int CurrentEatingCount { get; set; }

        public PhilosopherActionType? PrevAction { get; set; }
        
        public IDictionary<PhilosopherActionType, int> PhilosophersStatusStat { get; } = 
            Enum.GetValues<PhilosopherActionType>()
                .ToDictionary(type => type, _ => 0);
    }
    
    private class ForkStat
    {
        public IDictionary<ForkStatus, int> ForkStatusStat { get; } = 
            Enum.GetValues<ForkStatus>()
                .ToDictionary(type => type, _ => 0);
    }

    public record ForkStatusesTime(ForkStatus Status, int Time);
}