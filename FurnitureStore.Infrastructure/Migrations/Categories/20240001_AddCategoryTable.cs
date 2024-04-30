using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Categories;

[Migration(2024001, description: "Add Category Table")]
public class AddCategoryTable : Migration
{
    public override void Up()
    {
        Create.Table("category")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString(50).NotNullable();
    }
    public override void Down()
    {
        Delete.Table("category");
    }
}
