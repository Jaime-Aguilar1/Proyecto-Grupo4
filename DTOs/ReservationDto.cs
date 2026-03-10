namespace Proyecto_Grupo4.API.DTOs;

public class ReservationDto
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
    }

public class CreateReservationDto
{
    /**
   * Identificar que usuario hizo la reserva
   */
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserContact { get; set; } = string.Empty; 
    
    /*
     * Identificar que habitacion se reserva
     */
    public string RoomId { get; set; } = string.Empty;
    public string NumRoom { get; set; } = string.Empty;
    
    /*
     * Establecer fechas de la reserva
     */
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
    /*
     * Establecer los acompañantes del huesped
     */
    public int NumPart { get; set; }
    public List<string> NamePart { get; set; } = new List<string>();
}

public class ConfirmReservationDto
{
    /**
     * Id: Identificador de la Reserva
     */
    public string ReservationId { get; set; } = string.Empty;
    
    /*
     * Visualizar que habitacion se reservo
     */
    public string NumRoom { get; set; } = string.Empty;
    
    /*
     * Visualizar que fechas se reservo
     */
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
    /**
     * Total a pagar mas ISV de la Reserva
     */
    public double TotalIsv { get; set; }
    
    /**
    * Pago para reservar
    */
    public double PayReserv { get; set; }
    
    /**
    * Estado identificar si la habitaion esta Pendiente, Reservada, Check-in, Check-out
    */
    public string Status { get; set; } = string.Empty;
}

public class ExistReservationDto
{
    /*
    * Valida si el usuario ya tiene una reservacion y devuelve un True o False 
    */
    public bool HasReservation { get; set; }
    
    /*
     * Valida que habitacion se reservo
     */
    public string NumRoom { get; set; } = string.Empty;
    
    /*
     * Valida que fechas reservo se reservo
     */
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    
    /*
    * Válida el estado de la reserva
    */
    public string Status { get; set; } = string.Empty;
}

public  class BillReservationDto
{
    /*
     * Numero de la habitacion a la que se carga la factura
     * Ayuda a confirmar nuevamente si es su habitacion
     */
    public string NumRoom { get; set; } = string.Empty;
    
    /*
     * Cantidad de noches reservadas
     */
    public int NightsReserv { get; set; }
    
    /*
     * Precio base por noche
     */
    public double NightPrice { get; set; }

    /*
     * Multiplicacion de la cantidad de noches por el precio base
     */
    public double SubTotal { get; set; }
    
    /*
     * Este sera el 15%
     */
    public double ISV { get; set; }

    /*
     * Total a pagar mas ISV de la Reserva
     */
    public double TotalIsv { get; set; }

    /*
     * Pago para reservar
     */
    public double PayReserv { get; set; } 
}