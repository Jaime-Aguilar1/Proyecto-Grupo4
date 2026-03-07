using System;
using System.Collections.Generic;
using FirebaseAdmin.Firestore;
using Google.Cloud.Firestore;

namespace Proyecto.API.Models
{
   
    [FirestoreData]
    public class Habitacion
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Numero { get; set; }

        [FirestoreProperty]
        public string Tipo { get; set; } 

        [FirestoreProperty]
        public int Capacidad { get; set; }

        [FirestoreProperty]
        public decimal TarifaBase { get; set; }

        [FirestoreProperty]
        public List<string> Amenidades { get; set; }

        [FirestoreProperty]
        public string UrlFoto { get; set; }

        [FirestoreProperty]
        public int ContadorReservas { get; set; }

        [FirestoreProperty]
        public DateTime FechaCreacion { get; set; }

        [FirestoreProperty]
        public string CreadoPor { get; set; }
    }
}