namespace Proyecto_Grupo4.API.DTOs;

public class UserDto

{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
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

public class RoomDto 
{
    public string Id { get; set; } = string.Empty;
    public int NumeroHabitacion { get; set; } // 1 al 25 
    public string Tipo { get; set; } // Sencilla, Doble, etc. 
    public int Capacidad { get; set; }
    public string Amenidades { get; set; } // Ej: "King, Cocineta, Balcón" 
    public decimal Tarifa { get; set; }
    public string Estado { get; set; } = "libre"; // libre/reservado 
}
