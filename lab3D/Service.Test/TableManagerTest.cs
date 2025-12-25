using IService.objects;
using Moq;
using Service.service;

namespace Service.Test;

public class TableManagerTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        int philosopherCount = 5;

        // Act
        var manager = new TableManager(philosopherCount);

        // Assert
        Assert.Equal(philosopherCount, manager.PhilosopherCount);
        Assert.Equal(philosopherCount, manager.Forks.Length);
        Assert.Empty(manager.Philosophers);
        for (int i = 0; i < philosopherCount; i++)
        {
            Assert.Equal(i, manager.Forks[i].Id);
        }
    }

    [Fact]
    public void RegisterPhilosopher_AddsPhilosopherToDictionary()
    {
        // Arrange
        var manager = new TableManager(5);
        var mockPhilosopher = new Mock<IPhilosopher>();
        mockPhilosopher.Setup(p => p.Id).Returns(2);

        // Act
        manager.RegisterPhilosopher(mockPhilosopher.Object);

        // Assert
        Assert.Single(manager.Philosophers);
        Assert.Equal(mockPhilosopher.Object, manager.Philosophers[2]);
    }

    [Fact]
    public void GetLeftFork_ReturnsCorrectFork()
    {
        // Arrange
        var manager = new TableManager(5);
        int philosopherId = 3;

        // Act
        var fork = manager.GetLeftFork(philosopherId);

        // Assert
        Assert.Equal(philosopherId, fork.Id);
    }

    [Fact]
    public void GetRightFork_ReturnsCorrectFork_WithWrapAround()
    {
        // Arrange
        var manager = new TableManager(5);
        int philosopherId = 4; // Last one, right should be 0

        // Act
        var fork = manager.GetRightFork(philosopherId);

        // Assert
        Assert.Equal(0, fork.Id);
    }

    [Fact]
    public void GetRightFork_ReturnsCorrectFork_WithoutWrapAround()
    {
        // Arrange
        var manager = new TableManager(5);
        int philosopherId = 2;

        // Act
        var fork = manager.GetRightFork(philosopherId);

        // Assert
        Assert.Equal(3, fork.Id);
    }
}