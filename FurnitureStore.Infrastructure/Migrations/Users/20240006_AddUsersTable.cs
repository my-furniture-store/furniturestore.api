using FluentMigrator;

namespace FurnitureStore.Infrastructure.Migrations.Users;

[Migration(20240006, "add user table")]
public class AddUsersTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("user")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("username").AsString(64).NotNullable()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("password_hash").AsBinary(64).NotNullable()
            .WithColumn("password_salt").AsBinary(64).NotNullable()
            .WithColumn("access_token").AsString().Nullable()
            .WithColumn("verified_at").AsDateTime().Nullable()
            .WithColumn("password_reset_token").AsString().Nullable()
            .WithColumn("reset_token_expires").AsDateTime().Nullable();
    }
}
