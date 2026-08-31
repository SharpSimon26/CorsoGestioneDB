using CorsoGestioneDB.Abstractions.Interfaces;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Application.Models;

namespace CorsoGestioneDB.Application.Pipeline.Rules;

public class ResolveCategoryIdRule : IResolutionRule
{
    private readonly ICachedCategoryRepository _cachedCategoryRepository;

    /// <summary>
    /// Regola di risoluzione applicata a CategoryID
    /// </summary>
    public ResolveCategoryIdRule(ICachedCategoryRepository cachedCategoryRepository)
    {
        _cachedCategoryRepository = cachedCategoryRepository;
    }

    public bool CanApply(ImportContext context)
    {
        var product = context.Data.Product;

        return !string.IsNullOrWhiteSpace(product.CategoryName) && product.CategoryID == null;
    }

    public async Task ApplyAsync(ImportContext context)
    {
        var product = context.Data.Product;

        // Recupera la categoria dal database
        var category = await _cachedCategoryRepository.GetByNameAsync(product.CategoryName!);

        if (category != null)
        {
            context.AddModification(nameof(product.CategoryID), category.CategoryID, product.CategoryID, "Database lookup", Stage.RESOLVE);
            product.CategoryID = category.CategoryID;
        }
        else
        {
            context.AddIssue(nameof(product.CategoryID), $"Categoria '{product.ProductName}' non trovata.");
        }
    }
}
