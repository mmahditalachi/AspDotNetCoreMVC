using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.Migrations
{
    public partial class InitialMigratin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_User",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    HomeNumber = table.Column<string>(maxLength: 11, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 11, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    ConfrimPassword = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User", x => x.UserID);
                });

            migrationBuilder.InsertData(
                table: "_User",
                columns: new[] { "UserID", "ConfrimPassword", "Email", "FirstName", "HomeNumber", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("11a8e2a8-aeb6-4c22-90bc-9d7b9a5e07bd"), new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7"), "test3@test.com", "mohammad", "22771209", "talachi", new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7"), "09126344398", "king_mohi" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_User");
        }
    }
}
