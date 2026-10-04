using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Pipeline.Rules.Resolution;
using CorsoGestioneDB.Domain.Entities;
using Moq;

namespace CorsoGestioneDB.Application.Tests.Pipeline.Rules.Resolution.Order;

public class ResolvePaymentMethodRuleTests
{
    private readonly Mock<ICachedPaymentMethodRepository> _repositoryMock;
    private readonly ResolvePaymentMethodRule _rule;

    public ResolvePaymentMethodRuleTests()
    {
        _repositoryMock = new Mock<ICachedPaymentMethodRepository>();
        _rule = new ResolvePaymentMethodRule(_repositoryMock.Object);
    }

    [Fact]
    public void CanApply_ReturnsTrue_WhenPaymentMethod_IsSet_And_Id_IsNull()
    {
        // Arrange
        var context = CreateContext(paymentMethod: "PayPal", paymentMethodId: null);

        // Act
        var result = _rule.CanApply(context);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CanApply_ReturnsFalse_When_PaymentMethod_IsMissing(string? paymentMethod)
    {
        // Arrange
        var context = CreateContext(paymentMethod: paymentMethod, paymentMethodId: null);

        // Act
        var result = _rule.CanApply(context);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanApply_ReturnsFalse_When_PaymentMethodId_IsAlreadySet()
    {
        // Arrange
        var context = CreateContext(paymentMethod: "PayPal", paymentMethodId: 10);

        // Act
        var result = _rule.CanApply(context);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ApplyAsync_SetsPaymentMethodId_WhenMethodIsFound()
    {
        // Arrange
        var context = CreateContext(paymentMethod: "PayPal", paymentMethodId: null);

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodID = 42,
            PaymentMethodName = "PayPal"
        };

        _repositoryMock.Setup(r => r.GetByNameAsync("PayPal"))
                       .ReturnsAsync(paymentMethod);

        // Act
        await _rule.ApplyAsync(context);

        // Assert
        Assert.Equal(42, context.Data.Order.PaymentMethodID);
        Assert.Single(context.Modifications);

        _repositoryMock.Verify(r => r.GetByNameAsync("PayPal"), Times.Once);
    }

    [Fact]
    public async Task ApplyAsync_DoesNotSet_PaymentMethodId_WhenMethod_IsNotFound()
    {
        // Arrange
        var context = CreateContext(paymentMethod: "PayPal", paymentMethodId: null);

        _repositoryMock.Setup(r => r.GetByNameAsync("PayPal"))
                       .ReturnsAsync((PaymentMethod?)null);

        // Act
        await _rule.ApplyAsync(context);

        // Assert
        Assert.Null(context.Data.Order.PaymentMethodID);
        Assert.Single(context.Issues);

        _repositoryMock.Verify(r => r.GetByNameAsync("PayPal"), Times.Once);
    }

    private static ImportContext CreateContext(string? paymentMethod, int? paymentMethodId)
    {
        var context = new ImportContext(new StagingOrder());
        context.Data.Order.PaymentMethod = paymentMethod;
        context.Data.Order.PaymentMethodID = paymentMethodId;

        return context;
    }
}