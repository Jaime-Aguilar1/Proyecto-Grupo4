namespace Proyecto_Grupo4.API.Models;

public class Reserva
{
    /**
     * Id: Identificador de la Reserva
     */

    public string Id { get; set; } = string.Empty;

    /**
     * Guarda el nombre del usuraio que hospeda
     */
    public string UserName { get; set; } = string.Empty;

    /**
     * Contacto del huesped
     */
    public string Contact { get; set; } = string.Empty;

    /**
     * Id usuario
     */
    public string UserId { get; set; } = string.Empty;

    /**
     * Contabilizar la cantidad de acompañante
     */
    public int NumPart { get; set; }

    /**
     * Identificar nombres de los acompañantes
     */
    public string NamePart { get; set; } = string.Empty;

    /**
     * Define la fecha de llegada
     */
    public DateTime ArrivalDate { get; set; }

    /**
     * Define la fecha de salida
     */
    public DateTime DepartureDate { get; set; }
    

    /**
   * Numero de la Habitacion
   */
    public string NumRoom { get; set; } = string.Empty;


    /**
  * Estado identificar si la habitaion esta Pendiente o reservada
  */
    public string State { get; set; } = string.Empty;

    /**
     * Define la fecha entrada y de registro de los huespedes en el hotel
     */
    public DateTime CheckIn { get; set; }

    /**
     * Define la fecha de  Salida de los huespedes en el hotel
     */
    public DateTime Checkout { get; set; }


    /**
     * Total a pagar mas ISV de la Reserva
     */
    public double TotalIsv { get; set; }
    
    /**
    * Pago para reservar
    */
    public double PayReserv { get; set; } 
    
    /**
    * Pago para hacer el CheckOut
    */
    public double PayFinish { get; set; } 

}