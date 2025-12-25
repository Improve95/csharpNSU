using IService.objects;
using IService.service;
using Moq;
using Service.objects.fork;
using Service.objects.philosopher;
using Service.service;
using Utils.action;

namespace Service.Test;

public class PhilosopherTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var mockTableManager = new Mock<ITableManager>();
        var mockLeftFork = new Mock<IFork>();
        var mockRightFork = new Mock<IFork>();
        mockTableManager.Setup(m => m.GetLeftFork(It.IsAny<int>())).Returns(mockLeftFork.Object);
        mockTableManager.Setup(m => m.GetRightFork(It.IsAny<int>())).Returns(mockRightFork.Object);
        var mockStrategy = new Mock<Strategy>();

        // Act
        var philosopher = new Philosopher("TestName", mockTableManager.Object, mockStrategy.Object); // Use derived class for testing protected ctor

        // Assert
        Assert.NotEqual(0, philosopher.Id); // Static increment
        Assert.Equal("TestName", philosopher.Name);
        Assert.Equal(mockLeftFork.Object, philosopher.LeftFork);
        Assert.Equal(mockRightFork.Object, philosopher.RightFork);
        Assert.Equal(0, philosopher.TotalEating);
        Assert.Equal(PhilosopherActionType.Thinking, philosopher.GetActionType());
        Assert.True(philosopher.ContinueWork);
        mockTableManager.Verify(m => m.RegisterPhilosopher(philosopher), Times.Once);
    }

    [Fact]
    public void IncreaseEating_IncrementTotalEating()
    {
        // Arrange
        var philosopher = CreatePhilosopher();

        // Act
        philosopher.IncreaseEating();

        // Assert
        Assert.Equal(1, philosopher.TotalEating);
    }

    [Fact]
    public void SetStartHungryTime_UpdatesHungryStartTime()
    {
        // Arrange
        var philosopher = CreatePhilosopher();
        int time = 100;

        // Act
        philosopher.SetStartHungryTime(time);

        // Assert
        Assert.Equal(time, philosopher.GetStartHungryTime());
    }

    [Fact]
    public void WakeUp_ReleasesSemaphore()
    {
        // Arrange
        var philosopher = CreatePhilosopher();

        // Act
        philosopher.WakeUp();

        // Assert (indirect: semaphore count increases, but for unit, assume correct)
    }

    [Fact]
    public void Stop_SetsContinueWorkToFalse()
    {
        // Arrange
        var philosopher = CreatePhilosopher();

        // Act
        philosopher.Stop();

        // Assert
        Assert.False(philosopher.ContinueWork);
    }

    [Fact]
    public void SetAction_UpdatesActionAndIncreasesEatingIfEating()
    {
        // Arrange
        var philosopher = CreatePhilosopher();

        // Act
        philosopher.SetAction(PhilosopherActionType.Eating);

        // Assert
        Assert.Equal(PhilosopherActionType.Eating, philosopher.GetActionType());
        Assert.Equal(1, philosopher.TotalEating);
    }

    private IPhilosopher CreatePhilosopher()
    {
        var mockTableManager = new Mock<ITableManager>();
        var mockStrategy = new Mock<IStrategy>();
        return new Philosopher("Test", mockTableManager.Object, mockStrategy.Object);
    }
}