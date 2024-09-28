namespace Sowing_O2.Dtos
{
    public class UsuarioDto
    {
        public int NumDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
    }
    public class LoginDto
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}
