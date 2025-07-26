# Transformación Digital para Tecmave

Sistema web desarrollado para la modernización de procesos administrativos y de atención al cliente de la microempresa automotriz **Tecmave**.

Este proyecto tiene como objetivo optimizar la gestión de citas, control de colaboradores, facturación, seguimiento de vehículos y comunicación con clientes mediante una solución web robusta, segura y escalable.

---

## Tecnologías utilizadas

| Tecnología        | Rol en el sistema                         |
|-------------------|-------------------------------------------|
| Java 21           | Lenguaje principal para el backend        |
| Spring Boot 3.x   | Framework para la lógica del sistema      |
| Spring Security   | Control de autenticación y autorización   |
| Spring Data JPA   | Persistencia de datos con Hibernate       |
| MySQL 8.x         | Base de datos relacional                  |
| HTML/CSS/JS       | Interfaz de usuario                       |
| Thymeleaf         | Motor de plantillas del lado del servidor |


---

## Estructura del proyecto
src/
├── config/ → Configuraciones de seguridad, CORS, serialización
├── controller/ → Controladores REST que exponen las APIs
├── dto/ → Objetos de transferencia de datos
├── model/ → Entidades JPA que representan la BD
├── repository/ → Interfaces que extienden JpaRepository
├── service/ → Lógica de negocio encapsulada
├── resources/
└── application.properties → Configuración de base de datos y puertos

---

## 🔒 Seguridad

- Autenticación con Spring Security
- Roles de acceso: `ADMIN`, `CLIENTE`, `COLABORADOR`
- Acceso controlado a recursos mediante anotaciones (`@PreAuthorize`)
- Contraseñas cifradas con **BCrypt**
- Protección de endpoints con HTTPS en producción

---

## 📝 Funcionalidades principales

- 📆 Agendamiento de citas desde el panel del cliente
- 🧾 Generación de proformas y facturas automáticas
- 🚗 Seguimiento del estado del vehículo
- 🔔 Centro de notificaciones en tiempo real (planificado)
- 🧑‍🔧 Registro de entrada/salida de colaboradores
- 📩 Envío de boletines informativos vía correo electrónico (planificado)
- 📊 Bitácoras de acciones administrativas

---

## Requisitos para ejecutar el proyecto

### Requisitos mínimos

- JDK 21+
- Maven 3.8+
- MySQL Server 8.x
- Docker
- NetBeans

### 📦 Clonar el repositorio

```bash
git clone https://github.com/tuusuario/tecmave-transformacion-digital.git
cd tecmave-transformacion-digital

## Autores
Estudiantes:
Vivian Michelle Velázquez Rojas,
Joshua Emmanuel López Villanueva,  
Fabián Guevara Passot,  
Darian Khaled Gonzalez Rojas 

Instructor guía:
Mario Alberto Jimenez Espinoza

