using AmOzon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Web.Controllers;

public class ProductsController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index()
    {
        var client = httpClientFactory.CreateClient("Api");
        var products = await client.GetFromJsonAsync<List<ProductsResponse>>("/api/products/get-all");
        return View(products ?? new List<ProductsResponse>());
    }
}