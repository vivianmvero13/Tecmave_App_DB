using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tecmave.Api.Migrations
{
    /// <inheritdoc />
    public partial class correcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_promocion",
                table: "notificaciones");

            migrationBuilder.DropColumn(
                name: "NotificacionesActivadas",
                table: "aspnetusers");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "promocion_envios",
                newName: "id_usuario");

            migrationBuilder.RenameColumn(
                name: "IdPromocion",
                table: "promocion_envios",
                newName: "id_promocion");

            migrationBuilder.RenameColumn(
                name: "FechaEnvio",
                table: "promocion_envios",
                newName: "fecha_envio");

            migrationBuilder.RenameColumn(
                name: "IdEnvio",
                table: "promocion_envios",
                newName: "id_envio");
        }
    }
}
