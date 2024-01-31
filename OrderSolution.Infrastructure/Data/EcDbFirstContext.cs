using Microsoft.EntityFrameworkCore;
using OrderSolution.Infrastructure.Entities.Dbf;

namespace OrderSolution.Infrastructure.Data;

public partial class EcDbFirstContext : DbContext
{
    public EcDbFirstContext()
    {
    }

    public EcDbFirstContext(DbContextOptions<EcDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productdetail> Productdetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("images_pkey");

            entity.ToTable("images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Articlenumber).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Articlenumber).HasColumnName("articlenumber");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(7, 2)
                .HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_categoryid_fkey");

            entity.HasMany(d => d.Images).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "Productimage",
                    r => r.HasOne<Image>().WithMany()
                        .HasForeignKey("Imageid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("productimages_imageid_fkey"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Productid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("productimages_productid_fkey"),
                    j =>
                    {
                        j.HasKey("Productid", "Imageid").HasName("productimages_pkey");
                        j.ToTable("productimages");
                        j.IndexerProperty<string>("Productid").HasColumnName("productid");
                        j.IndexerProperty<string>("Imageid").HasColumnName("imageid");
                    });
        });

        modelBuilder.Entity<Productdetail>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("productdetails_pkey");

            entity.ToTable("productdetails");

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .HasColumnName("color");
            entity.Property(e => e.Size)
                .HasMaxLength(30)
                .HasColumnName("size");

            entity.HasOne(d => d.Product).WithOne(p => p.Productdetail)
                .HasForeignKey<Productdetail>(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productdetails_productid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
