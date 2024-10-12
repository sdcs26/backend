namespace WebApplication2.models
{
    public class Semilla
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int CantidadEnInventario { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaDeIngreso { get; set; }
        public string Ubicacion { get; set; }
    }
}
