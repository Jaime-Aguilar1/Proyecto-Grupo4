namespace Proyecto_Grupo4.API.DTOs;

public class UserDto

{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Passw { get; set; } = string.Empty;
    public string Role { get; set; } = "huésped"; // "gerente" o "huésped" [cite: 54, 136]
    public bool HasReserved { get; set; } // Requerimiento: saber si ya tiene reserva [cite: 137]
}

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Contacto { get; set; } = string.Empty; // Celular/Email [cite: 8]
    public string NumeroId { get; set; } = string.Empty; // Identidad [cite: 8]
}
