namespace eShop.Identity.API.Models;

public class RefreshClaimsRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;
}
