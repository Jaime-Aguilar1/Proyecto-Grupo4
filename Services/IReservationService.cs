using Proyecto_Grupo4.API.DTOs;
using Proyecto_Grupo4.API.Models;

namespace Proyecto_Grupo4.API.Services;

public interface IReservationService
{ 
    /*
     * Obtener todas las reservaciones
     */
    Task<List<ReservationDto>> GetReservations();

    /*
     * Obtener una reservacion especifica por Id
     */
    Task<ReservationDto?> GetReservationById(string reservationId);

    /*
     * Verificar si el usuario ya tiene una reservacion
     */
    Task<bool> HasUserReservation(string userId);

    /*
     * Obtener la reservacion actual del usuario
     */
    Task<ExistReservationDto> GetUserReservation(string userId);

    /*
     * Calcular el costo de la reservacion antes de crearla
     */
    Task<BillReservationDto> CalculateReservationCost(
        string roomId,
        DateTime checkIn,
        DateTime checkOut
    );

    /*
     * Crear una nueva reservacion
     */
    Task<ConfirmReservationDto> CreateReservation(
        CreateReservationDto createReservationDto,
        string userId
    );

    /*
     * Actualizar estado de la reservacion
     */
    Task UpdateReservationStatus(string reservationId, string status);

    /*
     * Cancelar una reservacion
     */
    Task DeleteReservation(string reservationId, string userId);
}
    
    
