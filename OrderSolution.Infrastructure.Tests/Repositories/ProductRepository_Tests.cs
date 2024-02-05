using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories;

namespace OrderSolution.Infrastructure.Tests.Repositories;

public class ProductRepository_Tests
{
    private ProductRepository _productRepository;
    private CategoryRepository _categoryRepository;
    private EcDbFirstContext _context;
    private string _categoryId;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EcDbFirstContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EcDbFirstContext(options);
        _productRepository = new ProductRepository(_context);
        _categoryRepository = new CategoryRepository(_context);
        
        _categoryId = Guid.NewGuid().ToString();
        _context.Categories.Add(new Category
        {
            Id = _categoryId,
            Name = "Test Category"
        });
        _context.SaveChanges();
        
    }

    [Test]
    public async Task GetAsync_ReturnsNullWhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentArticleNumber = "12345678";

        // Act
        var result = await _productRepository.GetAsync(p => p.Articlenumber == nonExistentArticleNumber);

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetAsync_ReturnsProductWhenProductExists()
    {
        // Arrange
        var existingProduct = new Product
        {
            Articlenumber = "12345678",
            Name = "Test Product",
            Description = "Test Description",
            Categoryid = _categoryId,
            Price = 0,
            Stock = 0,
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M"
            },
            Images = new List<Image>
            {
                new Image
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = "https://example.com/image.jpg"
                }
            }
        };
        _context.Products.Add(existingProduct);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAsync(p => p.Articlenumber == existingProduct.Articlenumber);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result?.Articlenumber, Is.EqualTo(existingProduct.Articlenumber));
            Assert.That(result?.Name, Is.EqualTo(existingProduct.Name));
            Assert.That(result?.Productdetail?.Size, Is.EqualTo(existingProduct.Productdetail.Size));
            Assert.That(result?.Productdetail?.Color, Is.EqualTo(existingProduct.Productdetail.Color));
            Assert.That(result?.Images.Count, Is.EqualTo(existingProduct.Images.Count));
        });
    }
    
    [Test]
    public async Task UpdateAsync_ReturnsTrueWhenProductExists()
    {
        // Arrange
        var existingProduct = new Product
        {
            Articlenumber = "12345678",
            Name = "Test Product",
            Description = "Test Description",
            Categoryid = _categoryId,
            Price = 0,
            Stock = 0,
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M"
            }
        };
        _context.Products.Add(existingProduct);
        await _context.SaveChangesAsync();

        var updatedProduct = new Product
        {
            Articlenumber = existingProduct.Articlenumber,
            Name = "Updated Product",
            Productdetail = new Productdetail
            {
                Color = "Blue",
                Size = "M"
            }
        };

        // Act
        var result = await _productRepository.UpdateAsync(p => p.Articlenumber == existingProduct.Articlenumber, updatedProduct);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task UpdateAsync_ReturnsFalseWhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentProduct = new Product
        {
            Articlenumber = "12345678",
        };

        var updatedProduct = new Product
        {
            Articlenumber = nonExistentProduct.Articlenumber,
        };

        // Act
        var result = await _productRepository.UpdateAsync(p => p.Articlenumber == nonExistentProduct.Articlenumber, updatedProduct);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task AddImageAsync_ReturnsProductWithAddedImageWhenProductExists()
    {
        // Arrange
        var existingProduct = new Product
        {
            Articlenumber = "12345678",
            Name = "Test Product",
            Description = "Test Description",
            Categoryid = _categoryId,
            Price = 0,
            Stock = 0,
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M"
            }
        };
        _context.Products.Add(existingProduct);
        await _context.SaveChangesAsync();

        var newImage = new Image
        {
            Id = Guid.NewGuid().ToString(),
            Url = "https://example.com/image.jpg"
        };

        // Act
        var result = await _productRepository.AddImageAsync(existingProduct.Articlenumber, newImage);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Articlenumber, Is.EqualTo(existingProduct.Articlenumber));
            Assert.That(result.Images.Contains(newImage), Is.True);
        });
    }
    
    [Test]
    public async Task AddImageAsync_ReturnsNullWhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentArticleNumber = "12345678";

        var newImage = new Image
        {
            Id = Guid.NewGuid().ToString(),
            Url = "https://example.com/image.jpg"
        };

        // Act
        var result = await _productRepository.AddImageAsync(nonExistentArticleNumber, newImage);

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task RemoveImageAsync_ReturnsTrueWhenImageExists()
    {
        // Arrange
        var existingProduct = new Product
        {
            Articlenumber = "12345678",
            Name = "Test Product",
            Description = "Test Description",
            Categoryid = _categoryId,
            Price = 0,
            Stock = 0,
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M"
            }
        };
        _context.Products.Add(existingProduct);
        await _context.SaveChangesAsync();

        var existingImage = new Image
        {
            Id = Guid.NewGuid().ToString(),
            Url = "https://example.com/image.jpg"
        };
        existingProduct.Images.Add(existingImage);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productRepository.RemoveImageAsync(existingProduct.Articlenumber, Guid.Parse(existingImage.Id));

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task RemoveImageAsync_ReturnsFalseWhenImageDoesNotExist()
    {
        // Arrange
        var nonExistentArticleNumber = "12345678";
        var nonExistentImageId = Guid.NewGuid();

        // Act
        var result = await _productRepository.RemoveImageAsync(nonExistentArticleNumber, nonExistentImageId);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsTrueWhenProductExists()
    {
        // Arrange
        var existingProduct = new Product
        {
            Articlenumber = "12345678",
            Name = "Test Product",
            Description = "Test Description",
            Categoryid = _categoryId,
            Price = 0,
            Stock = 0,
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M"
            },
            Images = new List<Image>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = "https://example.com/image.jpg"
                }
            }
        };
        _context.Products.Add(existingProduct);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productRepository.DeleteAsync(p => p.Articlenumber == existingProduct.Articlenumber);

        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task DeleteAsync_ReturnsFalseWhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentArticleNumber = "12345678";

        // Act
        var result = await _productRepository.DeleteAsync(p => p.Articlenumber == nonExistentArticleNumber);

        // Assert
        Assert.That(result, Is.False);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
    }
}