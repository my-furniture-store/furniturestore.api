using FurnitureStore.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureStore.Infrastructure.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product", "public");

        builder.HasKey(product => product.Id);

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(product => product.SubCategory)
            .WithMany(subCategory => subCategory.Products)
            .HasForeignKey(product => product.SubCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(product => product.Id)
            .HasColumnName("id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(product => product.Name)
            .HasColumnName("name")
            .HasColumnType("character varying")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(product => product.Description)
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(product => product.Price)
            .HasColumnName("price")
            .HasColumnType("decimal")
            .IsRequired();

        builder.Property(product => product.CategoryId)
            .HasColumnName("category_id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(product => product.SubCategoryId)
            .HasColumnName("sub_category_id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(product => product.SKU)
            .HasColumnName("sku")
            .HasColumnType("character varying")
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(product => product.StockQuantity)
            .HasColumnName("stock_quantity")
            .HasColumnType("int")
            .IsRequired(false);

        builder.Property(product => product.ImageUrl)
            .HasColumnName("image_url")
            .HasColumnType("character varying")
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(product => product.Dimensions)
            .HasColumnName("dimensions")
            .HasColumnType("character varying")
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(product => product.Weight)
            .HasColumnName("weight")
            .HasColumnType("decimal")
            .IsRequired(false);

        builder.Property(product => product.Material)
            .HasColumnName("material")
            .HasColumnType("character varying")
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(product => product.Colors)
            .HasColumnName("colors")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(product => product.Brand)
            .HasColumnName("brand")
            .HasColumnType("character varying")
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(product => product.Rating)
            .HasColumnName("rating")
            .HasColumnType("decimal")
            .IsRequired(false);

        builder.Property(product => product.DateAdded)
            .HasColumnName("date_added")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(product => product.IsFeatured)
            .HasColumnName("is_featured")
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(product => product.Discount)
            .HasColumnName("discount")
            .HasColumnType("decimal")
            .IsRequired(false);

        builder.Property(product => product.Status)
            .HasColumnName("status")
            .HasConversion(
                p => p.Value,
                value => ProductStatus.FromValue(value))
            .IsRequired();
    }
}
