using Moq;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;

namespace OrderSolution.Infrastructure.Tests.Services;

[TestFixture]
public class CategoryService_Tests
{
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_mockCategoryRepository.Object);
    }

    [Test]
    public async Task GetCategoriesAsync_ReturnsExpectedCategories()
    {
        // Arrange
        var expectedCategories = new List<Category>
        {
            new Category { Id = Guid.NewGuid().ToString(), Name = "Category1" },
            new Category { Id = Guid.NewGuid().ToString(), Name = "Category2" }
        };
        _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCategories);

        // Act
        var result = await _categoryService.GetCategoriesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(expectedCategories.Count));
        _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
    
    [Test]
    public async Task GetCategoryAsync_ReturnsCategoryWhenCategoryExists()
    {
        // Arrange
        var existingCategoryId = Guid.NewGuid();
        var existingCategory = new Category { Id = existingCategoryId.ToString(), Name = "Existing Category" };
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == existingCategoryId.ToString())).ReturnsAsync(existingCategory);

        // Act
        var result = await _categoryService.GetCategoryAsync(existingCategoryId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Name, Is.EqualTo(existingCategory.Name));
        _mockCategoryRepository.Verify(repo => repo.GetAsync(c => c.Id == existingCategoryId.ToString()), Times.Once);
    }

    [Test]
    public async Task GetCategoryAsync_ReturnsNullWhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = Guid.NewGuid();
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == nonExistentCategoryId.ToString())).ReturnsAsync((Category)null);

        // Act
        var result = await _categoryService.GetCategoryAsync(nonExistentCategoryId);

        // Assert
        Assert.That(result, Is.Null);
        _mockCategoryRepository.Verify(repo => repo.GetAsync(c => c.Id == nonExistentCategoryId.ToString()), Times.Once);
    }
    
    [Test]
    public async Task CreateCategoryAsync_CreatesNewCategory()
    {
        // Arrange
        var newCategoryDto = new CategoryCreateDto { Name = "New Category" };
        var newCategory = new Category { Id = Guid.NewGuid().ToString(), Name = newCategoryDto.Name };
        _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(newCategory);

        // Act
        var result = await _categoryService.CreateCategoryAsync(newCategoryDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(newCategoryDto.Name));
        _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == newCategoryDto.Name)), Times.Once);
    }
    
    [Test]
    public async Task UpdateCategoryAsync_UpdatesCategoryWhenCategoryExists()
    {
        // Arrange
        var existingCategoryId = Guid.NewGuid();
        var existingCategory = new Category { Id = existingCategoryId.ToString(), Name = "Existing Category" };
        var updatedCategoryDto = new CategoryCreateDto { Name = "Updated Category" };
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == existingCategoryId.ToString())).ReturnsAsync(existingCategory);
        _mockCategoryRepository.Setup(repo => repo.UpdateAsync(c => c.Id == existingCategoryId.ToString(), It.IsAny<Category>())).ReturnsAsync(true);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(existingCategoryId, updatedCategoryDto);

        // Assert
        Assert.That(result, Is.True);
        _mockCategoryRepository.Verify(repo => repo.UpdateAsync(c => c.Id == existingCategoryId.ToString(), It.IsAny<Category>()), Times.Once);
    }

    [Test]
    public async Task UpdateCategoryAsync_ReturnsFalseWhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = Guid.NewGuid();
        var updatedCategoryDto = new CategoryCreateDto { Name = "Updated Category" };
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == nonExistentCategoryId.ToString())).ReturnsAsync((Category)null);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(nonExistentCategoryId, updatedCategoryDto);

        // Assert
        Assert.That(result, Is.False);
        _mockCategoryRepository.Verify(repo => repo.UpdateAsync(c => c.Id == nonExistentCategoryId.ToString(), It.IsAny<Category>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteCategoryAsync_DeletesCategoryWhenCategoryExists()
    {
        // Arrange
        var existingCategoryId = Guid.NewGuid();
        var existingCategory = new Category { Id = existingCategoryId.ToString(), Name = "Existing Category" };
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == existingCategoryId.ToString())).ReturnsAsync(existingCategory);
        _mockCategoryRepository.Setup(repo => repo.DeleteAsync(c => c.Id == existingCategoryId.ToString())).ReturnsAsync(true);

        // Act
        var result = await _categoryService.DeleteCategoryAsync(existingCategoryId);

        // Assert
        Assert.That(result, Is.True);
        _mockCategoryRepository.Verify(repo => repo.DeleteAsync(c => c.Id == existingCategoryId.ToString()), Times.Once);
    }

    [Test]
    public async Task DeleteCategoryAsync_ReturnsFalseWhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = Guid.NewGuid();
        _mockCategoryRepository.Setup(repo => repo.GetAsync(c => c.Id == nonExistentCategoryId.ToString())).ReturnsAsync((Category)null);

        // Act
        var result = await _categoryService.DeleteCategoryAsync(nonExistentCategoryId);

        // Assert
        Assert.That(result, Is.False);
        _mockCategoryRepository.Verify(repo => repo.DeleteAsync(c => c.Id == nonExistentCategoryId.ToString()), Times.Once);
    }
}