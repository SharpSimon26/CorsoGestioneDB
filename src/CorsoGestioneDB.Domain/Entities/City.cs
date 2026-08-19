namespace CorsoGestioneDB.Domain.Entities;

public class City
{
    public int CityID { get; set; }
    public int ProvinceID { get; set; }
    public string CityName { get; set; } = string.Empty;
}
