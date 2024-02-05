using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories;

namespace OrderSolution.Infrastructure.Tests.Repositories;

public class CategoryRepository_Tests
{
    private CategoryRepository _categoryRepository;
    private EcDbFirstContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EcDbFirstContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EcDbFirstContext(options);
        _categoryRepository = new CategoryRepository(_context);

        // Add test data to in-memory database here if needed
    }

    [Test]
    public async Task GetAsync_ReturnsExpectedCategory()
    {
        // Arrange
        var expectedCategory = new Category { Id = Guid.NewGuid().ToString(), Name = "Test Category"};
        _context.Categories.Add(expectedCategory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.GetAsync(c => c.Id == expectedCategory.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(expectedCategory.Id));
    }
    
    [Test]
    public async Task GetAsync_ReturnsNullWhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = Guid.NewGuid().ToString();

        // Act
        var result = await _categoryRepository.GetAsync(c => c.Id == nonExistentCategoryId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
}