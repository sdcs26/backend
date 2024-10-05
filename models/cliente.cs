namespace WebApplication2.Models
{
    public class cliente
    {
        public string nombre { get; set; }
        public string id { get; set; }
        public string edad { get; set; }
        public string correo { get; set; }

        // Nueva propiedad para almacenar el hash de la contraseña
        public string PasswordHash { get; set; }
    }
}

