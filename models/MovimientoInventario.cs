namespace WebApplication2.models
{
    public class MovimientoInventario
    {
        public int Id { get; set; }
        public int SemillaId { get; set; } // Referencia a la semilla
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; } // "Ingreso" o "Egreso"
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }
    }
}
