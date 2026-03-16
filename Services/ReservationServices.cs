using Google.Cloud.Firestore;
using Proyecto_Grupo4.API.DTOs;
using Proyecto_Grupo4.API.Models;

namespace Proyecto_Grupo4.API.Services;

public class ReservationService : IReservationService
{
    private readonly FirestoreDb _firestoreDb;
    private const string UsersCollection = "Users";
    private const string RoomsCollection = "Rooms";
    private const string ReservationsCollection = "Reservations";

    public ReservationService(FirestoreDb firestoreDb)
    {
        _firestoreDb = firestoreDb;
    }

    // 1. Obtener todas las reservaciones (Útil para el reporte del Gerente)
    public async Task<List<ReservationDto>> GetReservations()
    {
        Query query = _firestoreDb.Collection(ReservationsCollection);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        
        return snapshot.Documents.Select(doc => doc.ConvertTo<ReservationDto>()).ToList();
    }

    // 2. Obtener una reservación específica
    public async Task<ReservationDto?> GetReservationById(string reservationId)
    {
        DocumentReference docRef = _firestoreDb.Collection(ReservationsCollection).Document(reservationId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        
        return snapshot.Exists ? snapshot.ConvertTo<ReservationDto>() : null;
    }

    // 3. VALIDACIÓN SOLICITADA: Basada en la colección de Reservas y el Status
    public async Task<bool> HasUserReservation(string userId)
    {
        // Buscamos reservas del usuario que no estén en "Check-out" o "Cancelada"
        Query query = _firestoreDb.Collection(ReservationsCollection)
            .WhereEqualTo("UserId", userId);
            
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Si existe alguna reserva con estado "Reserv" o "Check-in", el usuario está ocupado
        return snapshot.Documents.Any(d => 
            d.GetValue<string>("Status") == "Reserv" || 
            d.GetValue<string>("Status") == "Check-in");
    }

    // 4. Obtener la reserva actual para la pantalla "Ya Reservó"
    public async Task<ExistReservationDto> GetUserReservation(string userId)
    {
        Query query = _firestoreDb.Collection(ReservationsCollection)
            .WhereEqualTo("UserId", userId)
            .Limit(1); // Obtenemos la más reciente activa

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Documents.Count == 0) 
            return new ExistReservationDto { HasReservation = false };

        var doc = snapshot.Documents[0];
        return new ExistReservationDto
        {
            HasReservation = true,
            NumRoom = doc.GetValue<string>("NumRoom"),
            CheckIn = doc.GetValue<DateTime>("CheckIn"),
            CheckOut = doc.GetValue<DateTime>("CheckOut"),
            Status = doc.GetValue<string>("Status")
        };
    }

    // 5. Cálculo previo a la reserva (Requerimiento 3.49)
    public async Task<BillReservationDto> CalculateReservationCost(string roomId, DateTime checkIn, DateTime checkOut)
    {
        DocumentSnapshot roomSnap = await _firestoreDb.Collection(RoomsCollection).Document(roomId).GetSnapshotAsync();
        
        if (!roomSnap.Exists) throw new Exception("Habitación no encontrada");

        // Obtenemos tarifaBase de la colección Rooms (PDF 101)
        double nightPrice = roomSnap.GetValue<double>("tarifaBase"); 
        int nights = (checkOut - checkIn).Days;
        if (nights <= 0) nights = 1;

        double subTotal = nightPrice * nights;
        double isv = subTotal * 0.15;
        double total = subTotal + isv;

        return new BillReservationDto
        {
            NumRoom = roomSnap.GetValue<string>("numero"),
            NumNight = nights,
            NightPrice = nightPrice,
            SubTotal = subTotal,
            ISV = isv,
            TotalIsv = total,
            PayReserv = total * 0.50, // 50% para confirmar (lógica de negocio)
            TotalPayReserv = total
        };
    }

    // 6. Crear nueva reserva con Transacción Atómica (Requerimiento 3.47, 126)
    public async Task<ConfirmReservationDto> CreateReservation(CreateReservationDto dto, string userId)
    {
        return await _firestoreDb.RunTransactionAsync(async transaction =>
        {
            // Verificamos si ya existe reserva activa para evitar doble reservación
            bool alreadyHas = await HasUserReservation(userId);
            if (alreadyHas) throw new Exception("Ya tienes una reserva activa.");

            string resId = Guid.NewGuid().ToString();
            DocumentReference resRef = _firestoreDb.Collection(ReservationsCollection).Document(resId);
            DocumentReference userRef = _firestoreDb.Collection(UsersCollection).Document(userId);

            var reservation = new Reservation
            {
                Id = resId,
                UserId = userId,
                UserName = dto.UserName,
                RoomId = dto.RoomId,
                NumRoom = dto.NumRoom,
                TypeRoom = dto.TypeRoom,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                NumNight = dto.NumNight,
                Status = "Reserv", // Usando tu estado del DTO
                CreateReserv = DateTime.UtcNow
            };

            transaction.Set(resRef, reservation);
            
            [cite_start]// Actualizamos el usuario para cumplir con el PDF (hasReserved=true) [cite: 99]
            transaction.Update(userRef, new Dictionary<string, object>
            {
                { "hasReserved", true },
                { "reservedRoom", dto.NumRoom }
            });

            return new ConfirmReservationDto
            {
                ReservationId = resId,
                NumRoom = dto.NumRoom,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                Status = "Reserv"
            };
        });
    }

    // 7. Actualización de estado (Requerimiento 42)
    public async Task UpdateReservationStatus(string reservationId, string status)
    {
        DocumentReference docRef = _firestoreDb.Collection(ReservationsCollection).Document(reservationId);
        await docRef.UpdateAsync("Status", status);
    }

    // 8. Cancelar Reservación
    public async Task DeleteReservation(string reservationId, string userId)
    {
        // En Firestore, el borrado debe ser cuidadoso. 
        // Primero actualizamos al usuario para que pueda volver a reservar.
        DocumentReference userRef = _firestoreDb.Collection(UsersCollection).Document(userId);
        await userRef.UpdateAsync("hasReserved", false);
        
        // Luego borramos la reserva
        await _firestoreDb.Collection(ReservationsCollection).Document(reservationId).DeleteAsync();
    }
}
