using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tecmave.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixVehiculoProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "colaboradores",
                columns: table => new
                {
                    id_colaborador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    puesto = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fecha_contratacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colaboradores", x => x.id_colaborador);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados", x => x.id_estado);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "factura",
                columns: table => new
                {
                    id_factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    metodo_pago = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_factura", x => x.id_factura);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "marca",
                columns: table => new
                {
                    id_marca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marca", x => x.id_marca);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role_change_audit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TargetUserId = table.Column<int>(type: "int", nullable: false),
                    TargetUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreviousRole = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewRole = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: true),
                    ChangedByUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Action = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Detail = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceIp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_change_audit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipo_servicios",
                columns: table => new
                {
                    id_tipo_servicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_servicios", x => x.id_tipo_servicio);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notificaciones",
                columns: table => new
                {
                    id_notificaciones = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    mensaje = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_envio = table.Column<DateOnly>(type: "date", nullable: false),
                    tipo = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificaciones", x => x.id_notificaciones);
                    table.ForeignKey(
                        name: "FK_notificaciones_estados_id_estado",
                        column: x => x.id_estado,
                        principalTable: "estados",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "promociones",
                columns: table => new
                {
                    id_promocion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    titulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promociones", x => x.id_promocion);
                    table.ForeignKey(
                        name: "FK_promociones_estados_id_estado",
                        column: x => x.id_estado,
                        principalTable: "estados",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: true),
                    id_marca = table.Column<int>(type: "int", nullable: false),
                    anno = table.Column<int>(type: "int", nullable: false),
                    placa = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modelo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculos", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK_vehiculos_marca_id_marca",
                        column: x => x.id_marca,
                        principalTable: "marca",
                        principalColumn: "id_marca",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "servicios",
                columns: table => new
                {
                    id_servicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    tipo_servicio_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicios", x => x.id_servicio);
                    table.ForeignKey(
                        name: "FK_servicios_tipo_servicios_tipo_servicio_id",
                        column: x => x.tipo_servicio_id,
                        principalTable: "tipo_servicios",
                        principalColumn: "id_tipo_servicio",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agendamiento",
                columns: table => new
                {
                    id_agendamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    vehiculo_id = table.Column<int>(type: "int", nullable: false),
                    fecha_agregada = table.Column<DateOnly>(type: "date", maxLength: 150, nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_estimada = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_llegada = table.Column<TimeOnly>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agendamiento", x => x.id_agendamiento);
                    table.ForeignKey(
                        name: "FK_agendamiento_estados_id_estado",
                        column: x => x.id_estado,
                        principalTable: "estados",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_agendamiento_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "detalle_factura",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    factura_id = table.Column<int>(type: "int", nullable: false),
                    servicio_id = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    costo = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_factura", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK_detalle_factura_factura_factura_id",
                        column: x => x.factura_id,
                        principalTable: "factura",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_factura_servicios_servicio_id",
                        column: x => x.servicio_id,
                        principalTable: "servicios",
                        principalColumn: "id_servicio",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "resenas",
                columns: table => new
                {
                    id_resena = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    servicio_id = table.Column<int>(type: "int", nullable: false),
                    comentario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    calificacion = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resenas", x => x.id_resena);
                    table.ForeignKey(
                        name: "FK_resenas_servicios_servicio_id",
                        column: x => x.servicio_id,
                        principalTable: "servicios",
                        principalColumn: "id_servicio",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "revision",
                columns: table => new
                {
                    id_revision = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vehiculo_id = table.Column<int>(type: "int", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    id_servicio = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_estimada_entrega = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_entrega_final = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revision", x => x.id_revision);
                    table.ForeignKey(
                        name: "FK_revision_estados_id_estado",
                        column: x => x.id_estado,
                        principalTable: "estados",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_revision_servicios_id_servicio",
                        column: x => x.id_servicio,
                        principalTable: "servicios",
                        principalColumn: "id_servicio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_revision_vehiculos_vehiculo_id",
                        column: x => x.vehiculo_id,
                        principalTable: "vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "servicios_revision",
                columns: table => new
                {
                    id_servicio_revision = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    revision_id = table.Column<int>(type: "int", nullable: false),
                    servicio_id = table.Column<int>(type: "int", nullable: false),
                    costo_final = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicios_revision", x => x.id_servicio_revision);
                    table.ForeignKey(
                        name: "FK_servicios_revision_revision_revision_id",
                        column: x => x.revision_id,
                        principalTable: "revision",
                        principalColumn: "id_revision",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_servicios_revision_servicios_servicio_id",
                        column: x => x.servicio_id,
                        principalTable: "servicios",
                        principalColumn: "id_servicio",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "estados",
                columns: new[] { "id_estado", "nombre" },
                values: new object[,]
                {
                    { 1, "activo" },
                    { 2, "pendiente" },
                    { 3, "inactivo" },
                    { 4, "Ingresado" },
                    { 5, "En Diagnóstico" },
                    { 6, "Pendiente de aprobación" },
                    { 7, "En mantenimiento" },
                    { 8, "En pruebas" },
                    { 9, "Finalizado" },
                    { 10, "Entregado" },
                    { 11, "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "marca",
                columns: new[] { "id_marca", "nombre" },
                values: new object[,]
                {
                    { 1, "Jeep" },
                    { 2, "Dodge" },
                    { 3, "Toyota" },
                    { 4, "Nissan" },
                    { 5, "Honda" },
                    { 6, "Mitsubishi" },
                    { 7, "Suzuki" },
                    { 8, "Hyundai" },
                    { 9, "Chevrolet" },
                    { 10, "Chrysler" },
                    { 11, "Daihatsu" },
                    { 12, "RAM" },
                    { 13, "Ford" },
                    { 14, "GMC" },
                    { 15, "Hummer" },
                    { 16, "Isuzu" },
                    { 17, "Kia" },
                    { 18, "Lexus" },
                    { 19, "Mazda" }
                });

            migrationBuilder.InsertData(
                table: "tipo_servicios",
                columns: new[] { "id_tipo_servicio", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, "Servicios de mantenimiento periódico", "Mantenimiento preventivo" },
                    { 2, "Reparaciones por fallas", "Mantenimiento correctivo" },
                    { 3, "Diagnóstico/Reparación de una falla puntual", "Falla específica" }
                });

            migrationBuilder.InsertData(
                table: "servicios",
                columns: new[] { "id_servicio", "descripcion", "nombre", "precio", "tipo", "tipo_servicio_id" },
                values: new object[,]
                {
                    { 1, "Diagnóstico y reparación de sistemas electrónicos", "Electrónica", 120.00m, "Falla específica", 3 },
                    { 2, "Revisión y reparación del sistema de A/C", "Aire acondicionado", 150.00m, "Falla específica", 3 },
                    { 3, "Reparación de cajas manual/automática", "Transmisiones", 250.00m, "Falla específica", 3 },
                    { 4, "Cableado, luces y alternadores", "Electricidad", 100.00m, "Falla específica", 3 },
                    { 5, "Reemplazo y sellado", "Parabrisas", 180.00m, "Falla específica", 3 },
                    { 6, "Reparación y limpieza de interiores", "Tapicería", 130.00m, "Falla específica", 3 },
                    { 7, "General o parcial", "Pintura", 300.00m, "Falla específica", 3 },
                    { 8, "Aceite y filtro", "Cambio de aceite", 80.00m, "Mantenimiento preventivo", 1 },
                    { 9, "Chequeo completo", "Revisión general", 100.00m, "Mantenimiento preventivo", 1 },
                    { 10, "Pastillas y discos", "Cambio de frenos", 200.00m, "Mantenimiento correctivo", 2 },
                    { 11, "Fallas graves", "Reparación de motor", 500.00m, "Mantenimiento correctivo", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_agendamiento_id_estado",
                table: "agendamiento",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_agendamiento_vehiculo_id",
                table: "agendamiento",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_factura_factura_id",
                table: "detalle_factura",
                column: "factura_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_factura_servicio_id",
                table: "detalle_factura",
                column: "servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_notificaciones_id_estado",
                table: "notificaciones",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_promociones_id_estado",
                table: "promociones",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_resenas_servicio_id",
                table: "resenas",
                column: "servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_revision_id_estado",
                table: "revision",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_revision_id_servicio",
                table: "revision",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "IX_revision_vehiculo_id",
                table: "revision",
                column: "vehiculo_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_change_audit_Action",
                table: "role_change_audit",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_role_change_audit_ChangedAtUtc",
                table: "role_change_audit",
                column: "ChangedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_role_change_audit_TargetUserId",
                table: "role_change_audit",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_tipo_servicio_id",
                table: "servicios",
                column: "tipo_servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_revision_revision_id",
                table: "servicios_revision",
                column: "revision_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_revision_servicio_id",
                table: "servicios_revision",
                column: "servicio_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehiculos_id_marca",
                table: "vehiculos",
                column: "id_marca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agendamiento");

            migrationBuilder.DropTable(
                name: "colaboradores");

            migrationBuilder.DropTable(
                name: "detalle_factura");

            migrationBuilder.DropTable(
                name: "notificaciones");

            migrationBuilder.DropTable(
                name: "promociones");

            migrationBuilder.DropTable(
                name: "resenas");

            migrationBuilder.DropTable(
                name: "role_change_audit");

            migrationBuilder.DropTable(
                name: "servicios_revision");

            migrationBuilder.DropTable(
                name: "factura");

            migrationBuilder.DropTable(
                name: "revision");

            migrationBuilder.DropTable(
                name: "estados");

            migrationBuilder.DropTable(
                name: "servicios");

            migrationBuilder.DropTable(
                name: "vehiculos");

            migrationBuilder.DropTable(
                name: "tipo_servicios");

            migrationBuilder.DropTable(
                name: "marca");
        }
    }
}
