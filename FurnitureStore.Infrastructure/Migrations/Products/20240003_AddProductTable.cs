using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Products;

[Migration(20240003, description: "Add product table")]
internal class AddProductTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("product")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(150).NotNullable()
            .WithColumn("description").AsCustom("TEXT").Nullable()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("category_id").AsGuid().ForeignKey("category", "id").NotNullable()
            .WithColumn("sub_category_id").AsGuid().ForeignKey("sub_category", "id").NotNullable()
            .WithColumn("sku").AsString(100).Nullable()
            .WithColumn("stock_quantity").AsInt32().Nullable()
            .WithColumn("image_url").AsString(250).Nullable()
            .WithColumn("dimensions").AsString(250).Nullable()
            .WithColumn("weight").AsDecimal().Nullable()
            .WithColumn("material").AsString(100).Nullable()
            .WithColumn("colors").AsCustom("JsonB").Nullable()
            .WithColumn("brand").AsString(50).Nullable()
            .WithColumn("rating").AsInt32().Nullable()
            .WithColumn("date_added").AsDateTime().Nullable()
            .WithColumn("is_featured").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("discount").AsDecimal().Nullable()
            .WithColumn("status").AsInt32().NotNullable();
    }
}
