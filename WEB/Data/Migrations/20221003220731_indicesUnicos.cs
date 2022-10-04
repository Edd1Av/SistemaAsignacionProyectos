using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Data.Migrations
{
    public partial class indicesUnicos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos",
                column: "Clave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores",
                column: "CURP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores",
                column: "Id_Odoo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores",
                column: "RFC",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos",
                column: "Clave");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores",
                column: "CURP");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores",
                column: "Id_Odoo");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores",
                column: "RFC");
        }
    }
}
