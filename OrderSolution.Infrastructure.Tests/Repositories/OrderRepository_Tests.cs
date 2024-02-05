using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories;

namespace OrderSolution.Infrastructure.Tests.Repositories;

public class OrderRepository_Tests
{
    private OrderRepository _orderRepository;
    private CfContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CfContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CfContext(options);
        _orderRepository = new OrderRepository(_context);

        // Add test data to in-memory database here if needed
    }

    [Test]
    public async Task GetAsync_ReturnsNullWhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentOrderId = Guid.NewGuid();

        // Act
        var result = await _orderRepository.GetAsync(o => o.Id == nonExistentOrderId);

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetAsync_ReturnsOrderWhenOrderExists()
    {
        // Arrange
        var existingOrder = new OrderEntity
        {
            OrderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity
                {
                    ArticleNumber = "123456",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };
        _context.Orders.Add(existingOrder);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderRepository.GetAsync(o => o.Id == existingOrder.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(existingOrder.Id));
            Assert.That(result?.OrderItems.Count, Is.EqualTo(1));
            Assert.That(result?.OrderItems.First().ArticleNumber, Is.EqualTo("123456"));
        });
    }

    [Test]
    public async Task GetOrdersByCustomerIdAsync_ReturnsEmptyListWhenCustomerHasNoOrders()
    {
        // Arrange
        var customerIdWithNoOrders = Guid.NewGuid();

        // Act
        var result = await _orderRepository.GetOrdersByCustomerIdAsync(customerIdWithNoOrders);

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public async Task GetOrdersByCustomerIdAsync_ReturnsOrdersWhenCustomerHasOrders()
    {
        // Arrange
        var existingCustomerId = Guid.NewGuid();
        var existingOrder1 = new OrderEntity { CustomerId = existingCustomerId };
        var existingOrder2 = new OrderEntity { CustomerId = existingCustomerId };
        _context.Orders.AddRange(existingOrder1, existingOrder2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderRepository.GetOrdersByCustomerIdAsync(existingCustomerId);
        var orderEntities = result.ToList();

        // Assert
        Assert.That(orderEntities, Is.Not.Null);
        Assert.That(orderEntities, Has.Count.EqualTo(2));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
}