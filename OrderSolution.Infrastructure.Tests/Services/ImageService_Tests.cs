using Moq;
using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Tests.Services;

[TestFixture]
public class ImageService_Tests
{
    private Mock<IImageRepository> _mockImageRepository;
    private IImageService _imageService;

    [SetUp]
    public void SetUp()
    {
        _mockImageRepository = new Mock<IImageRepository>();
        _imageService = new ImageService(_mockImageRepository.Object);
    }
    
    [Test]
    public async Task UpdateImageAsync_ReturnsExpectedResult()
    {
        // Arrange
        var imageId = Guid.NewGuid();
        var dto = new ImageUpdateDto { ImageUrl = "http://example.com/image.jpg" };
        var mockImageRepository = new Mock<IImageRepository>();
        
        mockImageRepository.Setup(repo => repo.ExistsAsync(p => p.Id == imageId.ToString())).ReturnsAsync(true);
        mockImageRepository.Setup(repo => repo.UpdateAsync(x => x.Id == imageId.ToString(), It.IsAny<Image>())).ReturnsAsync(true);
        
        var imageService = new ImageService(mockImageRepository.Object);

        // Act
        var result = await imageService.UpdateImageAsync(imageId, dto);

        // Assert
        Assert.That(result, Is.True);
        mockImageRepository.Verify(repo => repo.ExistsAsync(p => p.Id == imageId.ToString()), Times.Once);
        mockImageRepository.Verify(repo => repo.UpdateAsync(x => x.Id == imageId.ToString(), It.IsAny<Image>()), Times.Once);
    }
    
    [Test]
    public async Task UpdateImageAsync_ReturnsFalse_WhenImageDoesNotExist()
    {
        // Arrange
        var imageId = Guid.NewGuid();
        var dto = new ImageUpdateDto { ImageUrl = "http://example.com/image.jpg" };
        _mockImageRepository.Setup(repo => repo.ExistsAsync(p => p.Id == imageId.ToString())).ReturnsAsync(false);

        // Act
        var result = await _imageService.UpdateImageAsync(imageId, dto);

        // Assert
        Assert.That(result, Is.False);
        _mockImageRepository.Verify(repo => repo.ExistsAsync(p => p.Id == imageId.ToString()), Times.Once);
        _mockImageRepository.Verify(repo => repo.UpdateAsync(x => x.Id == imageId.ToString(), It.IsAny<Image>()), Times.Never);
    }
}