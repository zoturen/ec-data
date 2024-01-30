using OrderSolution.Api;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class ImageRepository(EcDbFirstContext context) : Repository<Image>(context), IImageRepository
{
    
}