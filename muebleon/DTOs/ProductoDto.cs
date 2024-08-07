namespace muebleon.DTOs
{
    public class ProductoDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Estatus { get; set; }
        public List<DetalleConsumoDto> DetallesConsumo { get; set; }
    }
}
