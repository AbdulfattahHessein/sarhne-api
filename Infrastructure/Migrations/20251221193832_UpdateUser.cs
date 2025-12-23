using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdateUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "Password", table: "Users", newName: "PasswordHash");

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: false
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "IsActive", table: "Users");

        migrationBuilder.RenameColumn(name: "PasswordHash", table: "Users", newName: "Password");
    }
}
