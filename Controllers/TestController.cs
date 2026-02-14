using Microsoft.AspNetCore.Mvc;

namespace Proyecto.API.Controllers;


    /*
 * Controller de prueba para verificar que firebase funciona correctamente
 * Una vez confirmado se puede eliminar
 */
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
   private readonly FirebaseService _firebaseService;
   private readonly ILogger<TestController> _logger;
   
   /*
    * Constructor: ASP.NET Core Inyecta FirebaseService automaticamente
    * Porque necesitamos registrarlo en el program
    */

   public TestController(FirebaseService firebaseService, ILogger<TestController> logger)
   {
       _firebaseService = firebaseService;
       _logger = logger;
   }
   
   /*
    * Endepoint de prueba: GET/api/test/firebase
    * Intenta conectarse a Firebase y devuelve una respuesta
    */
   [HttpGet("firebase")]
   public async Task<IActionResult> TestFirebase()
   {
       try
       {
           _logger.LogInformation("Iniciando prueba se conexion Firebase");
           //Obtener la coleccion "test" (La creamos despues)
           var testColection= _firebaseService.GetCollection("Test");
           
           //Intentar leer el documento
           var snapshot = await testColection.GetSnapshotAsync();
           
           //si la conexion funciona 
           return Ok(new
           {
               success = true,
               mesage = "Conexion Exitosa",
               documentInTestCollection = snapshot.Count,
               Timestamp = DateTime.UtcNow
           });

       }
       catch (Exception e)
       {
           _logger.LogError($"Error en prueba: {e.Message}");
           return StatusCode(500, new
           {
               Success = false,
               mesage = "No conexion en el servidor",
               error = e.Message
           });
       }
   }
    /*
     * Endpoint de prueba simple: GET /api/test/health
     * Valida si la API esta corriendo
     */
 
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok( new
        {
            status = "API Corriendo",
            timestamp = DateTime.UtcNow
        });
    }
   
    }

