using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.API.Models;
using Proyecto.API.Services;

namespace Proyecto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")] 
    public class RoomsController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomsController(RoomService roomService)
        {
            _roomService = roomService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        
        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            
            var existing = await _roomService.GetByNumberAsync(room.Number);
            if (existing != null)
            {
                return BadRequest("El número de habitación ya existe.");
            }

            var createdRoom = await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetById(string id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(string id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest("ID de habitación no coincide.");
            }

            await _roomService.UpdateRoomAsync(id, room);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            
            var hasReservations = await _roomService.HasReservationsAsync(id);
            if (hasReservations)
            {
                return BadRequest("No se puede eliminar la habitación. Existen reservas asociadas.");
            }

            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}