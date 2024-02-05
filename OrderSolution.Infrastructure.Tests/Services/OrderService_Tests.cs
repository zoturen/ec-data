using Moq;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Tests.Services;

[TestFixture]
public class OrderService_Tests
{
    private Mock<IOrderRepository> _mockOrderRepository;
    private IOrderService _orderService;
    private Mock<IProductService> _mockProductService;

    [SetUp]
    public void SetUp()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockProductService = new Mock<IProductService>();
        _orderService = new OrderService(_mockOrderRepository.Object, _mockProductService.Object);
    }
    
    [Test]
    public async Task CreateOrderAsync_CreatesNewOrder()
    {
        // Arrange
        var orderCreateDto = new OrderCreateDto
        {
            CustomerId = Guid.NewGuid(),
            OrderItems = [new OrderItemCreateDto("AN1", 2)]
        };
        var expectedOrder = new OrderEntity
        {
            CustomerId = orderCreateDto.CustomerId,
            CreatedAt = DateTime.UtcNow,
            PaidAt = default,
            TotalAmount = 200,
            OrderItems = new List<OrderItemEntity>
            {
                new()
                {
                    ArticleNumber = "AN1",
                    Quantity = 2,
                    UnitPrice = 100,
                    TotalAmount = 200
                }
            }
        };
        
        var productDto = new ProductDto("AN1", "Product 1", "Description 1", 100, "category1", 10, new List<ImageDto>(), new ProductDetailDto("Detail 1", "Detail 2"));
        
        _mockOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<OrderEntity>())).ReturnsAsync(expectedOrder);
        _mockProductService.Setup(service => service.GetProductByArticleNumberAsync(It.IsAny<string>())).ReturnsAsync(productDto);
        _mockProductService.Setup(service => service.UpdateProductStockAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _orderService.CreateOrderAsync(orderCreateDto);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.Multiple(() =>
        {
            Assert.That(result.CustomerId, Is.EqualTo(expectedOrder.CustomerId));
            Assert.That(result.TotalAmount, Is.EqualTo(expectedOrder.TotalAmount));
        });
        _mockOrderRepository.Verify(repo => repo.AddAsync(It.IsAny<OrderEntity>()), Times.Once);
        _mockProductService.Verify(service => service.GetProductByArticleNumberAsync(It.IsAny<string>()), Times.Exactly(orderCreateDto.OrderItems.Count));
        _mockProductService.Verify(service => service.UpdateProductStockAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(orderCreateDto.OrderItems.Count));
    }
    
    [Test]
    public async Task GetOrderAsync_ReturnsExpectedOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var expectedOrder = new OrderEntity
        {
            CustomerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            PaidAt = default,
            TotalAmount = 200,
            OrderItems = new List<OrderItemEntity>
            {
                new()
                {
                    ArticleNumber = "AN1",
                    Quantity = 2,
                    UnitPrice = 100,
                    TotalAmount = 200
                }
            }
        };
        var productDto = new ProductDto("AN1", "Product 1", "Description 1", 100, "category1", 10, new List<ImageDto>(), new ProductDetailDto("Detail 1", "Detail 2"));
        _mockOrderRepository.Setup(repo => repo.GetAsync(x => x.Id == orderId)).ReturnsAsync(expectedOrder);
        _mockProductService.Setup(service => service.GetProductByArticleNumberAsync(It.IsAny<string>())).ReturnsAsync(productDto);

        // Act
        var result = await _orderService.GetOrderAsync(orderId);
        var orderItem = result?.OrderItems.FirstOrDefault();
        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(expectedOrder.Id));
            Assert.That(orderItem?.Name, Is.EqualTo(productDto.Name));
        });
        _mockOrderRepository.Verify(repo => repo.GetAsync(x => x.Id == orderId), Times.Once);
        _mockProductService.Verify(service => service.GetProductByArticleNumberAsync(It.IsAny<string>()), Times.Exactly(expectedOrder.OrderItems.Count));
    }

    [Test]
    public async Task GetOrdersAsync_ReturnsExpectedOrders()
    {
        // Arrange
        var expectedOrders = new List<OrderEntity>
        {
            new()
            {
                CustomerId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                PaidAt = default,
                TotalAmount = 200,
                OrderItems = new List<OrderItemEntity>
                {
                    new()
                    {
                        ArticleNumber = "AN1",
                        Quantity = 2,
                        UnitPrice = 100,
                        TotalAmount = 200
                    }
                }
            },
            new()
            {
                CustomerId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                PaidAt = default,
                TotalAmount = 200,
                OrderItems = new List<OrderItemEntity>
                {
                    new()
                    {
                        ArticleNumber = "AN1",
                        Quantity = 2,
                        UnitPrice = 100,
                        TotalAmount = 200
                    }
                }
            }
        };
        _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedOrders);

        // Act
        var result = await _orderService.GetOrdersAsync();
        var orderDtos = result.ToList();

        // Assert
        Assert.That(orderDtos, Is.Not.Null, "The result should not be null.");
        Assert.That(orderDtos, Has.Count.EqualTo(expectedOrders.Count));
        _mockOrderRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
    
    [Test]
    public async Task GetOrdersAsync_ReturnsExpectedOrdersForCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var expectedOrders = new List<OrderEntity>
        {
            new()
            {
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                PaidAt = default,
                TotalAmount = 200,
                OrderItems = new List<OrderItemEntity>
                {
                    new()
                    {
                        ArticleNumber = "AN1",
                        Quantity = 2,
                        UnitPrice = 100,
                        TotalAmount = 200
                    }
                }
            },
            new()
            {
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                PaidAt = default,
                TotalAmount = 200,
                OrderItems = new List<OrderItemEntity>
                {
                    new()
                    {
                        ArticleNumber = "AN1",
                        Quantity = 2,
                        UnitPrice = 100,
                        TotalAmount = 200
                    }
                }
            }
        };
        _mockOrderRepository.Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId)).ReturnsAsync(expectedOrders);

        // Act
        var result = await _orderService.GetOrdersAsync(customerId);
        var orderDtos = result.ToList();

        // Assert
        Assert.That(orderDtos, Is.Not.Null, "The result should not be null.");
        Assert.That(orderDtos, Has.Count.EqualTo(expectedOrders.Count));
        _mockOrderRepository.Verify(repo => repo.GetOrdersByCustomerIdAsync(customerId), Times.Once);
    }
    
    [Test]
    public async Task PayOrderAsync_ReturnsTrue_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var existingOrder = new OrderEntity
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            PaidAt = default,
            TotalAmount = 200,
            OrderItems = new List<OrderItemEntity>
            {
                new()
                {
                    ArticleNumber = "AN1",
                    Quantity = 2,
                    UnitPrice = 100,
                    TotalAmount = 200
                }
            }
        };
        _mockOrderRepository.Setup(repo => repo.GetAsync(x => x.Id == orderId)).ReturnsAsync(existingOrder);
        _mockOrderRepository.Setup(repo => repo.UpdateAsync(x => x.Id == orderId, It.IsAny<OrderEntity>())).ReturnsAsync(true);

        // Act
        var result = await _orderService.PayOrderAsync(orderId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, "The result should be true.");
            Assert.That(existingOrder.PaidAt, Is.Not.EqualTo(default(DateTime)));
        });
        _mockOrderRepository.Verify(repo => repo.GetAsync(x => x.Id == orderId), Times.Once);
        _mockOrderRepository.Verify(repo => repo.UpdateAsync(x => x.Id == orderId, It.IsAny<OrderEntity>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteOrderAsync_ReturnsTrue_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var existingOrder = new OrderEntity
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            PaidAt = default,
            TotalAmount = 200,
            OrderItems = new List<OrderItemEntity>
            {
                new()
                {
                    ArticleNumber = "AN1",
                    Quantity = 2,
                    UnitPrice = 100,
                    TotalAmount = 200
                }
            }
        };
        _mockOrderRepository.Setup(repo => repo.GetAsync(x => x.Id == orderId)).ReturnsAsync(existingOrder);
        _mockOrderRepository.Setup(repo => repo.DeleteAsync(x => x.Id == orderId)).ReturnsAsync(true);

        // Act
        var result = await _orderService.DeleteOrderAsync(orderId);

        // Assert
        Assert.That(result, Is.True, "The result should be true.");
        _mockOrderRepository.Verify(repo => repo.GetAsync(x => x.Id == orderId), Times.Once);
        _mockOrderRepository.Verify(repo => repo.DeleteAsync(x => x.Id == orderId), Times.Once);
    }
}