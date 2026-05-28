namespace eShop.Identity.UnitTests;

[TestClass]
public class IdentityControllerTests
{
    private const string LoginProvider = "ClaimsRefresh";
    private const string TokenName = "Version";

    [TestMethod]
    public async Task RefreshClaimsReturnsNoContentForAdmin()
    {
        var user = new ApplicationUser { Id = "target-user" };
        var userManager = CreateUserManager();
        userManager.FindByIdAsync(user.Id).Returns(user);
        userManager
            .SetAuthenticationTokenAsync(user, LoginProvider, TokenName, Arg.Any<string>())
            .Returns(IdentityResult.Success);

        var controller = CreateController(userManager, isAdmin: true);

        var result = await controller.RefreshClaims(new RefreshClaimsRequest { UserId = user.Id });

        Assert.IsInstanceOfType<NoContentResult>(result);
        await userManager.Received(1).SetAuthenticationTokenAsync(
            user,
            LoginProvider,
            TokenName,
            Arg.Is<string>(value => !string.IsNullOrWhiteSpace(value)));
    }

    [TestMethod]
    public async Task RefreshClaimsReturnsForbidForNonAdmin()
    {
        var userManager = CreateUserManager();
        var controller = CreateController(userManager, isAdmin: false);

        var result = await controller.RefreshClaims(new RefreshClaimsRequest { UserId = "target-user" });

        Assert.IsInstanceOfType<ForbidResult>(result);
        userManager.DidNotReceiveWithAnyArgs().FindByIdAsync(default!);
    }

    [TestMethod]
    public async Task RefreshClaimsReturnsNotFoundForMissingUser()
    {
        var userManager = CreateUserManager();
        userManager.FindByIdAsync("missing-user").Returns((ApplicationUser?)null);
        var controller = CreateController(userManager, isAdmin: true);

        var result = await controller.RefreshClaims(new RefreshClaimsRequest { UserId = "missing-user" });

        Assert.IsInstanceOfType<NotFoundResult>(result);
        userManager.DidNotReceiveWithAnyArgs().SetAuthenticationTokenAsync(default!, default!, default!, default!);
    }

    private static IdentityController CreateController(UserManager<ApplicationUser> userManager, bool isAdmin)
    {
        return new IdentityController(userManager)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, "admin-user"),
                        .. (isAdmin ? [new Claim(ClaimTypes.Role, "Admin")] : [])
                    ], "Test"))
                }
            }
        };
    }

    private static UserManager<ApplicationUser> CreateUserManager()
    {
        return Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            Substitute.For<IPasswordHasher<ApplicationUser>>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            Substitute.For<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Substitute.For<IServiceProvider>(),
            Substitute.For<ILogger<UserManager<ApplicationUser>>>());
    }
}
