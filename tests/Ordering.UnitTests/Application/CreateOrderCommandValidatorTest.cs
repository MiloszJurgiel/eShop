using eShop.Ordering.API.Application.Validations;
using FluentValidation;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class CreateOrderCommandValidatorTest
{
    private readonly CreateOrderCommandValidator _validator;

    public CreateOrderCommandValidatorTest()
    {
        var logger = Substitute.For<ILogger<CreateOrderCommandValidator>>();
        _validator = new CreateOrderCommandValidator(logger);
    }

    private static CreateOrderCommand BuildCommand(List<BasketItem> items)
    {
        return new CreateOrderCommand(
            items,
            userId: "user1",
            userName: "User One",
            city: "Seattle",
            street: "123 Main St",
            state: "WA",
            country: "US",
            zipcode: "98101",
            cardNumber: "1234567890123456",
            cardHolderName: "User One",
            cardExpiration: DateTime.UtcNow.AddYears(1),
            cardSecurityNumber: "123",
            cardTypeId: 1);
    }

    private static BasketItem CreateItem(int quantity) =>
        new BasketItem
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = 1,
            ProductName = "Product",
            UnitPrice = 9.99m,
            OldUnitPrice = 9.99m,
            Quantity = quantity,
            PictureUrl = "http://example.com/img.png"
        };

    [TestMethod]
    public void Validate_ValidOrder_ShouldPass()
    {
        var command = BuildCommand([CreateItem(2)]);
        var result = _validator.Validate(command);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_ItemWithZeroQuantity_ShouldFail()
    {
        var command = BuildCommand([CreateItem(0)]);
        var result = _validator.Validate(command);
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName.Contains("Units")));
    }

    [TestMethod]
    public void Validate_ItemWithNegativeQuantity_ShouldFail()
    {
        var command = BuildCommand([CreateItem(-5)]);
        var result = _validator.Validate(command);
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName.Contains("Units")));
    }

    [TestMethod]
    public void Validate_MixedValidAndInvalidQuantities_ShouldFail()
    {
        var command = BuildCommand([CreateItem(1), CreateItem(0), CreateItem(-3)]);
        var result = _validator.Validate(command);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual(2, result.Errors.Count(e => e.PropertyName.Contains("Units")));
    }
}
