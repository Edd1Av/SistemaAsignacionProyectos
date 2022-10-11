using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Data.Migrations
{
    public partial class Asignacion_Distribucion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.DropColumn(
                name: "RFC",
                schema: "app",
                table: "Colaboradores");

            migrationBuilder.CreateTable(
                name: "Asignacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha_Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_Final = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ColaboradorId = table.Column<int>(type: "int", nullable: false),
                    Id_Colaborador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asignacion_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalSchema: "app",
                        principalTable: "Colaboradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Distribucion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoId = table.Column<int>(type: "int", nullable: false),
                    AsignacionId = table.Column<int>(type: "int", nullable: false),
                    Id_Proyecto = table.Column<int>(type: "int", nullable: false),
                    Id_Asignacion = table.Column<int>(type: "int", nullable: false),
                    Porcentaje = table.Column<int>(type: "int", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distribucion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distribucion_Asignacion_AsignacionId",
                        column: x => x.AsignacionId,
                        principalTable: "Asignacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Distribucion_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "app",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignacion_ColaboradorId",
                table: "Asignacion",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Distribucion_AsignacionId",
                table: "Distribucion",
                column: "AsignacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Distribucion_ProyectoId",
                table: "Distribucion",
                column: "ProyectoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Distribucion");

            migrationBuilder.DropTable(
                name: "Asignacion");

            migrationBuilder.AddColumn<string>(
                name: "RFC",
                schema: "app",
                table: "Colaboradores",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_RFC",
                schema: "app",
                table: "Colaboradores",
                column: "RFC",
                unique: true);
        }
    }
}
