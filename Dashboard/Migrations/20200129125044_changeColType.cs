using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.Migrations
{
    public partial class changeColType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfrimPassword",
                table: "_User");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "_User",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "_User",
                keyColumn: "UserID",
                keyValue: new Guid("11a8e2a8-aeb6-4c22-90bc-9d7b9a5e07bd"),
                column: "Password",
                value: "4bf44e48-7066-483e-ba40-8be7141c5be7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Password",
                table: "_User",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "ConfrimPassword",
                table: "_User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "_User",
                keyColumn: "UserID",
                keyValue: new Guid("11a8e2a8-aeb6-4c22-90bc-9d7b9a5e07bd"),
                columns: new[] { "ConfrimPassword", "Password" },
                values: new object[] { new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7"), new Guid("4bf44e48-7066-483e-ba40-8be7141c5be7") });
        }
    }
}
