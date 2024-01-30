using OrderSolution.Api;
using OrderSolution.Infrastructure.Entities.Dbf;
using OrderSolution.Infrastructure.Repositories.Abstractions;

namespace OrderSolution.Infrastructure.Repositories;

public class CategoryRepository(EcDbFirstContext context) : Repository<Category>(context), ICategoryRepository
{
    
}