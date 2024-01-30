using OrderSolution.Api;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class ProductRepository(EcDbFirstContext context) : Repository<Product>(context), IProductRepository
{
    
}