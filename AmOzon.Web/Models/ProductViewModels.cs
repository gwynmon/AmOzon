using AmOzon.Contracts.Responses;
using System.ComponentModel.DataAnnotations;

namespace AmOzon.Web.Models;

public class ProductEditViewModel
{
    public Guid? Id { get; set; }

    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required, MinLength(4)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [Range(0, 1_000_000)]
    public int StockQuantity { get; set; }
}

public class ProductDetailsPageViewModel
{
    public required ProductsResponse Product { get; set; }
    public bool CanManageProduct { get; set; }
}