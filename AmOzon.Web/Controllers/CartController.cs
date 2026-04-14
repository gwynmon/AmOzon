using AmOzon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AmOzon.Web.Controllers;

public class CartController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index()
    {
        var client = CreateAuthorizedClientOrNull();
        if (client is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var response = await client.GetAsync("/api/cart/get-items");
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not load cart.";
            return View(new List<CartItemResponse>());
        }

        var items = await response.Content.ReadFromJsonAsync<List<CartItemResponse>>();
        return View(items ?? []);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Guid productId, int quantity = 1)
    {
        var client = CreateAuthorizedClientOrNull();
        if (client is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var response = await client.PostAsync($"/api/cart/add-item?productId={productId}&cartQuantity={Math.Max(1, quantity)}", null);
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not add product to cart.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(Guid itemId, int quantity)
    {
        var client = CreateAuthorizedClientOrNull();
        if (client is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var safeQuantity = Math.Max(1, quantity);
        var response = await client.PutAsync($"/api/cart/update-quantity/{itemId}/{safeQuantity}", null);
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not update quantity.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid itemId)
    {
        var client = CreateAuthorizedClientOrNull();
        if (client is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var response = await client.DeleteAsync($"/api/cart/delete-item/{itemId}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not delete cart item.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        var client = CreateAuthorizedClientOrNull();
        if (client is null)
        {
            return RedirectToAction("Login", "Account");docker compose down
        }

        await client.DeleteAsync("/api/cart/clear");
        return RedirectToAction(nameof(Index));
    }

    private HttpClient? CreateAuthorizedClientOrNull()
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var client = httpClientFactory.CreateClient("Api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
