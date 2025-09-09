using System;
using System.Collections.Generic;
using System.Linq;
using Loggier.service;
using philosophers.objects.fork;
using philosophers.objects.philosophers;

namespace philosophers;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var philosophersName = new List<string> { "Платон", "Аристотель", "Сократ", "Декарт", "Кант" };

        var fork = new Fork();
        Console.WriteLine(fork.Id);
        
        var f2 = new Fork();
        Console.WriteLine(f2.Id);
        
        var f3 = new Fork();
        Console.WriteLine(f3.Id);

        Console.WriteLine();
        
        var philosophers = philosophersName
            .Select(n => new Philosopher(n))
            .ToList();

        var philosopher = philosophers[0];
        Console.WriteLine(philosopher.Id);
        Console.WriteLine(philosophers[1].Id + " " + philosophers[1].Name);
        Console.WriteLine(philosophers[2].Id);

        Console.WriteLine();
        
        Console.WriteLine(philosopher.TotalEating);
        philosopher.AddEating();
        Console.WriteLine(philosopher.TotalEating);

        var logger = new DefaultLogger();
        logger.CreateSomeLogs();
    }
}