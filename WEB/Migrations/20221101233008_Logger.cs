using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Migrations
{
    public partial class Logger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logger",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logger", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logger",
                schema: "app");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "a18d0f63-9443-4336-adc7-4f1b9e6194a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c6e284e-4b1e-557f-97af-594d67fd8321",
                column: "ConcurrencyStamp",
                value: "10f4c021-8e32-4aef-9dbf-fa49c017e7ad");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2a2b3a3-8157-491b-afe1-2919433b0a03", "AQAAAAEAACcQAAAAEAnOhhmObgKngFYx4aM7/6U4sP/cc+lZjX/Xkc4Khzrmrha/fDpktje5gLgy8+LOwg==", "58abf16a-99d4-47a5-8941-c7411b92dc03" });
        }
    }
}
