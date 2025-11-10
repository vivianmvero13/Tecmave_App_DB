using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Front.Migrations
{
    /// <inheritdoc />
    public partial class IdentityInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "AspNetUsers",
                newName: "Apellidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Apellidos",
                table: "AspNetUsers",
                newName: "Apellido");
        }
    }
}
