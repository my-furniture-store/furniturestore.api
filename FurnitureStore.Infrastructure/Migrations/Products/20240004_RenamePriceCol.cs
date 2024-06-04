using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Products;

[Migration(20240004, description: "rename Price col to price(lowercase)")]
public class RenamePriceCol : AutoReversingMigration
{
    public override void Up()
    {
        Rename.Column("Price")
            .OnTable("product")
            .To("price");
    }
}
