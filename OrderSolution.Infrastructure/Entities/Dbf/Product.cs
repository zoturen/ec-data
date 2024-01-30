namespace OrderSolution.Infrastructure.Entities.Dbf;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string Categoryid { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Productdetail? Productdetail { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
