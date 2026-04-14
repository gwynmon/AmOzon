using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using AmOzon.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace AmOzon.Web.Controllers;

public class ProductsController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            var client = httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync("/api/products/get-all");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Products service is temporarily unavailable.";
                return View(new List<ProductsResponse>());
            }

            var products = await response.Content.ReadFromJsonAsync<List<ProductsResponse>>();
            return View(products ?? new List<ProductsResponse>());
        }
        catch
        {
            TempData["ErrorMessage"] = "Cannot connect to products service.";
            return View(new List<ProductsResponse>());
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync($"/api/products/get/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        var product = await response.Content.ReadFromJsonAsync<ProductsResponse>();
        if (product is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var model = new ProductDetailsPageViewModel
        {
            Product = product,
            CanManageProduct = await IsCurrentUserSellerOwner(product.SellerId)
        };

        return View(model);
    }

    public async Task<IActionResult> MyProducts()
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token) || !TryGetUserIdFromToken(token, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedClient(token);
        var sellerId = await ResolveSellerIdByUser(client, userId);
        if (sellerId is null)
        {
            TempData["ErrorMessage"] = "You are not a seller yet.";
            return RedirectToAction("Profile", "Account");
        }

        var response = await client.GetAsync($"/api/products/get-by-seller/{sellerId}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Could not load seller products.";
            return RedirectToAction(nameof(Index));
        }

        var products = await response.Content.ReadFromJsonAsync<List<ProductsResponse>>();
        return View(products ?? new List<ProductsResponse>());
    }

    public IActionResult Create()
    {
        if (string.IsNullOrWhiteSpace(Request.Cookies["jwt_token"]))
        {
            return RedirectToAction("Login", "Account");
        }

        return View("Upsert", new ProductEditViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Upsert", model);
        }

        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token) || !TryGetUserIdFromToken(token, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedClient(token);
        var sellerId = await ResolveSellerIdByUser(client, userId);
        if (sellerId is null)
        {
            TempData["ErrorMessage"] = "Become a seller first.";
            return RedirectToAction("Create", "Seller");
        }

        var request = new ProductsCreateRequest(model.Name, model.Description, model.Price, model.StockQuantity, sellerId.Value);
        var response = await client.PostAsJsonAsync("/api/products/create", request);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to create product.");
            return View("Upsert", model);
        }

        return RedirectToAction(nameof(MyProducts));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.GetAsync($"/api/products/get/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(MyProducts));
        }

        var product = await response.Content.ReadFromJsonAsync<ProductsResponse>();
        if (product is null || !await IsCurrentUserSellerOwner(product.SellerId))
        {
            TempData["ErrorMessage"] = "You cannot edit this product.";
            return RedirectToAction(nameof(MyProducts));
        }

        var model = new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity
        };

        return View("Upsert", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditViewModel model)
    {
        if (!ModelState.IsValid || model.Id is null)
        {
            return View("Upsert", model);
        }

        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedClient(token);
        var request = new ProductsUpdateRequest(model.Name, model.Description, model.Price, model.StockQuantity, null);
        var response = await client.PutAsJsonAsync($"/api/products/update/{model.Id.Value}", request);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to update product.");
            return View("Upsert", model);
        }

        return RedirectToAction(nameof(MyProducts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleDeleted(Guid id, bool isDeleted)
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = CreateAuthorizedClient(token);
        var endpoint = isDeleted ? $"/api/products/revoke-deleted/{id}" : $"/api/products/mark-deleted/{id}";
        var response = await client.PutAsync(endpoint, null);
        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] = "Failed to change product status.";
        }

        return RedirectToAction(nameof(MyProducts));
    }

    private HttpClient CreateAuthorizedClient(string token)
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
        var claim = jwt.Claims.FirstOrDefault(c =>
            c.Type.EndsWith("/nameidentifier", StringComparison.OrdinalIgnoreCase) ||
            c.Type.Equals("nameid", StringComparison.OrdinalIgnoreCase));

        return claim is not null && Guid.TryParse(claim.Value, out userId);
    }

    private async Task<Guid?> ResolveSellerIdByUser(HttpClient client, Guid userId)
    {
        var sellersResponse = await client.GetAsync("/api/sellers/get-all");
        if (!sellersResponse.IsSuccessStatusCode)
        {
            return null;
        }

        var sellers = await sellersResponse.Content.ReadFromJsonAsync<List<SellerResponse>>();
        return sellers?.FirstOrDefault(x => x.UserId == userId)?.Id;
    }

    private async Task<bool> IsCurrentUserSellerOwner(Guid sellerId)
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token) || !TryGetUserIdFromToken(token, out var userId))
        {
            return false;
        }

        var client = CreateAuthorizedClient(token);
        var currentSellerId = await ResolveSellerIdByUser(client, userId);
        return currentSellerId == sellerId;
    }
}