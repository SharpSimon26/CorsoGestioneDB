using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Domain.Entities;
using CorsoGestioneDB.Infrastructure.Database;
using Dapper;
using System.Data;

namespace CorsoGestioneDB.Infrastructure.Repositories;

public class PaymentMethodRepository : AbstractRepository, IPaymentMethodRepository
{
    public PaymentMethodRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public virtual async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from PaymentMethods order by PaymentMethodName";
        var paymentMethods = await db.QueryAsync<PaymentMethod>(sql);

        return paymentMethods;
    }

    public virtual async Task<PaymentMethod?> GetByNameAsync(string paymentMethodName)
    {
        using IDbConnection db = connectionFactory.CreateConnection();
        var sql = "select * from PaymentMethods where PaymentMethodName = @paymentMethodName";
        var paymentMethod = await db.QueryFirstOrDefaultAsync<PaymentMethod>(sql, new { paymentMethodName });

        return paymentMethod;
    }

    public async Task<PaymentMethod?> GetBestMatchByNameAsync(string paymentMethodName, int minimumSimilarity = 75, int minimumGap = 10)
    {
        using IDbConnection db = connectionFactory.CreateConnection();

        var sql = """
            WITH Scored AS
            (
                SELECT
                    PaymentMethodID,
                    EDIT_DISTANCE_SIMILARITY(
                        @paymentMethodName,
                        PaymentMethodName
                    ) AS Similarity
                FROM dbo.PaymentMethods
            ),
            Ranked AS
            (
                SELECT
                    PaymentMethodID,
                    Similarity,
                    ROW_NUMBER() OVER (
                        ORDER BY Similarity DESC, PaymentMethodID
                    ) AS Position,
                    LEAD(Similarity) OVER (
                        ORDER BY Similarity DESC, PaymentMethodID
                    ) AS NextSimilarity
                FROM Scored
            )
            SELECT pm.*
                FROM Ranked r
                INNER JOIN dbo.PaymentMethods pm
                    ON pm.PaymentMethodID = r.PaymentMethodID
                WHERE r.Position = 1
                AND r.Similarity >= @minimumSimilarity
                AND (
                    r.NextSimilarity IS NULL
                    OR r.Similarity - r.NextSimilarity >= @minimumGap
                );
            """;

        return await db.QueryFirstOrDefaultAsync<PaymentMethod>(sql, new { paymentMethodName, minimumSimilarity, minimumGap });
    }
}
