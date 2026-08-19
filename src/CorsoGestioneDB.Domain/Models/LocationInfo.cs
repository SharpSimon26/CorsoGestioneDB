namespace CorsoGestioneDB.Domain.Models;

public class LocationInfo
{
    public required int CityID { get; set; }
    public required string CityName { get; set; }
    public required int ProvinceID { get; set; }
    public required string ProvinceName { get; set; }
    public required int RegionID { get; set; }
    public required string RegionName { get; set; }
}