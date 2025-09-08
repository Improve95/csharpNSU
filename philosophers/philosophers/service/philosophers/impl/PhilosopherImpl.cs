using System;

namespace philosophers.service.philosophers.impl;

public class PhilosopherImpl : IPhilosopher
{
    
    public PhilosopherStatus Status { get; set; } = PhilosopherStatus.Thinking;
}