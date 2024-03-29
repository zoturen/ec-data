﻿namespace OrderSolution.Infrastructure.Entities.Dbf;

public partial class Productdetail
{
    public string Productid { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Size { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
