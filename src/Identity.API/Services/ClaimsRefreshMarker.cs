namespace eShop.Identity.API.Services;

internal static class ClaimsRefreshMarker
{
    public const string ClaimType = "claims_refresh_marker";
    public const string LoginProvider = "ClaimsRefresh";
    public const string TokenName = "Version";
}
