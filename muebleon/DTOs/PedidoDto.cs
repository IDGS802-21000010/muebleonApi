namespace muebleon.DTOs
{
    public class PedidoDto
    {
        public int idPedido { get; set; }
        public int idCliente {  get; set; }
        public int? idEmpleado { get; set; }
        public DateOnly Fecha {  get; set; }
        public decimal Total {  get; set; }
        public string Estatus { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; }
    }
}
