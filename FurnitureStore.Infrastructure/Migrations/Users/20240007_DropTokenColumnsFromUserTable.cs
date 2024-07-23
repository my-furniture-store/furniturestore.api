using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Users;

[Migration(20240007, ("drop access_token & password_reset_token columns from user table"))]
public class DropTokenColumnsFromUserTable : Migration
{
    public override void Up()
    {
        Delete.Column("access_token").FromTable("user");
        Delete.Column("password_reset_token").FromTable("user");
    }
    public override void Down()
    {
        Alter.Table("user")
            .AddColumn("access_token").AsString().Nullable()
            .AddColumn("password_reset_token").AsString().Nullable();
    }

}
