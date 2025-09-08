namespace philosophers.service.fork.impl;

public class ForkImpl : IFork
{

    public ForkStatus Status { get; set; } = ForkStatus.Available;
}