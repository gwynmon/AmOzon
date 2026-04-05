using System.Security.Claims;

namespace AmOzon.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claim))
            {
                throw new UnauthorizedAccessException("User ID claim is missing in the token.");
            }

            if (!Guid.TryParse(claim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid User ID format in the token.");
            }

            return userId;
        }
    }
}