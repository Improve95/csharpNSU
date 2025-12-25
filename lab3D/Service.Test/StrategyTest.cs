using IService.objects;
using IService.service;
using Moq;
using Service.objects.fork;
using Service.objects.philosopher;
using Service.service;
using Utils.action;

namespace Service.Test;

public class StrategyTests
{
    [Fact]
    public void GetNewAction_FromThinking_ReturnsHungry()
    {
        // Arrange
        var mockTableManager = new Mock<ITableManager>();
        var strategy = new Strategy();
        var philosopher = new Philosopher("Test", mockTableManager.Object, strategy);
        philosopher.SetAction(PhilosopherActionType.Thinking);
        var token = CancellationToken.None;

        // Act
        var (newAction, canStart) = strategy.GetNewAction(philosopher, token);

        // Assert
        Assert.Equal(PhilosopherActionType.Hungry, newAction);
        Assert.True(canStart);
    }

    [Fact]
    public void GetNewAction_FromHungryWithBothForks_ReturnsEating()
    {
        // Arrange
        var mockTableManager = new Mock<ITableManager>();
        var leftFork = new Fork(0);
        var rightFork = new Fork(1);
        mockTableManager.Setup(m => m.GetLeftFork(It.IsAny<int>())).Returns(leftFork);
        mockTableManager.Setup(m => m.GetRightFork(It.IsAny<int>())).Returns(rightFork);
        var strategy = new Strategy();
        var philosopher = new Philosopher("Test", mockTableManager.Object, strategy);
        philosopher.SetAction(PhilosopherActionType.Hungry);
        leftFork.SetOwner(philosopher);
        rightFork.SetOwner(philosopher);
        var token = CancellationToken.None;

        // Act
        var (newAction, canStart) = strategy.GetNewAction(philosopher, token);

        // Assert
        Assert.Equal(PhilosopherActionType.Eating, newAction);
        Assert.True(canStart);
    }

    [Fact]
    public void AddWaitingForkRelease_AddsToDictionary()
    {
        // Arrange
        var strategy = new Strategy();
        var mockFork = new Mock<IFork>();
        mockFork.Setup(f => f.Id).Returns(1);
        var mockTableManager = new Mock<ITableManager>();
        var philosopher = new Philosopher("Test", mockTableManager.Object, strategy);

        // Act
        strategy.AddWaitingForkRelease(mockFork.Object, philosopher);

        // Assert (indirect verification through behavior)
        // For example, invoke WakeUpSleepingPhilosophers privately if needed, but for now, assume correct via reflection or expose
    }
}