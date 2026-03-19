

using Proyecto_Grupo4.API.DTOs;
using Proyecto_Grupo4.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Grupo4.API.Controllers
{
    /// AuthController maneja la autenticación del sistema del Hotel
    /// Endpoints para registro de huéspedes e inicio de sesión
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;


        /// Constructor: Recibe IAuthService inyectado desde Program.cs
   
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

   
        /// POST /api/auth/register
        /// Registra un nuevo huésped en el sistema

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto? registerDto)
        {
            try
            {
                if (registerDto == null)
                {
                    return BadRequest(new { message = "El cuerpo de la petición es requerido" });
                }

                if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return BadRequest(new { message = "Email y contraseña son requeridos" });
                }

                // El servicio ya devuelve el objeto Usuario mapeado correctamente
                var usuarioCreado = await _authService.Register(registerDto);

                _logger.LogInformation($"Nuevo usuario registrado en el hotel: {usuarioCreado.Correo}");

                // Devolvemos 201 Created con el usuario
                return Created($"/api/auth/users/{usuarioCreado.Id}", usuarioCreado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en registro de hotel: {ex.Message}");
                return StatusCode(500, new { message = "Error interno al registrar el usuario" });
            }
        }


        /// POST /api/auth/login
        /// Inicia sesión y devuelve un token JWT
 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto? loginDto)
        {
            try
            {
                if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email))
                {
                    return BadRequest(new { message = "Credenciales incompletas" });
                }

                // El servicio devuelve el UserDto (ya filtrado) y el token generado
                var (userDto, token) = await _authService.Login(loginDto);

                _logger.LogInformation($"Sesión iniciada: {userDto.Email}");

                var response = new AuthResponseDto
                {
                    Success = true,
                    Message = "Bienvenido al sistema del Hotel",
                    Token = token,
                    User = userDto
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en login: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error al procesar el ingreso" });
            }
        }


        /// GET /api/auth/users/{userId}
        /// Obtiene el perfil de un usuario/huésped por ID
  
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new { message = "El ID es requerido" });
                }

                var usuario = await _authService.GetUserById(userId);

                if (usuario == null)
                {
                    return NotFound(new { message = "Huésped no encontrado" });
                }

                // Mapeamos manualmente a UserDto para no exponer la contraseña (Costrasena)
                var userDto = new UserDto
                {
                    Id = usuario.Id,
                    FullName = usuario.Nombre,
                    Email = usuario.Correo,
                    Role = usuario.Rol
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al buscar huésped: {ex.Message}");
                return StatusCode(500, new { message = "Error al obtener los datos" });
            }
        }
    }
}
    
