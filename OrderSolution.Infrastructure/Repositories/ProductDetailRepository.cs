using OrderSolution.Infrastructure.Data;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class ProductDetailRepository(EcDbFirstContext context) : Repository<Productdetail>(context), IProductDetailRepository
{
    
}