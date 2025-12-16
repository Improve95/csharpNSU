using Service.objects.fork;

namespace Service.service.impl;

public class ForkFactory : IForkFactory
{
    private readonly int _philosopherCount;

    private readonly Fork[] _forks;
    
    public ForkFactory(int philosopherCount)
    {
        _philosopherCount = philosopherCount;
        _forks = Enumerable.Range(0, philosopherCount)
            .Select(i => new Fork(i))
            .ToArray();
    }

    public Fork GetLeftFork(int philosopherId)
    {
        return _forks[philosopherId];
    }

    public Fork GetRightFork(int philosopherId)
    {
        return _forks[(philosopherId + 1) % _philosopherCount];
    }
}