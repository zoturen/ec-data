using System.ComponentModel.DataAnnotations;

namespace OrderSolution.Infrastructure.Dtos;

public class CategoryCreateDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = null!;
}