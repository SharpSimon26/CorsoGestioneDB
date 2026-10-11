using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules.Resolution;
using CorsoGestioneDB.Domain.Entities;
using Moq;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.Resolution.Order;

public class ResolveSalesChannelRuleTests
{
    private readonly Mock<ICachedSalesChannelRepository> _repositoryMock;
    private readonly ResolveSalesChannelRule _rule;

    public ResolveSalesChannelRuleTests()
    {
        _repositoryMock = new Mock<ICachedSalesChannelRepository>();
        _rule = new ResolveSalesChannelRule(_repositoryMock.Object);
    }

    [Fact]
    public void CanApply_ReturnsTrue_When_SalesChannel_IsSet_And_Id_IsNull()
    {
        // Arrange
        var context = CreateContext("Ebay", null);
    
        // Act
        var result = _rule.CanApply(context);
    
        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CanApply_ReturnsFalse_When_SalesChannel_IsMissing(string? salesChannel)
    {
        // Arrange
        var context = CreateContext(salesChannel, null);

        // Act
        var result = _rule.CanApply(context);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanApply_ReturnsFalse_When_SalesChannelId_IsAlreadySet()
    {
        // Arrange
        var context = CreateContext("PayPal", 10);

        // Act
        var result = _rule.CanApply(context);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ApplyAsync_SetsSalesChannelId_WhenIdIsFound()
    {
        // Arrange
        var context = CreateContext("PayPal", null);

        var salesChannel = new SalesChannel
        {
            SalesChannelID = 42,
            SalesChannelName = "PayPal"
        };

        _repositoryMock.Setup(r => r.GetByNameAsync("PayPal"))
                       .ReturnsAsync(salesChannel);

        // Act
        await _rule.ApplyAsync(context);

        // Assert
        Assert.Equal(42, context.Data.Order.SalesChannelID);
        Assert.Single(context.Modifications);

        _repositoryMock.Verify(r => r.GetByNameAsync("PayPal"), Times.Once);
    }

    [Fact]
    public async Task ApplyAsync_DoesNotSet_SalesChannelId_WhenId_IsNotFound()
    {
        // Arrange
        var context = CreateContext("PayPal", null);

        _repositoryMock.Setup(r => r.GetByNameAsync("PayPal"))
                       .ReturnsAsync((SalesChannel?)null);

        // Act
        await _rule.ApplyAsync(context);

        // Assert
        Assert.Null(context.Data.Order.SalesChannelID);
        Assert.Single(context.Issues);

        _repositoryMock.Verify(r => r.GetByNameAsync("PayPal"), Times.Once);
    }

    private static ImportContext CreateContext(string? salesChannel, int? salesChannelId)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.SalesChannel = salesChannel;
        context.Data.Order.SalesChannelID = salesChannelId;

        return context;
    }
}