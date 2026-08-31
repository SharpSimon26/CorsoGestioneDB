using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Services;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ResolveProductCodeRule : IResolutionRule
{
    private readonly IProductCodeResolverService _productCodeResolverService;

    /// <summary>
    /// Regola di risoluzione applicata a ProductCode
    /// </summary>
    public ResolveProductCodeRule(IProductCodeResolverService productCodeResolverService)
    {
        _productCodeResolverService = productCodeResolverService;
    }

    public bool CanApply(ImportContext context)
    {
        var product = context.Data.Product;

        return !string.IsNullOrWhiteSpace(product.ProductName);
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var product = context.Data.Product;

        // Recupera il codice del prodotto dal database
        var productInfo = await _productCodeResolverService.ResolveProductCode(product.ProductName!);

        if (productInfo != null)
        {
            if (product.ProductCode != productInfo.ProductCode)
            {
                context.AddModification(nameof(product.ProductCode), productInfo.ProductCode, product.ProductCode, "ProductCode", Models.Stage.RESOLVE);
                product.ProductCode = productInfo.ProductCode;
            }
        }
        else
        {
            context.AddIssue("ProductCode", $"Codice Prodotto per '{product.ProductName}' non trovato");
        }
    }
}