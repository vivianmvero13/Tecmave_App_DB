using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tecmave.Api.Migrations
{
    /// <inheritdoc />
    public partial class apellido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Apellidos",
                table: "aspnetusers",
                newName: "Apellido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "aspnetusers",
                newName: "Apellidos");
        }
    }
}
