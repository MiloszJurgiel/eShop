using eShop.Basket.API;
using eShop.Basket.API.Model;
using eShop.Basket.API.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace eShop.Basket.UnitTests;

[TestClass]
public class RedisBasketRepositoryTests
{
    [TestMethod]
    public async Task UpdateBasketAsync_SetsDefaultTtlOnCreate()
    {
        var database = Substitute.For<IDatabase>();
        database.StringSetAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue>(), Arg.Any<TimeSpan?>(), Arg.Any<bool>(), Arg.Any<When>(), Arg.Any<CommandFlags>())
            .Returns(true);

        var repository = CreateRepository(database, new BasketOptions());
        var basket = new CustomerBasket { BuyerId = "buyer-1" };

        await repository.UpdateBasketAsync(basket);

        await database.Received(1).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Is<TimeSpan?>(ttl => ttl == TimeSpan.FromDays(30)),
            false,
            When.Always,
            CommandFlags.None);
    }

    [TestMethod]
    public async Task UpdateBasketAsync_RefreshesTtlOnEveryUpdate()
    {
        var database = Substitute.For<IDatabase>();
        database.StringSetAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue>(), Arg.Any<TimeSpan?>(), Arg.Any<bool>(), Arg.Any<When>(), Arg.Any<CommandFlags>())
            .Returns(true);

        var repository = CreateRepository(database, new BasketOptions { RedisTtlDays = 14 });
        var basket = new CustomerBasket { BuyerId = "buyer-1" };

        await repository.UpdateBasketAsync(basket);
        await repository.UpdateBasketAsync(basket);

        await database.Received(2).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Is<TimeSpan?>(ttl => ttl == TimeSpan.FromDays(14)),
            false,
            When.Always,
            CommandFlags.None);
    }

    [TestMethod]
    public async Task UpdateBasketAsync_UsesTtlFromConfiguration()
    {
        var database = Substitute.For<IDatabase>();
        database.StringSetAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue>(), Arg.Any<TimeSpan?>(), Arg.Any<bool>(), Arg.Any<When>(), Arg.Any<CommandFlags>())
            .Returns(true);

        var repository = CreateRepository(database, new BasketOptions { RedisTtlDays = 5 });

        await repository.UpdateBasketAsync(new CustomerBasket { BuyerId = "buyer-1" });

        await database.Received(1).StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Is<TimeSpan?>(ttl => ttl == TimeSpan.FromDays(5)),
            false,
            When.Always,
            CommandFlags.None);
    }

    private static RedisBasketRepository CreateRepository(IDatabase database, BasketOptions options)
    {
        var multiplexer = Substitute.For<IConnectionMultiplexer>();
        multiplexer.GetDatabase(Arg.Any<int>(), Arg.Any<object>()).Returns(database);

        return new RedisBasketRepository(
            NullLogger<RedisBasketRepository>.Instance,
            multiplexer,
            Options.Create(options));
    }
}
