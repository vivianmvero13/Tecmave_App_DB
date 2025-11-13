using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tecmave.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNombreApellidoToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "aspnetusers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "aspnetusers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "aspnetusers");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "aspnetusers");

        }
    }
}
