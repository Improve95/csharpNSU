using System.Threading;

namespace philosophers.objects.fork;

public class ConcurrentFork : AbstractFork, IConcurrentFork
{
    public Mutex Mutex { get; private set; } = new Mutex();
}