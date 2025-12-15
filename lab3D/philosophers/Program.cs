using Microsoft.Extensions.Hosting;

namespace philosophers;

public abstract class Program
{
    public static void Main()
    {
        var builder = Host.CreateApplicationBuilder();
        // builder.Services.AddHostedService<>();
        
        var host = builder.Build();
        host.Run();
    }
}