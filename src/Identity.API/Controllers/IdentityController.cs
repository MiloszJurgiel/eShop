namespace eShop.Identity.API.Controllers;

[ApiController]
[Authorize]
[Route("api/identity")]
public class IdentityController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("refresh-claims")]
    public async Task<IActionResult> RefreshClaims([FromBody] RefreshClaimsRequest request)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.SetAuthenticationTokenAsync(
            user,
            ClaimsRefreshMarker.LoginProvider,
            ClaimsRefreshMarker.TokenName,
            Guid.NewGuid().ToString("N"));

        if (!result.Succeeded)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Failed to refresh claims.");
        }

        return NoContent();
    }
}
