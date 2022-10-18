using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Data.Migrations
{
    public partial class DistribucionRealAsignacionReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.DropColumn(
                name: "Porcentaje",
                schema: "app",
                table: "DistribucionAsignacion");

            migrationBuilder.DropColumn(
                name: "Fecha_Final",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.DropColumn(
                name: "Fecha_Inicio",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Final",
                schema: "app",
                table: "DistribucionAsignacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Inicio",
                schema: "app",
                table: "DistribucionAsignacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Proyectos_Clave",
                schema: "app",
                table: "Proyectos",
                column: "Clave");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador");

            migrationBuilder.CreateTable(
                name: "AsignacionesReal",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsignacion = table.Column<int>(type: "int", nullable: false),
                    Fecha_Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_Final = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesReal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionesReal_Asignaciones_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalSchema: "app",
                        principalTable: "Asignaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistribucionAsignacionReal",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsignacionReal = table.Column<int>(type: "int", nullable: false),
                    IdProyecto = table.Column<int>(type: "int", nullable: false),
                    Porcentaje = table.Column<int>(type: "int", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistribucionAsignacionReal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistribucionAsignacionReal_AsignacionesReal_IdAsignacionReal",
                        column: x => x.IdAsignacionReal,
                        principalSchema: "app",
                        principalTable: "AsignacionesReal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistribucionAsignacionReal_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalSchema: "app",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesReal_IdAsignacion",
                schema: "app",
                table: "AsignacionesReal",
                column: "IdAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_DistribucionAsignacionReal_IdAsignacionReal",
                schema: "app",
                table: "DistribucionAsignacionReal",
                column: "IdAsignacionReal");

            migrationBuilder.CreateIndex(
                name: "IX_DistribucionAsignacionReal_IdProyecto",
                schema: "app",
                table: "DistribucionAsignacionReal",
                column: "IdProyecto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistribucionAsignacionReal",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AsignacionesReal",
                schema: "app");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Proyectos_Clave",
                schema: "app",
                table: "Proyectos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones");

            migrationBuilder.DropColumn(
                name: "Fecha_Final",
                schema: "app",
                table: "DistribucionAsignacion");

            migrationBuilder.DropColumn(
                name: "Fecha_Inicio",
                schema: "app",
                table: "DistribucionAsignacion");

            migrationBuilder.AddColumn<int>(
                name: "Porcentaje",
                schema: "app",
                table: "DistribucionAsignacion",
                type: "int",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Final",
                schema: "app",
                table: "Asignaciones",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_Inicio",
                schema: "app",
                table: "Asignaciones",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_Clave",
                schema: "app",
                table: "Proyectos",
                column: "Clave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asignaciones_IdColaborador",
                schema: "app",
                table: "Asignaciones",
                column: "IdColaborador");
        }
    }
}
