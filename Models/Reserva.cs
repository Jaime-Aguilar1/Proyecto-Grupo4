using System;
using Google.Cloud.Firestore;
using static FirebaseAdmin.FirebaseApp;

namespace Proyecto.API.Models
{

    [FirestoreData]
    public class Reserva
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string IdUsuario { get; set; }

        [FirestoreProperty]
        public string NombreUsuario { get; set; }

        [FirestoreProperty]
        public string IdHabitacion { get; set; }

        [FirestoreProperty]
        public string NumeroHabitacion { get; set; }

        [FirestoreProperty]
        public string TipoHabitacion { get; set; }

        [FirestoreProperty]
        public DateTime FechaCheckIn { get; set; }

        [FirestoreProperty]
        public DateTime FechaCheckOut { get; set; }

        [FirestoreProperty]
        public int Noches { get; set; }

        [FirestoreProperty]
        public decimal CostoTotal { get; set; }

        [FirestoreProperty]
        public DateTime Timestamp { get; set; }
    }
}