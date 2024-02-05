using OrderSolution.Infrastructure.Dtos;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;
using OrderSolution.Infrastructure.Services.Abstractions;

namespace OrderSolution.Infrastructure.Services;

public class ImageService(IImageRepository imageRepository) : IImageService
{
    public async Task<bool> UpdateImageAsync(Guid imageId, ImageUpdateDto dto)
    {
        var exists = await imageRepository.ExistsAsync(p => p.Id == imageId.ToString());
        if (!exists)
            return false;
        
        var image = new Image
        {
            Id = imageId.ToString(),
            Url = dto.ImageUrl
        };
        
        var result = await imageRepository.UpdateAsync(x => x.Id == imageId.ToString(), image);

        return result;
    }
}