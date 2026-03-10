namespace Proyecto_Grupo4.API.Models;

public class Reservation
{
    /**
     * Id: Identificador de la Reserva
     */

    public string Id { get; set; } = string.Empty;
    
    /**
    * Id usuario
    */
    public string UserId { get; set; } = string.Empty;
    
    /**
     * Guarda el nombre del usuraio que hospeda
     */
    public string UserName { get; set; } = string.Empty;

    /**
     * Contacto del huesped
     */
    public string UserContact { get; set; } = string.Empty;
    
    /**
     * Contabilizar la cantidad de acompañante
     */
    public int NumPart { get; set; }

    /**
     * Identificar nombres de los acompañantes
     */
    public List<string> NamePart { get; set; } = new List<string>();
    
    /**
    * Id Room
    */
    public string RoomId { get; set; } = string.Empty;
    

    /**
   * Número de la Habitacion
   */
    public string NumRoom { get; set; } = string.Empty;

    /**
     * Define la fecha entrada y de registro de los huespedes en el hotel
     */
    public DateTime CheckIn { get; set; }

    /**
     * Define la fecha de  Salida de los huespedes en el hotel
     */
    public DateTime CheckOut { get; set; }
    
    /**
    * Estado identificar si la habitaion esta Pendiente, Reservada, Check-in, Check-out
    */
    public string Status { get; set; } = string.Empty;


    /**
     * Total a pagar mas ISV de la Reserva
     */
    public double TotalIsv { get; set; }
    
    /**
    * Pago para reservar
    */
    public double PayReserv { get; set; } 
    
    /*
     * Fecha de creación de la reserva
     */ 
    public DateTime CreateReserv { get; set; }
    

}