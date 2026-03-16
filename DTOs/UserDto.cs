namespace Proyecto_Grupo4.API.DTOs;


/// UserDto es lo que se envía hacia el Front-End cuando se solicita info del usuario.

public class UserDto
{
    /**
     * Id único del usuario (Mapeado de Usuario.Id)
     */
    public string Id { get; set; } = string.Empty;
    
    /**
     * Nombre visible del usuario (Mapeado de Usuario.Nombre)
     */
    public string FullName { get; set; } = string.Empty;
    
    /**
     * Correo electrónico (Mapeado de Usuario.Correo)
     */
    public string Email { get; set; } = string.Empty;
    
    /**
     * Rol en el sistema: huésped, gerente, etc. (Mapeado de Usuario.Rol)
     */
    public string Role { get; set; } = "huésped";
}


/// RegisterDto es lo que recibe el backend para crear un nuevo 'Usuario'

public class RegisterDto
{
    /**
     * Correo para la cuenta (Se guardará en Usuario.Correo)
     */
    public string Email { get; set; } = string.Empty;
    
    /**
     * Contraseña (Se guardará en Usuario.Costrasena)
     */
    public string Password { get; set; } = string.Empty;
    
    /**
     * Nombre completo (Se guardará en Usuario.Nombre)
     */
    public string FullName { get; set; } = string.Empty;
}


/// LoginDto contiene las credenciales para buscar al 'Usuario' en Firestore

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


/// AuthResponseDto es la respuesta tras la autenticación exitosa

public class AuthResponseDto
{
    /**
     * Indica si el proceso fue exitoso
     */
    public bool Success { get; set; }
    
    /**
     * Mensaje de estado (ej: "Login exitoso")
     */
    public string Message { get; set; } = string.Empty;
    
    /**
     * Token JWT generado para el usuario
     */
    public string Token { get; set; } = string.Empty;
    
    /**
     * Objeto UserDto con la información básica del usuario autenticado
     */
    public UserDto User { get; set; } = new UserDto();
}