using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using AmOzon.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [AllowAnonymous] // ← Обязательно! Иначе бесконечный редирект
    public IActionResult Login()
    {
        return View();
    }

// POST: /Account/Login — обрабатываем вход
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var client = _httpClientFactory.CreateClient("Api");
        var response = await client.PostAsJsonAsync("/api/auth/login", model);
    
        if (response.IsSuccessStatusCode)
        {
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
        
            if (authResult?.Token != null)
            {
                // Просто сохраняем токен в куку — как ты и делал
                Response.Cookies.Append("jwt_token", authResult.Token, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true,
                    Secure = false // Для локалки; в продакшене ставь true
                });
            
                return RedirectToAction("Profile"); // Успех → в профиль
            }
        }
    
        // Ошибка → показываем форму с сообщением
        ModelState.AddModelError("", "Неверный логин или пароль");
        return View(model);
    }
    
    public async Task<IActionResult> Profile()
    {
        var client = _httpClientFactory.CreateClient("Api");
        
        var token = Request.Cookies["jwt_token"];
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        var profile = await client.GetFromJsonAsync<UserResponse>("/api/users/me");
        
        if (profile == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = profile.Adapt<UserProfileViewModel>();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt_token");
        return RedirectToAction("Index", "Home");
    }

    // Остальные методы (Orders, Settings, EditProfile) можно добавить позже
}