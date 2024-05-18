using FurnitureStore.Domain.SubCategories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureStore.Infrastructure.SubCategories;

public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("sub_category", "public");

        builder.HasKey(subCategory => subCategory.Id);

        builder.HasOne(subCategoy => subCategoy.Category)
            .WithMany(category => category.SubCategories)
            .HasForeignKey(subCategory => subCategory.CategoryId);

        builder.Property(subCategory => subCategory.Id)
            .HasColumnName("id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(subCategory => subCategory.Name)
            .HasColumnName("name")
            .HasColumnType("character varying")
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(subCategory => subCategory.CategoryId)
            .HasColumnName("category_id")
            .HasColumnType("guid")
            .IsRequired()
            .ValueGeneratedNever();
    }
}
