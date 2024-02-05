using System.ComponentModel.DataAnnotations;

namespace OrderSolution.Infrastructure.Dtos;

public class ProductImageCreateDto
{
    [Required]
    public string ImageUrl { get; set; } = null!;
}