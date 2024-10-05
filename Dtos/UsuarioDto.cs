namespace Sowing_O2.Dtos
{
    public class UsuarioDto
    {
        public int NumDocumento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdRol { get; set; }
    }

    public class LoginDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TiempoExpiracion { get; set; }
    }
}
