using System.ComponentModel.DataAnnotations;

namespace OrderSolution.Infrastructure.Dtos;

public class ImageUpdateDto
{
    [Required]
    public string ImageUrl { get; set; }
}