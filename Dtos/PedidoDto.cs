namespace Sowing_O2.Dtos
{
    public class PedidoDTO
    {
        public string NumeroPedido { get; set; }
        public int EstadoId { get; set; }
        public string NotasEnvio { get; set; }
        public List<PedidoDetalleDTO> Detalles { get; set; }
    }

    public class PedidoDetalleDTO
    {
        public int SemillaId { get; set; }
        public int Cantidad { get; set; }
    }
}
