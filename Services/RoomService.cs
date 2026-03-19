using FirebaseAdmin.Auth;
using Proyecto.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.API.Services
{

    public class HabitacionService<FirebaseFirestore>
    {
        private readonly FirebaseFirestore _firestore;

        public HabitacionService(FirebaseFirestore firestore)
        {
            _firestore = firestore;
        }


        public async Task<IEnumerable<Habitacion>> ObtenerTodasAsync()
        {
            var snapshot = await _firestore.Collection("Habitaciones").GetSnapshotAsync();
            return snapshot.Documents.Select(d => d.ToObject<Habitacion>());
        }


        public async Task<Habitacion> ObtenerPorIdAsync(string id)
        {
            var doc = await _firestore.Collection("Habitaciones").Document(id).GetSnapshotAsync();
            return doc.Exists ? doc.ToObject<Habitacion>() : null;
        }


        public async Task<Habitacion> ObtenerPorNumeroAsync(string numero)
        {
            var snapshot = await _firestore
                .Collection("Habitaciones")
                .WhereEqualTo("Numero", numero)
                .GetSnapshotAsync();
            
            return snapshot.Documents.FirstOrDefault()?.ToObject<Habitacion>();
        }


        public async Task<Habitacion> CrearHabitacionAsync(Habitacion habitacion)
        {
            habitacion.Id = System.Guid.NewGuid().ToString();
            habitacion.FechaCreacion = System.DateTime.UtcNow;
            habitacion.ContadorReservas = 0;

            await _firestore.Collection("Habitaciones").Document(habitacion.Id).SetAsync(habitacion);
            return habitacion;
        }


        public async Task ActualizarHabitacionAsync(string id, Habitacion habitacion)
        {
            habitacion.Id = id;
            await _firestore.Collection("Habitaciones").Document(id).SetAsync(habitacion);
        }


        public async Task EliminarHabitacionAsync(string id)
        {
            await _firestore.Collection("Habitaciones").Document(id).DeleteAsync();
        }

       
        public async Task<bool> TieneReservasAsync(string idHabitacion)
        {
            var snapshot = await _firestore
                .Collection("Reservas")
                .WhereEqualTo("IdHabitacion", idHabitacion)
                .GetSnapshotAsync();
            
            return snapshot.Documents.Count > 0;
        }
    }
}
