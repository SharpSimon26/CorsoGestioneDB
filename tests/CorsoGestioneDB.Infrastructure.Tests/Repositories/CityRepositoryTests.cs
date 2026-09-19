using System.Data;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;

namespace CorsoGestioneDB.Infrastructure.Tests.Repositories;

public class CityRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturn_ListOfCity()
    {
        // Arrance
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var exptectedCities = new List<City>
        {
            new() { CityID = 1, CityName = "Trieste", ProvinceID = 1 },
            new() { CityID = 2, CityName = "Gorizia", ProvinceID = 1 },
            new() { CityID = 3, CityName = "Udine", ProvinceID = 1 },
            new() { CityID = 4, CityName = "Pordenone", ProvinceID = 1 },
        };

        // Imposta il mock per restituire la connessione mockata
        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);
    
        // Imposta Moq.Dapper per intercettare la query
        mockConnection.SetupDapperAsync(conn => conn.QueryAsync<City>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(exptectedCities);

        var repository = new CityRepository(mockFactory.Object);

        // Act
        var result = await repository.GetAllAsync();
    
        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(4, result.Count());

        // Controlla che nessun elemento sia null e che gli ID siano validi
        Assert.All(result, item => 
        {
            Assert.NotNull(item);
            Assert.True(item.CityID > 0);
            Assert.True(item.ProvinceID > 0);
            Assert.True(item.CityName.Length > 0);
            Assert.False(string.IsNullOrWhiteSpace(item.CityName));
        });

        // Verifica elementi specifici
        Assert.Equal(1, result.First().CityID);
        Assert.Equal("Pordenone", result.Last().CityName);

        // Verifica che la connessione al DB sia stata invocata esattamente 1 volta
        mockFactory.Verify(db => db.CreateConnection(), Times.Once);
    }
}