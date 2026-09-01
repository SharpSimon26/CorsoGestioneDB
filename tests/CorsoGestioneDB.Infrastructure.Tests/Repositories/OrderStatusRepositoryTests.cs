using System.Data;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;

namespace CorsoGestioneDB.Infrastructure.Tests.Repositories;

public class OrderStatusRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_Returns_List_Of_OrderStatus()
    {
        // 1. Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedOrderStatuses = new List<OrderStatus>
        {
            new() { OrderStatusID = 1, OrderStatusName = "In lavorazione" },
            new() { OrderStatusID = 2, OrderStatusName = "Spedito" },
            new() { OrderStatusID = 3, OrderStatusName = "Consegnato" },
            new() { OrderStatusID = 4, OrderStatusName = "Reso" },
            new() { OrderStatusID = 5, OrderStatusName = "Annullato" }
        };

        // Imposta il mock per restituire la connessione mockata
        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Imposta Moq.Dapper per intercettare la query
        mockConnection.SetupDapperAsync(conn => conn.QueryAsync<OrderStatus>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedOrderStatuses);

        var repository = new OrderStatusRepository(mockFactory.Object);

        // 2. Act
        var result = await repository.GetAllAsync();

        // 3. Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(5, result.Count());

        // Controlla che nessun elemento sia null e che gli ID siano validi
        Assert.All(result, item => 
        {
            Assert.NotNull(item);
            Assert.True(item.OrderStatusID > 0);
        });

        // Verifica elementi specifici
        Assert.Equal("In lavorazione", result.First().OrderStatusName);
        Assert.Equal("Annullato", result.Last().OrderStatusName);

        // Verifica che la connessione al DB sia stata invocata esattamente 1 volta
        mockFactory.Verify(db => db.CreateConnection(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrderStatus_WhenIdExists()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedStatus = new OrderStatus { OrderStatusID = 1, OrderStatusName = "In lavorazione" };

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Mockiamo la risposta per una singola istanza
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<OrderStatus>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedStatus);

        var repository = new OrderStatusRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("In lavotazione");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.OrderStatusID);
        Assert.Equal("In lavorazione", result.OrderStatusName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNameDoesNotExist()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Dapper restituisce null se non trova nulla
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<OrderStatus>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync((OrderStatus?)null);

        var repository = new OrderStatusRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("Inesistente"); // Nome inesistente

        // Assert
        Assert.Null(result);
    }
}