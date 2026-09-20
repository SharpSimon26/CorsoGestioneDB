using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;
using CorsoGestioneDB.Application.Services;

namespace CorsoGestioneDB.Application.Pipeline.Rules.Reconstruction;

public class ReconstructLocationRule : IReconstructionRule
{
    private readonly ILocationReconstructorService _reconstructorService;

    public ReconstructLocationRule(ILocationReconstructorService reconstructorService)
    {
        _reconstructorService = reconstructorService;
    }

    public async Task<bool> CanApplyAsync(ImportContext context)
    {
        var customer = context.Data.Customer;

        if (!string.IsNullOrWhiteSpace(customer.City))
        {
            var locationInfo = await _reconstructorService.ReconstructLocation(customer.City);

            if (locationInfo != null)
            {
                var canApply = (customer.Province != locationInfo.Province || customer.Region != locationInfo.Region) 
                    && locationInfo.UsagePercentage > 80m;

                return canApply;
            }
        }
        
        return false;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var customer = context.Data.Customer;

        var locationInfo = await _reconstructorService.ReconstructLocation(customer.City!);

        if (customer.Province != locationInfo!.Province)
        {
            // Traccia della modifica
            context.AddModification(nameof(customer.Province), locationInfo.Province, customer.Province, GetType().Name, Stage.RECONSTRUCT);

            customer.Province = locationInfo.Province;
        }

        if (customer.Region != locationInfo.Region)
        {
            // Traccia della modifica
            context.AddModification(nameof(customer.Region), locationInfo.Region, customer.Region, GetType().Name, Stage.RECONSTRUCT);

            customer.Region = locationInfo.Region;
        }
    }
}