using OrderSolution.Infrastructure.Dtos;

namespace OrderSolution.Infrastructure.Services.Abstractions;

public interface IImageService
{
    Task<bool> UpdateImageAsync(Guid imageId, ImageUpdateDto dto);
}