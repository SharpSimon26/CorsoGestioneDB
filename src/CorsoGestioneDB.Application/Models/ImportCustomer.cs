namespace CorsoGestioneDB.Application.Models;

public class ImportCustomer
{
    public int? CustomerID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int? CityID { get; set; }
    public string? City { get; set; }
    public int? ProvinceID { get; set; }
    public string? Province { get; set; }
    public int? RegionID { get; set; }
    public string? Region { get; set; }
    public DateTime? SignupDate { get; set; }
}