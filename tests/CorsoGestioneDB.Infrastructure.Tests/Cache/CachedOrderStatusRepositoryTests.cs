using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Cache;
using Moq;

namespace CorsoGestioneDB.Infrastructure.Tests.Cache;

public class CachedOrderStatusRepositoryTests
{
    private readonly Mock<IOrderStatusRepository> _mockRepository;
    private readonly List<OrderStatus> _fakeStatuses;

    public CachedOrderStatusRepositoryTests()
    {
        _mockRepository = new Mock<IOrderStatusRepository>();

        _fakeStatuses = new List<OrderStatus>
        {
            new() { OrderStatusID = 1, OrderStatusName = "In lavorazione" },
            new() { OrderStatusID = 2, OrderStatusName = "Spedito" },
            new() { OrderStatusID = 3, OrderStatusName = "Consegnato" }
        };
    }

    [Fact]
    public async Task GetAllAsync_FirstCall_ShouldFetchFromRepository()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        var result = await cachedRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_MultipleCalls_ShouldUseCacheAndCallRepositoryOnlyOnce()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        await cachedRepository.GetAllAsync();
        await cachedRepository.GetAllAsync();
        var result = await cachedRepository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
        // Verifichiamo che il DB/Repository sia stato chiamato 1 sola volta in totale
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByNameAsync_WhenExists_ShouldReturnOrderStatus()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        var result = await cachedRepository.GetByNameAsync("Spedito");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.OrderStatusID);
        Assert.Equal("Spedito", result.OrderStatusName);
    }

    [Theory]
    [InlineData("spedito")]
    [InlineData("SPEDITO")]
    [InlineData("SpEdItO")]
    public async Task GetByNameAsync_ShouldBeCaseInsensitive(string searchName)
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        var result = await cachedRepository.GetByNameAsync(searchName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Spedito", result.OrderStatusName);
    }

    [Fact]
    public async Task GetByNameAsync_WhenDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        var result = await cachedRepository.GetByNameAsync("Inesistente");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByNameAsync_Then_GetAllAsync_ShouldShareSameCache()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_fakeStatuses);

        var cachedRepository = new CachedOrderStatusRepository(_mockRepository.Object);

        // Act
        await cachedRepository.GetByNameAsync("Spedito"); // Popola la cache
        var allStatuses = await cachedRepository.GetAllAsync(); // Deve usare la cache gia pronta

        // Assert
        Assert.Equal(3, allStatuses.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}