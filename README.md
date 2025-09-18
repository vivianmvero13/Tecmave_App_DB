# Transformación Digital para Tecmave

Sistema web desarrollado para la modernización de procesos administrativos y de atención al cliente de la microempresa automotriz **Tecmave**.

Este proyecto tiene como objetivo optimizar la gestión de citas, control de colaboradores, facturación, seguimiento de vehículos y comunicación con clientes mediante una solución web robusta, segura y escalable.

---

## Tecnologías utilizadas

| Tecnología        | Rol en el sistema                         |
|-------------------|-------------------------------------------|
| C#                | Lenguaje principal para el backend        |
| Spring Boot 3.x   | Framework para la lógica del sistema      |
| MySQL 8.x         | Base de datos relacional                  |
| HTML/CSS/JS       | Interfaz de usuario                       |

---

## Seguridad

- Autenticación con Spring Security
- Roles de acceso: `ADMIN`, `CLIENTE`, `COLABORADOR`
- Acceso controlado a recursos mediante anotaciones (`@PreAuthorize`)
- Contraseñas cifradas con **BCrypt**
- Protección de endpoints con HTTPS en producción

---

## Funcionalidades principales

- Agendamiento de citas desde el panel del cliente
- Generación de proformas y facturas automáticas
- Seguimiento del estado del vehículo
- Centro de notificaciones en tiempo real (planificado)
- Registro de entrada/salida de colaboradores
- Envío de boletines informativos vía correo electrónico (planificado)
- Bitácoras de acciones administrativas

## Autores

**Estudiantes:**

- Vivian Michelle Velázquez Rojas  
- Joshua Emmanuel López Villanueva  
- Fabián Guevara Passot  
- Darian Khaled González Rojas
- Daniel Andrés Lopez Salazar

**Instructor guía:**  
Mario Alberto Jiménez Espinoza
