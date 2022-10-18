using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Data.Migrations
{
    public partial class hasindextablas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Proyectos_Clave",
                schema: "app",
                table: "Proyectos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

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
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador",
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
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Proyectos_Clave",
                schema: "app",
                table: "Proyectos",
                column: "Clave");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Colaboradores_CURP",
                schema: "app",
                table: "Colaboradores",
                column: "CURP");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Colaboradores_Id_Odoo",
                schema: "app",
                table: "Colaboradores",
                column: "Id_Odoo");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador");
        }
    }
}
