using System.Data;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;

namespace CorsoGestioneDB.Infrastructure.Tests.Repositories;

public class SalesChannelRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_Returns_List_Of_SalesChannel()
    {
        // 1. Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedSalesChannels = new List<SalesChannel>
        {
            new() { SalesChannelID = 1, SalesChannelName = "E-commerce" },
            new() { SalesChannelID = 2, SalesChannelName = "Marketplace" },
            new() { SalesChannelID = 3, SalesChannelName = "Negozio" },
            new() { SalesChannelID = 4, SalesChannelName = "Telefono" },
        };
    
        // Imposta il mock per restituire la connessione mockata
        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Imposta Moq.Dapper per intercettare la query
        mockConnection.SetupDapperAsync(conn => conn.QueryAsync<SalesChannel>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedSalesChannels);

        var repository = new SalesChannelRepository(mockFactory.Object);

        // 2. Act
        var result = await repository.GetAllAsync();
    
        // 3. Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(4, result.Count());

        // Controlla che nessun elemento sia null e che gli ID siano validi
        Assert.All(result, item => 
        {
            Assert.NotNull(item);
            Assert.True(item.SalesChannelID > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.SalesChannelName));
        });

        // Verifica elementi specifici
        Assert.Equal("E-commerce", result.First().SalesChannelName);
        Assert.Equal("Telefono", result.Last().SalesChannelName);

        // Verifica che la connessione al DB sia stata invocata esattamente 1 volta
        mockFactory.Verify(db => db.CreateConnection(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn_SalesChannel_WhenIdExists()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedChannel = new SalesChannel { SalesChannelID = 4, SalesChannelName = "Telefono" };

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Mockiamo la risposta per una singola istanza
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<SalesChannel>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedChannel);

        var repository = new SalesChannelRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("Telefono");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.SalesChannelID);
        Assert.Equal("Telefono", result.SalesChannelName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn_Null_WhenNameDoesNotExist()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Dapper restituisce null se non trova nulla
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<SalesChannel>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync((SalesChannel?)null);

        var repository = new SalesChannelRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("Inesistente"); // Nome inesistente

        // Assert
        Assert.Null(result);
    }
}