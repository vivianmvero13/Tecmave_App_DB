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

CREATE TABLE aspnetusers (
  Id INT NOT NULL AUTO_INCREMENT,
  UserName VARCHAR(256) NULL,
  NormalizedUserName VARCHAR(256) NULL,
  Email VARCHAR(256) NULL,
  NormalizedEmail VARCHAR(256) NULL,
  EmailConfirmed TINYINT(1) NOT NULL DEFAULT 0,
  PasswordHash LONGTEXT NULL,
  SecurityStamp LONGTEXT NULL,
  ConcurrencyStamp LONGTEXT NULL,
  PhoneNumber LONGTEXT NULL,
  PhoneNumberConfirmed TINYINT(1) NOT NULL DEFAULT 0,
  TwoFactorEnabled TINYINT(1) NOT NULL DEFAULT 0,
  LockoutEnd DATETIME(6) NULL,
  LockoutEnabled TINYINT(1) NOT NULL DEFAULT 0,
  AccessFailedCount INT NOT NULL DEFAULT 0,
  PRIMARY KEY (Id),
  UNIQUE KEY IX_UserName (NormalizedUserName),
  KEY IX_Email (NormalizedEmail)
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

SET FOREIGN_KEY_CHECKS = 1;
