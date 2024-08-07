namespace muebleon.DTOs
{
    public class CompraDto
    {

        public DateOnly Fecha { get; set; }
        public int IdEmpleado { get; set; }
        public int IdProveedor { get; set; }
        public List<DetalleCompraDto> Detalles { get; set; }
    }
}
