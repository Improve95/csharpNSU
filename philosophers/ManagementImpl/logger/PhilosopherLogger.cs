using System.Text;
using ManagementImpl.manager;
using philosophers.objects.fork;
using static ManagementImpl.metric.DiscretePhilosopherMetricsCollector;

namespace ManagementImpl.logger;

public abstract class PhilosopherLogger
{
    public static string CreateLog(int step, IPhilosopherManager[] philosopherManagers) 
    {
        var sb = new StringBuilder();
        sb.Append($"===== STEP {step} =====\n");
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
    
    public static string CreateStatLog(FinalStat stat)
    {
        var sb = new StringBuilder();

        sb.AppendLine("===== Итоговая статистика философов =====");
        sb.AppendLine();

        sb.AppendLine("Общие показатели:");
        sb.AppendLine($"Средняя скорость еды по всем философам: {stat.MiddleEatingSpeedsByAll}");
        sb.AppendLine($"Среднее время голода по всем философам: {stat.MiddleHungryTimesByAll}");
        sb.AppendLine($"Максимальное время ожидания голода: {stat.MaxHungryWaitingTime}");

        if (stat.TheMostHungry != null) 
            sb.AppendLine($"Самый голодный философ: {stat.TheMostHungry.Philosopher.Name}");

        sb.AppendLine();
        sb.AppendLine("Статистика по философам:");
        for (var i = 0; i < stat.Managers.Length; i++)
        {
            var manager = stat.Managers[i];
            var eatingSpeed = stat.MiddleEatingSpeeds.ElementAtOrDefault(i);
            var hungryTime = stat.MiddleHungryTimes.ElementAtOrDefault(i);

            sb.AppendLine($"{manager.GetPhilosopherName()} id-{manager.GetPhilosopherId()}:");
            sb.AppendLine($"  Средняя скорость еды: {eatingSpeed}");
            sb.AppendLine($"  Среднее время голода: {hungryTime}");
            sb.AppendLine();
        }

        sb.AppendLine("Статистика вилок:");
        for (var i = 0; i < stat.Forks.Length; i++)
        {
            DiscreteFork fork = stat.Forks[i];
            sb.AppendLine($"Вилка id-{fork.Id}:");
            if (i < stat.ForksStatusesTimes.Count)
            {
                foreach (var status in stat.ForksStatusesTimes[i])
                    sb.AppendLine($"  {status}");
            }
            sb.AppendLine();
        }

        sb.AppendLine("===== Конец статистики =====");
        return sb.ToString();
    }
}