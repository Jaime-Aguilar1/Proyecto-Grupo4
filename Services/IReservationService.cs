using Proyecto_Grupo4.API.DTOs;
using Proyecto_Grupo4.API.Models;

namespace Proyecto_Grupo4.API.Services;

public interface IReservationService
{
    
    /*
     * Lista de todas las reservas
     */
    Task<List<ReservationDto>> GetReservations();

    /*
     * Ver la reserva del usuario
     */
    Task<ExistReservationDto> GetUserReservation(string userId);

    /*
     * Ver la factura del la reserva
     */
    Task<BillReservationDto> CalculateReservationCost(
        string roomId,
        DateTime checkIn,
        DateTime checkOut
    );

    /*
     * Crear la reserva
     */
    Task<Reservation> CreateReservation(
        CreateReservationDto createReservationDto,
        string userId
    );

   
}
