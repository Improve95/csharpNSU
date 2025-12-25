using System.Data;
using IService.objects;
using Moq;
using Service.objects.fork;
using Utils.fork;

namespace Service.Test;

public class ForkTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        int id = 1;

        // Act
        var fork = new Fork(id);

        // Assert
        Assert.Equal(id, fork.Id);
        Assert.Null(fork.Owner);
        Assert.Equal(ForkStatus.Available, fork.Status); // Assuming ForkStatus.Available is the default
        Assert.NotNull(fork.Lock);
    }

    [Fact]
    public void SetOwner_SetsOwnerAndUpdatesStatus_WhenOwnerIsNotNull()
    {
        // Arrange
        var fork = new Fork(1);
        var mockPhilosopher = new Mock<IPhilosopher>();

        // Act
        fork.SetOwner(mockPhilosopher.Object);

        // Assert
        Assert.Equal(mockPhilosopher.Object, fork.Owner);
        Assert.Equal(ForkStatus.InUse, fork.Status);
    }

    [Fact]
    public void SetOwner_ThrowsNoNullAllowedException_WhenOwnerIsNull()
    {
        // Arrange
        var fork = new Fork(1);

        // Act & Assert
        Assert.Throws<NoNullAllowedException>(() => fork.SetOwner(null));
    }

    [Fact]
    public void DropOwner_ClearsOwnerAndUpdatesStatus()
    {
        // Arrange
        var fork = new Fork(1);
        var mockPhilosopher = new Mock<IPhilosopher>();
        fork.SetOwner(mockPhilosopher.Object);

        // Act
        fork.DropOwner();

        // Assert
        Assert.Null(fork.Owner);
        Assert.Equal(ForkStatus.Available, fork.Status);
    }

    [Fact]
    public void IsOwner_ReturnsTrue_WhenPhilosopherIsOwner()
    {
        // Arrange
        var fork = new Fork(1);
        var mockPhilosopher = new Mock<IPhilosopher>();
        fork.SetOwner(mockPhilosopher.Object);

        // Act
        bool result = fork.IsOwner(mockPhilosopher.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOwner_ReturnsFalse_WhenPhilosopherIsNotOwner()
    {
        // Arrange
        var fork = new Fork(1);
        var mockOwner = new Mock<IPhilosopher>();
        var mockOther = new Mock<IPhilosopher>();
        fork.SetOwner(mockOwner.Object);

        // Act
        bool result = fork.IsOwner(mockOther.Object);

        // Assert
        Assert.False(result);
    }
}