using Microsoft.AspNetCore.Mvc;
using Proyecto_Grupo4.API.DTOs;
using Proyecto_Grupo4.API.Services;

namespace Proyecto_Grupo4.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    // GET: api/reservations
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetAll()
    {
        var reservations = await _reservationService.GetReservations();
        return Ok(reservations);
    }

    // GET: api/reservations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetById(string id)
    {
        var reservation = await _reservationService.GetReservationById(id);
        if (reservation == null) return NotFound("La reservación no existe.");
        
        return Ok(reservation);
    }

    // GET: api/reservations/user/{userId}/exists
    [cite_start]// REQUERIMIENTO: Validar si el usuario ya reservó [cite: 51, 122]
    [HttpGet("user/{userId}/exists")]
    public async Task<ActionResult<ExistReservationDto>> CheckExists(string userId)
    {
        var result = await _reservationService.GetUserReservation(userId);
        return Ok(result);
    }

    // POST: api/reservations/calculate
    [cite_start]// REQUERIMIENTO: Visualizar costo total con impuestos [cite: 49]
    [HttpPost("calculate")]
    public async Task<ActionResult<BillReservationDto>> Calculate([FromBody] CalculateRequest request)
    {
        try 
        {
            var bill = await _reservationService.CalculateReservationCost(
                request.RoomId, request.CheckIn, request.CheckOut);
            return Ok(bill);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/reservations
    [cite_start]// REQUERIMIENTO: Confirmación exitosa y validación única [cite: 48, 52]
    [HttpPost]
    public async Task<ActionResult<ConfirmReservationDto>> Create([FromBody] CreateReservationDto dto)
    {
        try
        {
            [cite_start]// El servicio ya maneja la transacción atómica y la validación de doble reserva [cite: 125, 126]
            var result = await _reservationService.CreateReservation(dto, dto.UserId);
            return CreatedAtAction(nameof(GetById), new { id = result.ReservationId }, result);
        }
        catch (Exception ex)
        {
            [cite_start]// Manejo de error si el usuario ya tiene reserva (Escenario 2) [cite: 176, 179]
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/reservations/{id}/user/{userId}
    [HttpDelete("{id}/user/{userId}")]
    public async Task<IActionResult> Delete(string id, string userId)
    {
        await _reservationService.DeleteReservation(id, userId);
        return NoContent();
    }
}

// Clase auxiliar para la petición de cálculo
public class CalculateRequest
{
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}
