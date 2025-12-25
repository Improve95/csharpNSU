using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Service.config;
using Service.service;

namespace Service.Test;

public class SimulationLifetimeServiceTests
{
    [Fact]
    public async Task ExecuteAsync_DelaysForConfiguredDuration_ThenStopsApplication()
    {
        // Arrange
        var mockLifetime = new Mock<IHostApplicationLifetime>();
        var mockOptions = new Mock<IOptions<SimulationConfiguration>>();
        var mockLogger = new Mock<ILogger<SimulationLifetimeService>>();
        var config = new SimulationConfiguration { DurationSeconds = 1 };
        mockOptions.Setup(o => o.Value).Returns(config);

        var service = new SimulationLifetimeService(mockLifetime.Object, mockOptions.Object, mockLogger.Object);
        var cancellationToken = new CancellationToken(false);

        // Act
        await service.StartAsync(cancellationToken); // Start to trigger ExecuteAsync
        await Task.Delay(1500); // Wait slightly longer than delay to allow completion

        // Assert
        mockLifetime.Verify(l => l.StopApplication(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_CancelsGracefully_WhenTokenIsCancelled()
    {
        // Arrange
        var mockLifetime = new Mock<IHostApplicationLifetime>();
        var mockOptions = new Mock<IOptions<SimulationConfiguration>>();
        var mockLogger = new Mock<ILogger<SimulationLifetimeService>>();
        var config = new SimulationConfiguration { DurationSeconds = 10 };
        mockOptions.Setup(o => o.Value).Returns(config);

        var service = new SimulationLifetimeService(mockLifetime.Object, mockOptions.Object, mockLogger.Object);
        var cts = new CancellationTokenSource();

        // Act
        var task = service.StartAsync(cts.Token);
        cts.Cancel();
        await task;

        // Assert
        mockLifetime.Verify(l => l.StopApplication(), Times.Never);
    }
}