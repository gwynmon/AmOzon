using AmOzon.Contracts.Responses;
using AmOzon.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace AmOzon.Web.Controllers;

public class SellerController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index()
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync("/api/sellers/get-all");
        if (!response.IsSuccessStatusCode)
        {
            return View(new List<SellerResponse>());
        }

        var sellers = await response.Content.ReadFromJsonAsync<List<SellerResponse>>();
        return View(sellers ?? []);
    }

    public async Task<IActionResult> Dashboard()
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token) || !TryGetUserIdFromToken(token, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedApiClient(token);
        var sellersResponse = await client.GetAsync("/api/sellers/get-all");
        if (!sellersResponse.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not load seller data.";
            return RedirectToAction("Profile", "Account");
        }

        var sellers = await sellersResponse.Content.ReadFromJsonAsync<List<SellerResponse>>() ?? [];
        var seller = sellers.FirstOrDefault(x => x.UserId == userId);
        if (seller is null)
        {
            TempData["ErrorMessage"] = "You are not a seller yet.";
            return RedirectToAction("Profile", "Account");
        }

        var productsResponse = await client.GetAsync($"/api/products/get-by-seller/{seller.Id}");
        var products = productsResponse.IsSuccessStatusCode
            ? await productsResponse.Content.ReadFromJsonAsync<List<ProductsResponse>>() ?? []
            : [];

        var model = new SellerDashboardViewModel
        {
            Seller = seller,
            Products = products
        };

        return View(model);
    }

    public IActionResult Create()
    {
        if (string.IsNullOrWhiteSpace(Request.Cookies["jwt_token"]))
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSeller()
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedApiClient(token);
        var response = await client.PostAsync("/api/sellers/create", null);

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not create seller profile.";
            return RedirectToAction(nameof(Create));
        }

        var payload = await response.Content.ReadFromJsonAsync<SellerCreateResponse>();
        if (!string.IsNullOrWhiteSpace(payload?.newToken))
        {
            Response.Cookies.Append("jwt_token", payload.newToken, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true,
                Secure = false
            });
        }

        TempData["SuccessMessage"] = "Seller profile created.";
        return RedirectToAction(nameof(Dashboard));
    }

    private HttpClient CreateAuthorizedApiClient(string token)
    {
        var client = httpClientFactory.CreateClient("Api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private static bool TryGetUserIdFromToken(string token, out Guid userId)
    {
        userId = Guid.Empty;
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            return false;
        }

        var jwt = handler.ReadJwtToken(token);
        var userIdClaim = jwt.Claims.FirstOrDefault(c =>
            c.Type.EndsWith("/nameidentifier", StringComparison.OrdinalIgnoreCase) ||
            c.Type.Equals("nameid", StringComparison.OrdinalIgnoreCase));

        return userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out userId);
    }

    private sealed record SellerCreateResponse(string message, Guid sellerId, string newToken);
}
