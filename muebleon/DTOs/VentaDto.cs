namespace muebleon.DTOs
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public DateOnly Fecha { get; set; }
        public int IdPedido { get; set; }
        public decimal Total { get; set; }
        public PedidoDto Pedido { get; set; }
    }
}
