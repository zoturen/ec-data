using System.Linq.Expressions;
using Moq;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;

namespace OrderSolution.Infrastructure.Tests.Services;

[TestFixture]
public class CustomerService_Tests
{
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private CustomerService _customerService;

    [SetUp]
    public void SetUp()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_mockCustomerRepository.Object);
    }
    
    [Test]
    public async Task CreateAsync_CreatesNewCustomer()
    {
        // Arrange
        var newCustomerDto = new CustomerCreateDto
        {
            FirstName = "Test Customer",
            LastName = "Test Last Name",
            Email = "test@me.com",
            PhoneNumber = "1234567890",
            Street = "Test Street",
            City = "Test City",
            ZipCode = "12345",
            Country = "Test Country"
        };
        var newCustomerEntity = newCustomerDto.ToEntity(Guid.NewGuid());
        _mockCustomerRepository.Setup(repo => repo.AddAsync(It.IsAny<CustomerEntity>())).ReturnsAsync(newCustomerEntity);

        // Act
        var result = await _customerService.CreateAsync(newCustomerDto);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result.FirstName, Is.EqualTo(newCustomerDto.FirstName));
        _mockCustomerRepository.Verify(repo => repo.AddAsync(It.IsAny<CustomerEntity>()), Times.Once);
    }

    [Test]
    public async Task GetAsync_ReturnsExpectedCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var expectedCustomer = new CustomerEntity {
            Id = customerId, 
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test Customer",
                LastName = "Test Last Name",
                Email = "test@me.com",
                PhoneNumber = "1234567890"
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test Street",
                City = "Test City",
                ZipCode = "12345",
                Country = "Test Country"
            }
        };
        _mockCustomerRepository.Setup(repo => repo.GetAsync(c => c.Id == customerId)).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _customerService.GetAsync(expectedCustomer.Id);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result.FirstName, Is.Not.Null, "The CustomerDetail should not be null.");
        Assert.That(result.FirstName, Is.EqualTo(expectedCustomer.CustomerDetail.FirstName));
        _mockCustomerRepository.Verify(repo => repo.GetAsync(c => c.Id == customerId), Times.Once);
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsAllCustomers()
    {
        // Arrange
        var expectedCustomers = new List<CustomerEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerDetail = new CustomerDetailEntity
                {
                    FirstName = "Test Customer 1",
                    LastName = "Test Last Name",
                    Email = "test1@me.com",
                    PhoneNumber = "1234567890"
                },
                CustomerAddress = new CustomerAddressEntity
                {
                    Street = "Test Street",
                    City = "Test City",
                    ZipCode = "12345",
                    Country = "Test Country"
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                CustomerDetail = new CustomerDetailEntity
                {
                    FirstName = "Test Customer 2",
                    LastName = "Test Last Name",
                    Email = "test2@me.com",
                    PhoneNumber = "1234567890"
                },
                CustomerAddress = new CustomerAddressEntity
                {
                    Street = "Test Street",
                    City = "Test City",
                    ZipCode = "12345",
                    Country = "Test Country"
                }
            }
        };
        _mockCustomerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCustomers);

        // Act
        var result = await _customerService.GetAllAsync();
        var customerDtos = result.ToList();

        // Assert
        Assert.That(customerDtos, Is.Not.Null, "The result should not be null.");
        Assert.That(customerDtos, Has.Count.EqualTo(expectedCustomers.Count));
        _mockCustomerRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
    
    [Test]
    public async Task UpdateAsync_UpdatesExistingCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new CustomerEntity
        {
            Id = customerId,
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test Customer 2",
                LastName = "Test Last Name",
                Email = "test2@me.com",
                PhoneNumber = "1234567890"
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test Street",
                City = "Test City",
                ZipCode = "12345",
                Country = "Test Country"
            }
        };
        var updatedCustomerDto = new CustomerCreateDto
        {
            FirstName = "Updated Customer",
            LastName = "Updated Last Name",
            Email = "updated@me.com",
            PhoneNumber = "0987654321",
            Street = "Updated Street",
            City = "Updated City",
            ZipCode = "54321",
            Country = "Updated Country"
        };
        _mockCustomerRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expression<Func<CustomerEntity, bool>>>(), It.IsAny<CustomerEntity>())).ReturnsAsync(true);

        // Act
        var result = await _customerService.UpdateAsync(existingCustomer.Id, updatedCustomerDto);

        // Assert
        Assert.That(result, Is.True, "The result should be true.");
        _mockCustomerRepository.Verify(repo => repo.UpdateAsync(c => c.Id == customerId, It.IsAny<CustomerEntity>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteAsync_DeletesExistingCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new CustomerEntity
        {
            Id = customerId
        };
        
        _mockCustomerRepository.Setup(repo => repo.DeleteAsync(c => c.Id == customerId)).ReturnsAsync(true);

        // Act
        var result = await _customerService.DeleteAsync(existingCustomer.Id);

        // Assert
        Assert.That(result, Is.True, "The result should be true.");
        _mockCustomerRepository.Verify(repo => repo.DeleteAsync(c => c.Id == customerId), Times.Once);
    }
}