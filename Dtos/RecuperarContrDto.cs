namespace Sowing_O2.Dtos
{
    public class RecuperarContrasenaDto
    {
        public string Correo { get; set; }
    }

    public class ConfirmarRecuperarContrasenaDto
    {
        public string Token { get; set; }
        public string NuevaContrasena { get; set; }
    }
}
