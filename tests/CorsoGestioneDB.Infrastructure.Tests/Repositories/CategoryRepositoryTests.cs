using System.Data;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;

namespace CorsoGestioneDB.Infrastructure.Tests.Repositories;

public class CategoryRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturn_ListOfCategory()
    {
        // 1. Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedCategories = new List<Category>
        {
            new() { CategoryID = 1, CategoryName = "Accessori" },
            new() { CategoryID = 2, CategoryName = "Arredo" },
            new() { CategoryID = 3, CategoryName = "Audio" },
            new() { CategoryID = 4, CategoryName = "Informatica" },
            new() { CategoryID = 5, CategoryName = "Rete" },
            new() { CategoryID = 6, CategoryName = "Ufficio" }
        };

        // Imposta il mock per restituire la connessione mockata
        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Imposta Moq.Dapper per intercettare la query
        mockConnection.SetupDapperAsync(conn => conn.QueryAsync<Category>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedCategories);

        var repository = new CategoryRepository(mockFactory.Object);

        // 2. Act
        var result = await repository.GetAllAsync();

        // 3. Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(6, result.Count());

        // Controlla che nessun elemento sia null e che gli ID siano validi
        Assert.All(result, item => 
        {
            Assert.NotNull(item);
            Assert.True(item.CategoryID > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.CategoryName));
        });

        // Verifica elementi specifici
        Assert.Equal(1, result.First().CategoryID);
        Assert.Equal("Accessori", result.First().CategoryName);
        Assert.Equal(6, result.Last().CategoryID);
        Assert.Equal("Ufficio", result.Last().CategoryName);

        // Verifica che la connessione al DB sia stata invocata esattamente 1 volta
        mockFactory.Verify(db => db.CreateConnection(), Times.Once);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCategory_WhenNameExists()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedCategory = new Category { CategoryID = 1, CategoryName = "Accessori" };

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Mockiamo la risposta per una singola istanza
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<Category>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedCategory);

        var repository = new CategoryRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("Accessori");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CategoryID);
        Assert.Equal("Accessori", result.CategoryName);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNameDoesNotExist()
    {
        // Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Dapper restituisce null se non trova nulla
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<Category>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync((Category?)null);

        var repository = new CategoryRepository(mockFactory.Object);

        // Act
        var result = await repository.GetByNameAsync("Inesistente"); // Nome inesistente

        // Assert
        Assert.Null(result);
    }
}