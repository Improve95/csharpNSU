using System;
using philosophers.service.philosophers;
using philosophers.service.philosophers.impl;

namespace philosophers;

public abstract class Program
{
    public static void Main(string[] args)
    { 
        Console.WriteLine("Hello, World!");

        var philosopher = new PhilosopherImpl();
        philosopher.Status = PhilosopherStatus.Hungry;
        
        Console.WriteLine(philosopher.ToString());
        Console.WriteLine(philosopher.Status);
    }
}