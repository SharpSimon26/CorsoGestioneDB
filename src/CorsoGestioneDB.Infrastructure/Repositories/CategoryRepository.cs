using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class CategoryRepository : AbstractRepository, ICategoryRepository
{
    public CategoryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public virtual async Task<IEnumerable<Category>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Categories order by CategoryName";
        var categories = await db.QueryAsync<Category>(sql);

        return categories;
    }

    public virtual async Task<Category?> GetByNameAsync(string categoryName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from Categories where CategoryName = @categoryName";
        var category = await db.QueryFirstOrDefaultAsync<Category>(sql, new { categoryName });

        return category;
    }
}
