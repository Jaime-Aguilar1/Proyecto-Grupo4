using Google.Cloud.Firestore;

namespace Proyecto_Grupo4.API.Models
{
   
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Correo { get; set; }

        [FirestoreProperty]
        public string Nombre { get; set; }
       
        [FirestoreProperty]
        public string Costrasena { get; set; }

        [FirestoreProperty]
        public string Rol { get; set; } 

        [FirestoreProperty]
        public DateTime FechaCreacion { get; set; }
    }
}
