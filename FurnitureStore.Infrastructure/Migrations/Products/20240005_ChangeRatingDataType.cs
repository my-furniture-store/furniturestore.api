using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Products;

[Migration(20240005, "Change rating type from int to decimal")]
public class ChangeRatingDataType : Migration
{

    public override void Up()
    {
        Alter.Column("rating")
            .OnTable("product")
            .AsDecimal()
            .Nullable();
    }
    public override void Down()
    {
        Alter.Column("rating")
            .OnTable("product")
            .AsInt32()
            .Nullable();
    }
}
