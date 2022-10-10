using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class AddMigrationnotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable");

            migrationBuilder.RenameTable(
                name: "UserTable",
                newName: "userTable");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "userTable",
                newName: "userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userTable",
                table: "userTable",
                column: "userId");

            migrationBuilder.CreateTable(
                name: "NoteTable",
                columns: table => new
                {
                    NoteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Reminder = table.Column<DateTime>(nullable: false),
                    Colour = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Archive = table.Column<bool>(nullable: false),
                    Pinned = table.Column<bool>(nullable: false),
                    Trash = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Edited = table.Column<DateTime>(nullable: false),
                    userId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTable", x => x.NoteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userTable",
                table: "userTable");

            migrationBuilder.RenameTable(
                name: "userTable",
                newName: "UserTable");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "UserTable",
                newName: "userID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable",
                column: "userID");
        }
    }
}
