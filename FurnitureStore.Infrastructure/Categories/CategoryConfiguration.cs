using FurnitureStore.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureStore.Infrastructure.Categories;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("category", "public");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id).HasColumnName("id").HasColumnType("guid").IsRequired().ValueGeneratedNever();
        builder.Property(category => category.Name).HasColumnName("name").HasColumnType("character varying").IsRequired().HasMaxLength(50);
    }
}
