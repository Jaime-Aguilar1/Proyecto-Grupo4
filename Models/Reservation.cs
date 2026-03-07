namespace Proyecto_Grupo4.API.Models;

public class Reservation

{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public int RoomNumber { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int Nights { get; set; } // Calculado en el backend [cite: 17]
    public decimal TotalConIsv { get; set; } // Incluye impuestos [cite: 5, 33]
    public string Estado { get; set; } = "confirmada"; // confirmada/pendiente [cite: 26]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}  
