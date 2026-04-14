namespace AmOzon.Web.Models;

public class UserProfileViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsSeller { get; set; }
    
    // Статистика (заглушки, пока не подключишь реальные эндпоинты)
    public int OrdersCount { get; set; }
    public int CartItemsCount { get; set; }
    public int ProductsCount { get; set; }
    
    // Последние заказы
    public List<OrderSummaryViewModel>? RecentOrders { get; set; }
}

public class OrderSummaryViewModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}