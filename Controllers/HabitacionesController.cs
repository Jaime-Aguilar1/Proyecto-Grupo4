using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.API.Models;
using Proyecto.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de habitaciones (Solo Gerentes)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Gerente")]
    public class HabitacionesController : ControllerBase
    {
        private readonly HabitacionService _habitacionService;

        public HabitacionesController(HabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Habitacion>>> ObtenerTodas()
        {
            var habitaciones = await _habitacionService.ObtenerTodasAsync();
            return Ok(habitaciones);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Habitacion>> ObtenerPorId(string id)
        {
            var habitacion = await _habitacionService.ObtenerPorIdAsync(id);
            if (habitacion == null)
            {
                return NotFound("Habitación no encontrada.");
            }
            return Ok(habitacion);
        }

   
        [HttpPost]
        public async Task<ActionResult<Habitacion>> CrearHabitacion(Habitacion habitacion)
        {
            
            var existente = await _habitacionService.ObtenerPorNumeroAsync(habitacion.Numero);
            if (existente != null)
            {
                return BadRequest("El número de habitación ya existe.");
            }

            var habitacionCreada = await _habitacionService.CrearHabitacionAsync(habitacion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = habitacionCreada.Id }, habitacionCreada);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarHabitacion(string id, Habitacion habitacion)
        {
            if (id != habitacion.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo de la solicitud.");
            }

            await _habitacionService.ActualizarHabitacionAsync(id, habitacion);
            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarHabitacion(string id)
        {
            
            var tieneReservas = await _habitacionService.TieneReservasAsync(id);
            
            if (tieneReservas)
            {
                return BadRequest("No se puede eliminar la habitación. Existen reservas asociadas.");
            }

            await _habitacionService.EliminarHabitacionAsync(id);
            return NoContent();
        }
    }
}