USE tecmave;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS servicios_revision;
DROP TABLE IF EXISTS detalle_factura;
DROP TABLE IF EXISTS resenas;
DROP TABLE IF EXISTS agendamiento;
DROP TABLE IF EXISTS revision;
DROP TABLE IF EXISTS vehiculos;
DROP TABLE IF EXISTS promociones;
DROP TABLE IF EXISTS notificaciones;
DROP TABLE IF EXISTS servicios;
DROP TABLE IF EXISTS tipo_servicios;
DROP TABLE IF EXISTS marca;
DROP TABLE IF EXISTS modelo;
DROP TABLE IF EXISTS factura;
DROP TABLE IF EXISTS colaboradores;

DROP TABLE IF EXISTS aspnetroleclaims;
DROP TABLE IF EXISTS aspnetuserclaims;
DROP TABLE IF EXISTS aspnetusertokens;
DROP TABLE IF EXISTS aspnetuserlogins;
DROP TABLE IF EXISTS aspnetuserroles;
DROP TABLE IF EXISTS aspnetroles;
DROP TABLE IF EXISTS estados;
DROP TABLE IF EXISTS aspnetusers;
DROP TABLE IF EXISTS role_change_audit;

CREATE TABLE aspnetusers (
  Id INT NOT NULL AUTO_INCREMENT,
  Nombre VARCHAR(150) NOT NULL,
  Apellido VARCHAR(150) NOT NULL,
  Cedula VARCHAR(50) NOT NULL,
  Direccion VARCHAR(250) NOT NULL,
  UserName VARCHAR(256) DEFAULT NULL,
  NormalizedUserName VARCHAR(256) DEFAULT NULL,
  Email VARCHAR(256) DEFAULT NULL,
  NormalizedEmail VARCHAR(256) DEFAULT NULL,
  EmailConfirmed TINYINT(1) NOT NULL DEFAULT 0,
  PasswordHash LONGTEXT,
  SecurityStamp LONGTEXT,
  ConcurrencyStamp LONGTEXT,
  PhoneNumber VARCHAR(50) DEFAULT NULL,
  PhoneNumberConfirmed TINYINT(1) NOT NULL DEFAULT 0,
  TwoFactorEnabled TINYINT(1) NOT NULL DEFAULT 0,
  LockoutEnd DATETIME(6) DEFAULT NULL,
  LockoutEnabled TINYINT(1) NOT NULL DEFAULT 0,
  AccessFailedCount INT NOT NULL DEFAULT 0,
  PRIMARY KEY (Id),
  UNIQUE KEY UserNameIndex (NormalizedUserName),
  KEY EmailIndex (NormalizedEmail)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetroles (
  Id INT NOT NULL AUTO_INCREMENT,
  Name VARCHAR(256) NULL,
  NormalizedName VARCHAR(256) NULL,
  ConcurrencyStamp LONGTEXT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY IX_RoleName (NormalizedName)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetuserroles (
  UserId INT NOT NULL,
  RoleId INT NOT NULL,
  PRIMARY KEY (UserId, RoleId),
  KEY IX_AspNetUserRoles_RoleId (RoleId),
  CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES aspnetusers (Id) ON DELETE CASCADE,
  CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES aspnetroles (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetuserlogins (
  LoginProvider VARCHAR(255) NOT NULL,
  ProviderKey VARCHAR(255) NOT NULL,
  ProviderDisplayName LONGTEXT NULL,
  UserId INT NOT NULL,
  PRIMARY KEY (LoginProvider, ProviderKey),
  KEY IX_AspNetUserLogins_UserId (UserId),
  CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES aspnetusers (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetusertokens (
  UserId INT NOT NULL,
  LoginProvider VARCHAR(255) NOT NULL,
  Name VARCHAR(255) NOT NULL,
  Value LONGTEXT NULL,
  PRIMARY KEY (UserId, LoginProvider, Name),
  CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES aspnetusers (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetuserclaims (
  Id INT NOT NULL AUTO_INCREMENT,
  UserId INT NOT NULL,
  ClaimType LONGTEXT NULL,
  ClaimValue LONGTEXT NULL,
  PRIMARY KEY (Id),
  KEY IX_AspNetUserClaims_UserId (UserId),
  CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES aspnetusers (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE aspnetroleclaims (
  Id INT NOT NULL AUTO_INCREMENT,
  RoleId INT NOT NULL,
  ClaimType LONGTEXT NULL,
  ClaimValue LONGTEXT NULL,
  PRIMARY KEY (Id),
  KEY IX_AspNetRoleClaims_RoleId (RoleId),
  CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES aspnetroles (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE estados (
  id_estado INT NOT NULL,
  nombre VARCHAR(255) NOT NULL,
  PRIMARY KEY (id_estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO estados (id_estado, nombre) VALUES
(1, 'activo'),(2, 'pendiente'),(3, 'inactivo');

CREATE TABLE tipo_servicios (
  id_tipo_servicio INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(100) NOT NULL,
  descripcion VARCHAR(100) NOT NULL,
  PRIMARY KEY (id_tipo_servicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE servicios (
  id_servicio INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(150) NOT NULL,
  descripcion VARCHAR(150) NOT NULL,
  tipo VARCHAR(150) NOT NULL,
  precio DECIMAL(10,2) NOT NULL,
  tipo_servicio_id INT NOT NULL,
  PRIMARY KEY (id_servicio),
  KEY tipo_servicio_idx (tipo_servicio_id),
  CONSTRAINT FK_Servicios_TipoServicio FOREIGN KEY (tipo_servicio_id) REFERENCES tipo_servicios (id_tipo_servicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE modelo (
  id_modelo INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(255) NOT NULL,
  PRIMARY KEY (id_modelo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE marca (
  id_marca INT NOT NULL AUTO_INCREMENT,
  id_modelo INT NOT NULL,
  nombre VARCHAR(255) NOT NULL,
  PRIMARY KEY (id_marca),
  KEY FK_Marca_Modelo (id_modelo),
  CONSTRAINT FK_Marca_Modelo FOREIGN KEY (id_modelo) REFERENCES modelo (id_modelo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE vehiculos (
  id_vehiculo INT NOT NULL AUTO_INCREMENT,
  cliente_id INT DEFAULT NULL,
  id_marca INT NOT NULL,
  anno int NOT NULL,
  placa VARCHAR(255) NOT NULL,
  modelo VARCHAR(255),
  PRIMARY KEY (id_vehiculo),
  KEY FK_Vehiculos_Cliente (cliente_id),
  KEY FK_Vehiculos_Marca (id_marca),
  CONSTRAINT FK_Vehiculos_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers (Id),
  CONSTRAINT FK_Vehiculos_Marca FOREIGN KEY (id_marca) REFERENCES marca (id_marca)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE factura (
  id_factura INT NOT NULL AUTO_INCREMENT,
  cliente_id INT NULL,
  fecha_emision DATETIME NULL,
  total DECIMAL(10,2) NULL,
  metodo_pago VARCHAR(100) NULL,
  PRIMARY KEY (id_factura),
  KEY FK_Factura_Cliente (cliente_id),
  CONSTRAINT FK_Factura_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE detalle_factura (
  id_detalle INT NOT NULL AUTO_INCREMENT,
  factura_id INT NOT NULL,
  servicio_id INT NOT NULL,
  descripcion TEXT NOT NULL,
  costo DECIMAL(10,2) NOT NULL,
  subtotal DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (id_detalle),
  KEY FK_DetalleFactura_Factura (factura_id),
  KEY FK_DetalleFactura_Servicio (servicio_id),
  CONSTRAINT FK_DetalleFactura_Factura FOREIGN KEY (factura_id) REFERENCES factura (id_factura),
  CONSTRAINT FK_DetalleFactura_Servicio FOREIGN KEY (servicio_id) REFERENCES servicios (id_servicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE resenas (
  id_resena INT NOT NULL AUTO_INCREMENT,
  cliente_id INT DEFAULT NULL,
  servicio_id INT NOT NULL,
  comentario TEXT NOT NULL,
  calificacion FLOAT NOT NULL,
  PRIMARY KEY (id_resena),
  KEY FK_Resenas_Servicio (servicio_id),
  KEY FK_Resenas_Cliente (cliente_id),
  CONSTRAINT FK_Resenas_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers (Id),
  CONSTRAINT FK_Resenas_Servicio FOREIGN KEY (servicio_id) REFERENCES servicios (id_servicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE notificaciones (
  id_notificaciones INT NOT NULL AUTO_INCREMENT,
  usuario_id INT NOT NULL,
  mensaje VARCHAR(255) NOT NULL,
  fecha_envio DATE NOT NULL,
  tipo VARCHAR(45) NOT NULL,
  id_estado INT NOT NULL,
  PRIMARY KEY (id_notificaciones),
  KEY FK_Notificaciones_Usuario (usuario_id),
  KEY FK_Notificaciones_Estado (id_estado),
  CONSTRAINT FK_Notificaciones_Usuario FOREIGN KEY (usuario_id) REFERENCES aspnetusers (Id),
  CONSTRAINT FK_Notificaciones_Estado FOREIGN KEY (id_estado) REFERENCES estados (id_estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE promociones (
  id_promocion INT NOT NULL AUTO_INCREMENT,
  titulo VARCHAR(255) NOT NULL,
  descripcion VARCHAR(255) NOT NULL,
  fecha_inicio DATE NOT NULL,
  fecha_fin DATE NOT NULL,
  id_estado INT NOT NULL,
  PRIMARY KEY (id_promocion),
  KEY FK_Promociones_Estado (id_estado),
  CONSTRAINT FK_Promociones_Estado FOREIGN KEY (id_estado) REFERENCES estados (id_estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE revision (
  id_revision INT NOT NULL AUTO_INCREMENT,
  vehiculo_id INT NOT NULL,
  fecha_ingreso DATETIME NOT NULL,
  id_servicio INT NOT NULL,
  id_estado INT NOT NULL,
  fecha_estimada_entrega DATETIME NOT NULL,
  fecha_entrega_final DATETIME NOT NULL,
  PRIMARY KEY (id_revision),
  KEY FK_Revision_Vehiculo (vehiculo_id),
  KEY FK_Revision_Servicio (id_servicio),
  KEY FK_Revision_Estado (id_estado),
  CONSTRAINT FK_Revision_Vehiculo FOREIGN KEY (vehiculo_id) REFERENCES vehiculos (id_vehiculo),
  CONSTRAINT FK_Revision_Servicio FOREIGN KEY (id_servicio) REFERENCES servicios (id_servicio),
  CONSTRAINT FK_Revision_Estado FOREIGN KEY (id_estado) REFERENCES estados (id_estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE servicios_revision (
  id_servicio_revision INT NOT NULL AUTO_INCREMENT,
  revision_id INT DEFAULT NULL,
  servicio_id INT DEFAULT NULL,
  costo_final DECIMAL(10,2) DEFAULT NULL,
  PRIMARY KEY (id_servicio_revision),
  KEY FK_SR_Servicio (servicio_id),
  KEY FK_SR_Revision (revision_id),
  CONSTRAINT FK_SR_Revision FOREIGN KEY (revision_id) REFERENCES revision (id_revision),
  CONSTRAINT FK_SR_Servicio FOREIGN KEY (servicio_id) REFERENCES servicios (id_servicio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE agendamiento (
  id_agendamiento INT NOT NULL AUTO_INCREMENT,
  cliente_id INT DEFAULT NULL,
  vehiculo_id INT NOT NULL,
  fecha_agregada VARCHAR(150) NOT NULL,
  id_estado INT NOT NULL DEFAULT 2,
  PRIMARY KEY (id_agendamiento),
  KEY FK_Agendamiento_Vehiculo (vehiculo_id),
  KEY FK_Agendamiento_Cliente (cliente_id),
  KEY FK_Agendamiento_Estado (id_estado),
  CONSTRAINT FK_Agendamiento_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers (Id),
  CONSTRAINT FK_Agendamiento_Vehiculo FOREIGN KEY (vehiculo_id) REFERENCES vehiculos (id_vehiculo),
  CONSTRAINT FK_Agendamiento_Estado FOREIGN KEY (id_estado) REFERENCES estados (id_estado)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE colaboradores (
  id_colaborador INT NOT NULL AUTO_INCREMENT,
  id_usuario INT NOT NULL,
  puesto VARCHAR(45) NOT NULL,
  salario DECIMAL(10,2) NOT NULL,
  fecha_contratacion VARCHAR(45) NOT NULL,
  PRIMARY KEY (id_colaborador),
  KEY FK_Colab_Usuario (id_usuario),
  CONSTRAINT FK_Colab_Usuario FOREIGN KEY (id_usuario) REFERENCES aspnetusers (Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE role_change_audit (
  Id BIGINT AUTO_INCREMENT PRIMARY KEY,
  TargetUserId INT NOT NULL,
  TargetUserName VARCHAR(256) NULL,
  PreviousRole VARCHAR(256) NULL,
  NewRole VARCHAR(256) NULL,
  ChangedByUserId INT NULL,
  ChangedByUserName VARCHAR(256) NULL,
  ChangedAtUtc DATETIME(6) NOT NULL,
  Action VARCHAR(20) NOT NULL,
  Detail VARCHAR(1024) NULL,
  SourceIp VARCHAR(64) NULL,
  INDEX IX_role_change_audit_TargetUserId (TargetUserId),
  INDEX IX_role_change_audit_ChangedAtUtc (ChangedAtUtc),
  INDEX IX_role_change_audit_Action (Action)
);

ALTER TABLE aspnetroles
  ADD COLUMN Description varchar(256) NULL AFTER NormalizedName,
  ADD COLUMN IsActive tinyint(1) NOT NULL DEFAULT 1 AFTER Description;

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO estados (id_estado, nombre) VALUES
(4, 'Ingresado'),
(5, 'En Diagnóstico'),
(6, 'Pendiente de aprobación'),
(7, 'En mantenimiento'),
(8, 'En pruebas'),
(9, 'Finalizado'),
(10, 'Entregado'),
(11, 'Cancelado');

-- 1) Quitar la foreign key de 'marca' que apunta a 'modelo'
ALTER TABLE marca DROP FOREIGN KEY FK_Marca_Modelo;
ALTER TABLE marca DROP INDEX FK_Marca_Modelo;
ALTER TABLE marca DROP COLUMN id_modelo;
DROP TABLE modelo;

USE tecmave;

INSERT INTO marca (nombre) VALUES
('Sin marca'),
('Jeep'),
('Dodge'),
('Toyota'),
('Nissan'),
('Honda'),
('Mitsubishi'),
('Suzuki'),
('Hyundai'),
('Chevrolet'),
('Chrysler'),
('Daihatsu'),
('RAM'),
('Ford'),
('GMC'),
('Hummer'),
('Isuzu'),
('Kia'),
('Lexus'),
('Mazda');

  
INSERT INTO aspnetroles (Id, Name, NormalizedName, Description, IsActive)
VALUES
(1, 'Admin', 'ADMIN', 'Administrador con acceso completo al sistema', 1),
(2, 'Colaborador', 'COLABORADOR', 'Empleado o técnico del taller con permisos limitados', 1),
(3, 'Cliente', 'CLIENTE', 'Usuario cliente que puede ver y registrar sus vehículos', 1);


INSERT INTO aspnetusers
(Nombre, Apellidos, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES
('Vivian', 'Velazquez', 'Vivian', 'VIVIAN', 'vivian@tecmave.com', 'VIVIAN@TECMAVE.COM', 1,
 'AQAAAAIAAYagAAAAEAdminHashDemo==', 'SEC123', 'CONC123', '88888888', 1, 0, 0, 0),

('Joshua','Lopez', 'Joshua', 'JOSHUA', 'joshua@tecmave.com', 'joshua@TECMAVE.COM', 1,
 'AQAAAAIAAYagAAAAEColabHashDemo==', 'SEC456', 'CONC456', '87777777', 1, 0, 0, 0),

('Khaled', 'Gonzalez', 'Khaled', 'KHALED', 'khaled@tecmave.com', 'KHALED@TECMAVE.COM', 1,
 'AQAAAAIAAYagAAAAEClienteHashDemo==', 'SEC789', 'CONC789', '86666666', 1, 0, 0, 0),

('Daniel', 'Lopez', 'Daniel', 'DANIEL', 'Daniel@tecmave.com', 'DANIEL@TECMAVE.COM', 1,
 'AQAAAAIAAYagAAAAEClienteHashDemo==', 'SEC789', 'CONC789', '86666666', 1, 0, 0, 0);

INSERT INTO servicios (nombre, descripcion, tipo, precio, tipo_servicio_id)
VALUES
('Electrónica', 'Diagnóstico y reparación de sistemas electrónicos', 'Falla específica', 120.00, 3),
('Aire acondicionado', 'Revisión y reparación del sistema de A/C', 'Falla específica', 150.00, 3),
('Transmisiones', 'Reparación de cajas de transmisión manual y automática', 'Falla específica', 250.00, 3),
('Electricidad', 'Revisión de cableado, luces y alternadores', 'Falla específica', 100.00, 3),
('Parabrisas', 'Reemplazo y sellado de parabrisas', 'Falla específica', 180.00, 3),
('Tapicería', 'Reparación y limpieza de interiores', 'Falla específica', 130.00, 3),
('Pintura', 'Pintura general o parcial del vehículo', 'Falla específica', 300.00, 3);

-- Mantenimiento preventivo
INSERT INTO servicios (nombre, descripcion, tipo, precio, tipo_servicio_id)
VALUES
('Cambio de aceite', 'Sustitución de aceite y filtro para mantener el motor en óptimas condiciones', 'Mantenimiento preventivo', 80.00, 1),
('Revisión general', 'Chequeo completo del vehículo para prevenir averías', 'Mantenimiento preventivo', 100.00, 1);

-- Mantenimiento correctivo
INSERT INTO servicios (nombre, descripcion, tipo, precio, tipo_servicio_id)
VALUES
('Cambio de frenos', 'Reemplazo de pastillas y discos de freno', 'Mantenimiento correctivo', 200.00, 2),
('Reparación de motor', 'Corrección de fallas graves en el motor', 'Mantenimiento correctivo', 500.00, 2);


-- Tabla: vehiculos
ALTER TABLE vehiculos
  ADD CONSTRAINT FK_Vehiculos_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers(Id);

-- Tabla: factura
ALTER TABLE factura
  ADD CONSTRAINT FK_Factura_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers(Id);

-- Tabla: resenas
ALTER TABLE resenas
  ADD CONSTRAINT FK_Resenas_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers(Id);

-- Tabla: notificaciones
ALTER TABLE notificaciones
  ADD CONSTRAINT FK_Notificaciones_Usuario FOREIGN KEY (usuario_id) REFERENCES aspnetusers(Id);

-- Tabla: agendamiento
ALTER TABLE agendamiento
  ADD CONSTRAINT FK_Agendamiento_Cliente FOREIGN KEY (cliente_id) REFERENCES aspnetusers(Id);

-- Tabla: colaboradores
ALTER TABLE colaboradores
  ADD CONSTRAINT FK_Colab_Usuario FOREIGN KEY (id_usuario) REFERENCES aspnetusers(Id) ON DELETE CASCADE;
  
  
  -- Tabla: vehiculos
ALTER TABLE vehiculos DROP FOREIGN KEY FK_Vehiculos_Cliente;

-- Tabla: factura
ALTER TABLE factura DROP FOREIGN KEY FK_Factura_Cliente;

-- Tabla: resenas
ALTER TABLE resenas DROP FOREIGN KEY FK_Resenas_Cliente;

-- Tabla: notificaciones
ALTER TABLE notificaciones DROP FOREIGN KEY FK_Notificaciones_Usuario;

-- Tabla: agendamiento
ALTER TABLE agendamiento DROP FOREIGN KEY FK_Agendamiento_Cliente;

-- Tabla: colaboradores
ALTER TABLE colaboradores DROP FOREIGN KEY FK_Colab_Usuario;

