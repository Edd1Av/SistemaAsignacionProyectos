using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Migrations
{
    public partial class unaasignacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "13c5f6bc-ce60-4daf-96cf-b9774c35d9b0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c6e284e-4b1e-557f-97af-594d67fd8321",
                column: "ConcurrencyStamp",
                value: "ce6ddedb-4e70-451b-9000-94410fa251e0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "854ff6b9-e056-41ef-85b9-0f5cbb62e7c7", "AQAAAAEAACcQAAAAEBPfpb48QoCy5XibZI3jcVrl7aquONCj1ZS91cistA8KXTRFgJMQ6dcR2phe4QeMSA==", "06ee17aa-1687-4a05-8add-70b7e003de58" });

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

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

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador");
        }
    }
}
