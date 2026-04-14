using AmOzon.Contracts.Responses;

namespace AmOzon.Web.Models;

public class SellerDashboardViewModel
{
    public required SellerResponse Seller { get; set; }
    public required List<ProductsResponse> Products { get; set; }
}