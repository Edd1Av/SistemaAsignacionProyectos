using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Migrations
{
    public partial class FixLogger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                schema: "app",
                table: "Logger",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "app",
                table: "Logger",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "059b42ea-c0f9-4837-aa84-4c6c75fd7bd9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c6e284e-4b1e-557f-97af-594d67fd8321",
                column: "ConcurrencyStamp",
                value: "cc385a8c-23eb-4e4a-83b1-e91c338a411c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9fa134a9-f425-402b-95ff-ab740cf023be", "AQAAAAEAACcQAAAAENptonQiFnioTupw4ZDW6psj3walF81Y2deoJnyveH77p8wKnLnBMqEK6ls1V02AxQ==", "23f72f6c-afc4-4984-8d6a-fd7f6f3bdbea" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "app",
                table: "Logger");

            migrationBuilder.AlterColumn<int>(
                name: "Accion",
                schema: "app",
                table: "Logger",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "9d2e7779-792a-4525-8341-9c0178e5d2f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c6e284e-4b1e-557f-97af-594d67fd8321",
                column: "ConcurrencyStamp",
                value: "fdedd739-6458-4810-9eb9-2bccf5687ca3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6a29b241-a1fe-40b1-b29e-d021bdea2f42", "AQAAAAEAACcQAAAAEEyx+ZADGomoJf9ff7imwKqVBbB+WLOXEfJ4v8BzIkhMPPMq2BJtaBUjeVp6fi+kBg==", "7aba50b5-bb2e-4621-8401-811876939061" });
        }
    }
}
