﻿namespace muebleon.DTOs
{
    public class DetallePedidoDto
    {
        public int IdDetallePedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
