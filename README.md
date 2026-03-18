# Documentación Técnica: Sistema de Gestión Integrada de Reservas Hoteleras (ProBook)

**Información General**
**Nombre del Proyecto:** ProBook
**Asignatura:** Programación IV-Web 

---

## 1. Descripción del Proyecto

ProBook es una plataforma web desarrollada para la gestión hotelera, orientada a digitalizar el proceso de reservas. El sistema está diseñado para hoteles boutique, resorts y cadenas hoteleras independientes. 
Garantiza los principios operacionales fundamentales: una reserva por habitación-período, confidencialidad en pagos y transparencia en la disponibilidad.

La arquitectura del sistema maneja dos tipos de usuarios:
**Admin:** Encargados de la gestión de habitaciones y el monitoreo de disponibilidad mediante reportes y estadísticas en tiempo real.
**Huéspedes:** Usuarios con capacidad de visualizar disponibilidad y realizar reservas de forma única y vinculante.

---

## 2. Tecnologías y Arquitectura

El sistema backend ha sido construido bajo una arquitectura de controladores y servicios implementando el principio de Inyección de Dependencias.

**Framework Backend:** ASP.NET Core 6.0 Web API.
**Autenticación:** JSON Web Tokens (JWT) para autenticación y Firebase Authentication.
**Base de Datos:** Firebase Firestore (NoSQL.
**Integración Cloud:** Firebase Admin SDK y Firebase Storage.

---

## 3. Estructura de la Base de Datos (Firestore)

El modelo de datos se compone de tres colecciones principales:

**Users:** Almacena la información de cada usuario incluyendo identificador único, email, nombre y rol. Incluye campos de control como "hasReserved" para indicar si ya realizó una reserva.
Los campos "reservedRoom" y "reservedDates" guardan la habitación y período seleccionado. El campo "reservationTimestamp" registra fecha y hora para auditoría.
**Rooms:** Contiene la información de cada habitación: número, tipo, capacidad, amenidades, URL de fotos, tarifa base y contador de reservas. Implementa campos de auditoría "createdAt" y "createdBy" para rastrear quién y cuándo se registró la habitación.
**Reservations:** Registro inmutable de cada reserva emitida. Almacena las relaciones entre huéspedes y habitaciones (userId, roomId, roomNumber, etc.), fechas de estadía (checkInDate, checkOutDate), noches, costoTotal y timestamp. Funciona como log de auditoría sin comprometer confidencialidad.

---

## 4. Módulos y Requerimientos Funcionales

### 4.1. Autenticación y Autorización
Registro de usuarios (huésped por defecto) e inicio de sesión.
Control de acceso basado en roles implementado a nivel de endpoints mediante validación de tokens JWT con expiración.

### 4.2. Panel de Gerente
**Gestión de Habitaciones:** Permite crear habitación (número, tipo, capacidad, amenidades, tarifa base). Permite editar información y visualizar lista de habitaciones. Aplica la restricción de eliminar habitaciones sin reservas.
**Gestión de Huéspedes:** Visualización de lista de huéspedes registrados y estado de reserva (confirmada/pendiente).
**Dashboard y Reportes:** Visualización del total de habitaciones, noches reservadas, porcentaje de ocupación e ingresos generados.

### 4.3. Panel de Huésped
**Reserva:** Visualizar habitaciones disponibles, seleccionar y confirmar reserva. Visualizar costo total con impuestos.
**Validaciones:** Verificar que no haya reservado previamente, aplicando un bloqueo permanente después de reservar.Mostrar información si ya reservó. Imposibilidad de modificar la reserva.

---

## 5. Prevención de Doble Reserva (Concurrencia)

El sistema implementa mecanismos robustos de seguridad transaccional para garantizar la reserva única:
Transacciones de Firestore y actualización atómica del campo "hasReserved".
Verificación en base de datos antes de reservar.

--

## 6. Endpoints Principales

### Autenticacion
* POST /api/auth/register - Registro de nuevos usuarios.
* POST /api/auth/login - Inicio de sesión (Retorna Token JWT).

### Habitaciones (Rooms)
* GET /api/reservations/available - Consultar disponibilidad por fechas.
* POST /api/rooms - Crear habitacion (Solo Admin).

### Reservas (Reservations)
* POST /api/reservations - Crear reserva (Requiere Auth y valida fechas).
* POST /api/reservations/calculate - Pre-visualizar costos e impuestos (15% ISV).
* PATCH /api/reservations/{id}/status - Cambiar estado y liberar recursos.

---

## 7. Guía de Configuración y Despliegue

### Requisitos Previos
.NET SDK 6.0.
Visual Studio 2022 o Jetbrains Rider.
Cuenta de Firebase  con Firestore habilitado.

### Configuración del Entorno

1. **Clonación del Repositorio:**
   Descargar el código fuente del repositorio y abrir la solución en Visual Studio o un IDE compatible.

2. **Configuración de Credenciales de Firebase:**
   * Generar la clave privada de cuenta de servicio en formato JSON desde la consola de Google Cloud/Firebase.
   * Ubicar el archivo JSON en la ruta `/Config/` dentro del directorio principal del proyecto API.
   * Asegurar que las propiedades del archivo estén configuradas como "Copiar si es posterior" (Copy if newer) en el proceso de compilación.

3. **Variables de Configuración (appsettings.json):**
   Definir los parámetros de seguridad para la emisión de tokens JWT:
   ```json
   "Jwt": {
     "SecretKey": "[Insertar clave secreta de longitud adecuada]",
     "Issuer": "ProBook",
     "Audience": "ProBookUsers"
   }

---
## Creditos
Desarrollado como proyecto integrador para el curso de Programacion Web 2026.
Grupo #4 - Proyecto ProBook. Trabajo humilde pero funcional :D. 
