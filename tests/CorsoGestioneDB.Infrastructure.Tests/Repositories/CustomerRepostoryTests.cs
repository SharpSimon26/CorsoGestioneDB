using System.Data;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using CorsoGestioneDB.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;

namespace CorsoGestioneDB.Infrastructure.Tests.Repositories;

public class CustomerRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // 1. Arrange
        var mockFactory = new Mock<IDbConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();

        var expectedCustomer = new Customer { CustomerID = 1, FirstName = "Mario", LastName = "Rossi" };

        // Imposta il mock per restituire la connessione mockata
        mockFactory.Setup(db => db.CreateConnection()).Returns(mockConnection.Object);

        // Imposta Moq.Dapper per intercettare la query
        mockConnection.SetupDapperAsync(conn => conn.QueryFirstOrDefaultAsync<Customer>(
                It.IsAny<string>(), It.IsAny<object>(), null, null, null
            ))
            .ReturnsAsync(expectedCustomer);

        var repository = new CustomerRepository(mockFactory.Object);

        // 2. Act
        var result = await repository.GetByIdAsync(1);

        // 3. Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CustomerID);
        Assert.Equal("Mario", result.FirstName);
        Assert.Equal("Rossi", result.LastName);
    }
}