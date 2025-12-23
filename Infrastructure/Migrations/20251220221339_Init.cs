using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uniqueidentifier",
                    nullable: false,
                    defaultValueSql: "NEWSEQUENTIALID()"
                ),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ProfileSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<Guid>(
                    type: "uniqueidentifier",
                    nullable: false,
                    defaultValueSql: "NEWSEQUENTIALID()"
                ),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsPublic = table.Column<bool>(type: "bit", nullable: false),
                IsFav = table.Column<bool>(type: "bit", nullable: false),
                SendAnonymously = table.Column<bool>(type: "bit", nullable: false),
                SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    name: "FK_Messages_Users_ReceiverId",
                    column: x => x.ReceiverId,
                    principalTable: "Users",
                    principalColumn: "Id"
                );
                table.ForeignKey(
                    name: "FK_Messages_Users_SenderId",
                    column: x => x.SenderId,
                    principalTable: "Users",
                    principalColumn: "Id"
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ReceiverId",
            table: "Messages",
            column: "ReceiverId"
        );

        migrationBuilder.CreateIndex(
            name: "IX_Messages_SenderId",
            table: "Messages",
            column: "SenderId"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Messages");

        migrationBuilder.DropTable(name: "Users");
    }
}
