using Service.objects.fork;

namespace Service.service;

public interface IForkFactory
{
    Fork GetLeftFork(int philosopherId);
    Fork GetRightFork(int philosopherId);
}