using System.ComponentModel.DataAnnotations;

public class MovimientoInventario
{
    public int Id { get; set; }

    public int SemillaId { get; set; }

    public DateTime FechaMovimiento { get; set; }

    [Required] // Asegura que el tipo de movimiento no sea nulo
    [StringLength(10)] // Limita el tamaño del tipo de movimiento
    public string TipoMovimiento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser positiva")] // Valida que la cantidad sea mayor que 0
    public int Cantidad { get; set; }

    public string Observaciones { get; set; }
}

