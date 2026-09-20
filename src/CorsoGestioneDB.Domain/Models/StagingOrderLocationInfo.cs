namespace CorsoGestioneDB.Domain.Models;

public class StagingOrderLocationInfo
{
    public required string City { get; set; }
    public required string Province { get; set; }
    public required string Region { get; set; }
    public int NumOrders { get; set; }
    public decimal UsagePercentage { get; set; }
}