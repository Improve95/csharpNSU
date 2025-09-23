using System.Threading;

namespace philosophers.objects.fork;

public interface IConcurrentFork: IFork
{
    Mutex Mutex { get; }
}