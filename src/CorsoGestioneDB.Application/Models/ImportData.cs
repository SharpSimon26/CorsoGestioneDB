namespace CorsoGestioneDB.Application.Models;

public class ImportData
{
    public ImportData()
    {
        Order = new();
        OrderLine = new();
        Customer = new();
        Product = new();
    }

    public ImportOrder Order { get; private set; }
    public ImportOrderLine OrderLine { get; private set; }
    public ImportCustomer Customer { get; private set; }
    public ImportProduct Product { get; private set; }
}