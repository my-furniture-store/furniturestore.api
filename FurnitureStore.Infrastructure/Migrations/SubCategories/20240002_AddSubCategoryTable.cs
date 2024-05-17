using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.SubCategories;

[Migration(20240002, description: "Add sub_category Table")]
public class AddSubCategoryTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("sub_category")
           .WithColumn("id").AsGuid().PrimaryKey()
           .WithColumn("name").AsString(25).NotNullable()
           .WithColumn("category_id").AsGuid().ForeignKey("category", "id").NotNullable();
    }
}
