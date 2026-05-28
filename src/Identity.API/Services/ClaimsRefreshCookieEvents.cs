namespace eShop.Identity.API.Services;

public class ClaimsRefreshCookieEvents(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : CookieAuthenticationEvents
{
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var principal = context.Principal;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var userId = principal.FindFirstValue(JwtClaimTypes.Subject) ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return;
        }

        var expectedMarker = await userManager.GetAuthenticationTokenAsync(user, ClaimsRefreshMarker.LoginProvider, ClaimsRefreshMarker.TokenName);
        var currentMarker = principal.FindFirstValue(ClaimsRefreshMarker.ClaimType);

        if (string.IsNullOrWhiteSpace(expectedMarker) ||
            string.Equals(expectedMarker, currentMarker, StringComparison.Ordinal))
        {
            return;
        }

        context.ReplacePrincipal(await signInManager.CreateUserPrincipalAsync(user));
        context.ShouldRenew = true;
    }
}
