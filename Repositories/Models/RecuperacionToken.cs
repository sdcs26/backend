namespace Sowing_O2.Repositories.Models
{
    public class RecuperacionToken
    {
        public int Id { get; set; }
        public string Correo { get; set; }
        public string Token { get; set; }
        public DateTime FechaExp { get; set; }
    }
}
