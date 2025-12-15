namespace strategy.service;

public interface IDiscreteStrategy
{
    void DoStep(int step, bool enableLog);
}