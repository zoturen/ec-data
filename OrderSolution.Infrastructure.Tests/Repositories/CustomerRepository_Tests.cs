using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities;
using OrderSolution.Infrastructure.Repositories;

namespace OrderSolution.Infrastructure.Tests.Repositories;

public class CustomerRepository_Tests
{
    private CustomerRepository _customerRepository;
    private CfContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CfContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CfContext(options);
        _customerRepository = new CustomerRepository(_context);
        // setup a test customer
        
        
    }
    
    [Test]
    public async Task GetAsync_ReturnsCustomerWhenCustomerExists()
    {
        // Arrange
        var existingCustomer = new CustomerEntity
        {
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test name",
                LastName = "Test last name",
                Email = "test@me.com",
                PhoneNumber = "123456789",

            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test street",
                City = "Test city",
                ZipCode = "12345",
                Country = "Test country",
            }
        };
        _context.Customers.Add(existingCustomer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _customerRepository.GetAsync(c => c.Id == existingCustomer.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(existingCustomer.Id));
            Assert.That(result?.CustomerDetail.FirstName, Is.EqualTo(existingCustomer.CustomerDetail.FirstName));
            Assert.That(result?.CustomerAddress.City, Is.EqualTo(existingCustomer.CustomerAddress.City));
        });
    }

    [Test]
    public async Task GetAsync_ReturnsNullWhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentCustomerId = Guid.NewGuid();

        // Act
        var result = await _customerRepository.GetAsync(c => c.Id == nonExistentCustomerId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_ReturnsFalseWhenCustomerDoesNotExist()
    {
        // Arrange
        var nonExistentCustomerId = Guid.NewGuid();
        var updatedCustomer = new CustomerEntity { Id = nonExistentCustomerId};

        // Act
        var result = await _customerRepository.UpdateAsync(c => c.Id == nonExistentCustomerId, updatedCustomer);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task UpdateAsync_ReturnsTrueWhenCustomerExists()
    {
        // Arrange
        var existingCustomer = new CustomerEntity
        {
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test name",
                LastName = "Test last name",
                Email = "test@me.com",
                PhoneNumber = "123456789",
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test street",
                City = "Test city",
                ZipCode = "12345",
                Country = "Test country",
            }
        };
        _context.Customers.Add(existingCustomer);
        await _context.SaveChangesAsync();

        var updatedCustomer = new CustomerEntity
        {
            Id = existingCustomer.Id,
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Updated name",
                LastName = "Updated last name",
                Email = "updated@me.com",
                PhoneNumber = "987654321",
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Updated street",
                City = "Updated city",
                ZipCode = "54321",
                Country = "Updated country",
            }
        };

        // Act
        var result = await _customerRepository.UpdateAsync(c => c.Id == existingCustomer.Id, updatedCustomer);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyListWhenDatabaseIsEmpty()
    {
        // Act
        var result = await _customerRepository.GetAllAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsAllCustomersWhenDatabaseIsNotEmpty()
    {
        // Arrange
        var existingCustomer1 = new CustomerEntity
        {
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test name 1",
                LastName = "Test last name 1",
                Email = "test1@me.com",
                PhoneNumber = "123456789",
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test street 1",
                City = "Test city 1",
                ZipCode = "12345",
                Country = "Test country 1",
            }
        };
        _context.Customers.Add(existingCustomer1);

        var existingCustomer2 = new CustomerEntity
        {
            CustomerDetail = new CustomerDetailEntity
            {
                FirstName = "Test name 2",
                LastName = "Test last name 2",
                Email = "test2@me.com",
                PhoneNumber = "987654321",
            },
            CustomerAddress = new CustomerAddressEntity
            {
                Street = "Test street 2",
                City = "Test city 2",
                ZipCode = "54321",
                Country = "Test country 2",
            }
        };
        _context.Customers.Add(existingCustomer2);

        await _context.SaveChangesAsync();

        // Act
        var result = await _customerRepository.GetAllAsync();
        var customerEntities = result.ToList();

        // Assert
        Assert.That(customerEntities, Is.Not.Null);
        Assert.That(customerEntities, Has.Count.EqualTo(2));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
}