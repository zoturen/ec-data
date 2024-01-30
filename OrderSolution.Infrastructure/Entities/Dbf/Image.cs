namespace OrderSolution.Infrastructure.Entities.Dbf;

public partial class Image
{
    public string Id { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
