using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tecmave.Api.Migrations
{
    /// <inheritdoc />
    public partial class cambio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    VehiculoIdVehiculo = table.Column<int>(type: "int", nullable: false),
                    FechaMantenimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    RecordatorioEnviado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos", x => x.IdMantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_vehiculos_VehiculoIdVehiculo",
                        column: x => x.VehiculoIdVehiculo,
                        principalTable: "vehiculos",
                        principalColumn: "id_vehiculo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_cliente_id",
                table: "vehiculos",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_VehiculoIdVehiculo",
                table: "Mantenimientos",
                column: "VehiculoIdVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_vehiculos_aspnetusers_cliente_id",
                table: "vehiculos",
                column: "cliente_id",
                principalTable: "aspnetusers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehiculos_aspnetusers_cliente_id",
                table: "vehiculos");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropIndex(
                name: "IX_vehiculos_cliente_id",
                table: "vehiculos");
        }
    }
}
