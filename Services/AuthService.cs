using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Grupo4.API.DTOs;
using Proyecto.API.Models; 
using Proyecto; 

namespace Proyecto_Grupo4.API.Services;


public class AuthService : IAuthService 
{
    private readonly IConfiguration _configuration;
    private readonly FirebaseService _firebaseService;
    private readonly CollectionReference _usuariosCollection;

    public AuthService(FirebaseService firebaseService, IConfiguration configuration)
    {
        _firebaseService = firebaseService;
        _configuration = configuration;
        // Obtenemos la colección usando tu método del FirebaseService
        _usuariosCollection = _firebaseService.GetCollection("Usuarios");
    }

    public async Task<Usuario> Register(RegisterDto registerDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
            {
                throw new ArgumentException("Email y password son requeridos");
            }

            // 1. Verificar si el correo ya existe
            var query = await _usuariosCollection
                .WhereEqualTo("Correo", registerDto.Email)
                .GetSnapshotAsync();

            if (query.Count > 0)
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            // 2. Crear el objeto Usuario adaptado a tu modelo Firestore
            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = registerDto.FullName,
                Correo = registerDto.Email,
                Costrasena = registerDto.Password, // Respetando el nombre de tu modelo
                Rol = "huésped", // Rol por defecto para el hotel
                FechaCreacion = DateTime.UtcNow
            };

            // 3. Guardar en Firestore
            await _usuariosCollection.Document(nuevoUsuario.Id).SetAsync(nuevoUsuario);

            return nuevoUsuario;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al registrar usuario: {e.Message}");
            throw;
        }
    }

    public async Task<(UserDto user, string token)> Login(LoginDto loginDto)
    {
        try
        {
            // 1. Buscar usuario por Correo
            var query = await _usuariosCollection
                .WhereEqualTo("Correo", loginDto.Email)
                .GetSnapshotAsync();

            if (query.Count == 0)
            {
                throw new InvalidOperationException("Credenciales incorrectas");
            }

            var userDoc = query.Documents[0];
            var usuario = userDoc.ConvertTo<Usuario>();

            // 2. Validar contraseña
            if (usuario.Costrasena != loginDto.Password)
            {
                throw new InvalidOperationException("Credenciales incorrectas");
            }

            // 3. Generar el token
            var token = GenerateJwtToken(usuario);

            // 4. Mapear a UserDto para la respuesta
            var userDto = new UserDto
            {
                Id = userDoc.Id,
                FullName = usuario.Nombre,
                Email = usuario.Correo,
                Role = usuario.Rol,
                
            };

            return (userDto, token);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error en login: {e.Message}");
            throw;
        }
    }

    public async Task<bool> ValidateToken(string token)
    {
        try
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey)) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Usuario?> GetUserById(string userId)
    {
        var doc = await _usuariosCollection.Document(userId).GetSnapshotAsync();
        return doc.Exists ? doc.ConvertTo<Usuario>() : null;
    }

    public string GenerateJwtToken(Usuario user)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(secretKey))
            throw new InvalidOperationException("JWT SecretKey no configurado en appsettings.json");

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id),
                new Claim("email", user.Correo),
                new Claim("name", user.Nombre),
                new Claim("role", user.Rol)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}