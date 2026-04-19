using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using AmOzon.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace AmOzon.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("Api");
        var request = new LoginRequest(model.Email, model.Password);
        var response = await client.PostAsJsonAsync("/api/auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (string.IsNullOrWhiteSpace(authResult?.Token))
        {
            ModelState.AddModelError(string.Empty, "Authentication failed.");
            return View(model);
        }

        Response.Cookies.Append("jwt_token", authResult.Token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            HttpOnly = true,
            Secure = false
        });

        return RedirectToAction(nameof(Profile));
    }

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(RegisterViewModel.ConfirmPassword), "Passwords do not match.");
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("Api");
        var request = new UserRequest(model.Name, model.Age, model.Email, model.Password);
        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Could not create account.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Account created. Please sign in.";
        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> Profile()
    {
        var token = Request.Cookies["jwt_token"];
        if (string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction(nameof(Login));
        }

        var client = CreateAuthorizedClient(token);
        var profileResponse = await client.GetAsync("/api/users/me");
        if (!profileResponse.IsSuccessStatusCode)
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction(nameof(Login));
        }

        var profile = await profileResponse.Content.ReadFromJsonAsync<UserResponse>();
        if (profile is null)
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction(nameof(Login));
        }

        var model = new UserProfileViewModel
        {
            Id = profile.Id,
            Name = profile.Name,
            Age = profile.Age,
            Email = profile.Email,
            SellerId = profile.SellerId
        };

        var cartResponse = await client.GetAsync("/api/cart/get-items");
        if (cartResponse.IsSuccessStatusCode)
        {
            var cartItems = await cartResponse.Content.ReadFromJsonAsync<List<CartItemResponse>>();
            model.CartItemsCount = cartItems?.Count ?? 0;
        }

        if (TryGetUserIdFromToken(token, out var userId))
        {
            var productsResponse = await client.GetAsync("/api/products/get-all");
            if (productsResponse.IsSuccessStatusCode)
            {
                var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductsResponse>>();
                model.ProductsCount = products?.Count(x => x.SellerId == userId) ?? 0;
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Orders() => RedirectToAction("Index", "Products");
    public IActionResult Settings() => RedirectToAction(nameof(Profile));
    public IActionResult EditProfile() => RedirectToAction(nameof(Profile));

    private HttpClient CreateAuthorizedClient(string token)
    {
        var client = _httpClientFactory.CreateClient("Api");
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
}