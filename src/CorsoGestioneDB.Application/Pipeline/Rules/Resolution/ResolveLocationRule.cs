using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Resolution;

public class ResolveLocationRule : IResolutionRule
{
    private readonly ICachedLocationInfoRepository _cachedLocationInfoRepository;

    /// <summary>
    /// Regola di risoluzione applicata a CityID
    /// </summary>
    public ResolveLocationRule(ICachedLocationInfoRepository cachedLocationInfoRepository)
    {
        _cachedLocationInfoRepository = cachedLocationInfoRepository;
    }

    public bool CanApply(ImportContext context)
    {
        var customer = context.Data.Customer;

        return !string.IsNullOrWhiteSpace(customer.City) && customer.CityID == null;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var customer = context.Data.Customer;

        // Recupera le informazioni dal database
        var location = await _cachedLocationInfoRepository.GetLocationInfoByCityNameAsync(customer.City!);

        if (location != null)
        {
            context.AddModification(nameof(customer.CityID), location.CityID, customer.CityID, "Database lookup", Stage.RESOLVE);
            customer.CityID = location.CityID;

            context.AddModification(nameof(customer.ProvinceID), location.ProvinceID, customer.ProvinceID, "Database lookup", Stage.RESOLVE);
            customer.ProvinceID = location.ProvinceID;

            context.AddModification(nameof(customer.RegionID), location.RegionID, customer.RegionID, "Database lookup", Stage.RESOLVE);
            customer.RegionID = location.RegionID;
        }
        else
        {
            context.AddIssue(nameof(customer.CityID), $"Città '{customer.City}' non trovata.");
        }
    }
}