namespace eShop.Identity.API.Services;

public class ApplicationUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var marker = await UserManager.GetAuthenticationTokenAsync(user, ClaimsRefreshMarker.LoginProvider, ClaimsRefreshMarker.TokenName);

        if (!string.IsNullOrWhiteSpace(marker))
        {
            identity.AddClaim(new Claim(ClaimsRefreshMarker.ClaimType, marker));
        }

        return identity;
    }
}
