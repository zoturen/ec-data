using System.Linq.Expressions;
using Moq;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Tests.Services;

[TestFixture]
public class ProductService_Tests
{
    private Mock<IProductRepository> _mockProductRepository;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IImageRepository> _mockImageRepository;
    private IProductService _productService;

    [SetUp]
    public void SetUp()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockImageRepository = new Mock<IImageRepository>();
        _productService = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object, _mockImageRepository.Object);
    }

    [Test]
    public async Task GetAllProductsAsync_ReturnsExpectedProducts()
    {
        // Arrange
        var catId1 = Guid.NewGuid().ToString();
        var catId2 = Guid.NewGuid().ToString();
        var expectedProducts = new List<Product>
        {
            new Product
            {
                Articlenumber = "AN1",
                Name = "Product 1",
                Description = "Description 1",
                Price = 100,
                Stock = 10,
                Categoryid = catId1,
                Category = new Category
                {
                    Id = catId1,
                    Name = "Category 1"
                },
                Productdetail = new Productdetail
                {
                    Color = "Red",
                    Size = "M",
                }
            },
            new Product
            {
                Articlenumber = "AN2",
                Name = "Product 2",
                Description = "Description 2",
                Price = 100,
                Stock = 10,
                Categoryid = catId2,
                Category = new Category
                {
                    Id = catId2,
                    Name = "Category 2"
                },
                Productdetail = new Productdetail
                {
                    Color = "Red",
                    Size = "M",
                }
            }
        };
        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetAllProductsAsync();
        var productSimpleDtos = result.ToList();

        // Assert
        Assert.That(productSimpleDtos, Is.Not.Null, "The result should not be null.");
        Assert.That(productSimpleDtos, Has.Count.EqualTo(expectedProducts.Count));
        _mockProductRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
    
    [Test]
    public async Task GetProductByArticleNumberAsync_ReturnsExpectedProduct()
    {
        // Arrange
        var articleNumber = "AN1";
        var catId1 = Guid.NewGuid().ToString();
        var expectedProduct = new Product
        {
            Articlenumber = articleNumber,
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            Stock = 10,
            Categoryid = catId1,
            Category = new Category
            {
                Id = catId1,
                Name = "Category 1"
            },
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M",
            },
            Images = new List<Image>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = "https://example.com/image1.jpg"
                }
            }
        };
        _mockProductRepository.Setup(repo => repo.GetAsync(x => x.Articlenumber == articleNumber)).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.GetProductByArticleNumberAsync(articleNumber);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result?.ArticleNumber, Is.EqualTo(articleNumber));
        _mockProductRepository.Verify(repo => repo.GetAsync(x => x.Articlenumber == articleNumber), Times.Once);
    }
    
    [Test]
    public async Task CreateProductAsync_ReturnsExpectedProduct_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid().ToString();
        var productCreateDto = new ProductCreateDto
        {
            ArticleNumber = "AN1",
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            Stock = 10,
            CategoryId = categoryId,
            Color = "Red",
            Size = "M"
        };
        
        var expectedProduct = productCreateDto.ToEntity();
        _mockCategoryRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Expression<Func<Category, bool>> predicate) => predicate.Compile()(new Category { Id = categoryId }));
        _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.CreateProductAsync(productCreateDto);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result.ArticleNumber, Is.EqualTo(productCreateDto.ArticleNumber));
        _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockCategoryRepository.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);
    }
    
    [Test]
    public async Task UpdateProductAsync_ReturnsUpdatedProduct()
    {
        // Arrange
        var articleNumber = "AN1";
        var categoryId = Guid.NewGuid().ToString();
        var productUpdateDto = new ProductUpdateDto
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 200,
            Stock = 20,
            CategoryId = categoryId,
            Color = "Blue",
            Size = "L"
        };

        var existingProduct = new Product
        {
            Articlenumber = articleNumber,
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            Stock = 10,
            Categoryid = categoryId,
            Category = new Category
            {
                Id = categoryId,
                Name = "Category 1"
            },
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M",
            }
        };

        var updatedProduct = productUpdateDto.ToEntity();
        updatedProduct.Articlenumber = articleNumber; // Keep the same article number

        _mockCategoryRepository.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Expression<Func<Category, bool>> predicate) => predicate.Compile()(new Category { Id = categoryId }));
        _mockProductRepository.Setup(repo => repo.UpdateAsync(x => x.Articlenumber == articleNumber, It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productService.UpdateProductAsync(articleNumber, productUpdateDto);

        // Assert
        Assert.That(result, Is.True, "The result should not be null.");
        _mockCategoryRepository.Verify(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Once);
        _mockProductRepository.Verify(repo => repo.UpdateAsync(x => x.Articlenumber == articleNumber, It.IsAny<Product>()), Times.Once);
    }
    
    [Test]
    public async Task DeleteProductAsync_ReturnsTrue_WhenProductExists()
    {
        // Arrange
        var articleNumber = "AN1";
        
        _mockProductRepository.Setup(repo => repo.DeleteAsync(x => x.Articlenumber == articleNumber)).ReturnsAsync(true);

        // Act
        var result = await _productService.DeleteProductAsync(articleNumber);

        // Assert
        Assert.That(result, Is.True);
        _mockProductRepository.Verify(repo => repo.DeleteAsync(x => x.Articlenumber == articleNumber), Times.Once);
    }
    
    [Test]
    public async Task AddImageToProductAsync_ReturnsUpdatedProduct_WhenProductExists()
    {
        // Arrange
        var articleNumber = "AN1";
        var imageUrl = "http://example.com/image.jpg";
        var productImageCreateDto = new ProductImageCreateDto
        {
            ImageUrl = imageUrl
        };

        var existingProduct = new Product
        {
            Articlenumber = articleNumber,
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            Stock = 10,
            Categoryid = Guid.NewGuid().ToString(),
            Category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Category 1"
            },
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M",
            }
        };

        var updatedProduct = existingProduct;
        updatedProduct.Images.Add(new Image { Url = imageUrl });

        _mockProductRepository.Setup(repo => repo.ExistsAsync(p => p.Articlenumber == articleNumber)).ReturnsAsync(true);
        _mockProductRepository.Setup(repo => repo.AddImageAsync(articleNumber, It.IsAny<Image>())).ReturnsAsync(updatedProduct);

        // Act
        var result = await _productService.AddImageToProductAsync(articleNumber, productImageCreateDto);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result.Images, Has.Some.Property("ImageUrl").EqualTo(imageUrl));
        _mockProductRepository.Verify(repo => repo.ExistsAsync(p => p.Articlenumber == articleNumber), Times.Once);
        _mockProductRepository.Verify(repo => repo.AddImageAsync(articleNumber, It.IsAny<Image>()), Times.Once);
    }
    
    [Test]
    public async Task RemoveImageFromProductAsync_ReturnsTrue_WhenImageExists()
    {
        // Arrange
        var articleNumber = "AN1";
        var imageId = Guid.NewGuid();
        

        _mockProductRepository.Setup(repo => repo.RemoveImageAsync(articleNumber, imageId)).ReturnsAsync(true);
        _mockImageRepository.Setup(repo => repo.DeleteAsync(x => x.Id == imageId.ToString())).ReturnsAsync(true);

        // Act
        var result = await _productService.RemoveImageFromProductAsync(articleNumber, imageId);

        // Assert
        Assert.That(result, Is.True);
        _mockProductRepository.Verify(repo => repo.RemoveImageAsync(articleNumber, imageId), Times.Once);
        _mockImageRepository.Verify(repo => repo.DeleteAsync(x => x.Id == imageId.ToString()), Times.Once);
    }
    
    [Test]
    public async Task UpdateProductStockAsync_ReturnsTrue_WhenProductExists()
    {
        // Arrange
        var articleNumber = "AN1";
        var newStock = 20;
        var existingProduct = new Product
        {
            Articlenumber = articleNumber,
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            Stock = 10,
            Categoryid = Guid.NewGuid().ToString(),
            Category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Category 1"
            },
            Productdetail = new Productdetail
            {
                Color = "Red",
                Size = "M",
            }
        };

        var updatedProduct = existingProduct;
        updatedProduct.Stock = newStock;

        _mockProductRepository.Setup(repo => repo.GetAsync(x => x.Articlenumber == articleNumber)).ReturnsAsync(existingProduct);
        _mockProductRepository.Setup(repo => repo.UpdateAsync(x => x.Articlenumber == articleNumber, It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productService.UpdateProductStockAsync(articleNumber, newStock);

        // Assert
        Assert.That(result, Is.True);
        _mockProductRepository.Verify(repo => repo.GetAsync(x => x.Articlenumber == articleNumber), Times.Once);
        _mockProductRepository.Verify(repo => repo.UpdateAsync(x => x.Articlenumber == articleNumber, It.IsAny<Product>()), Times.Once);
    }
}