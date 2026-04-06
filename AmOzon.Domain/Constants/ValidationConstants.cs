namespace AmOzon.Domain.Constants;

public static class ValidationConstants
{
    public const int MaxProductNameLength = 128;
    public const int MaxUserNameLength = 32;
    public const int MaxDescriptionLength = 256;
    public const int MaxEmailLength = 256;
    public const int MaxPasswordLength = 64;
    public const decimal MaxProductPrice = 999999999999999999.99m;
    public const int MaxStockQuantity = 1000;
}