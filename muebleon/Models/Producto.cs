using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public int Estatus { get; set; }

    public virtual ICollection<DetalleConsumo> DetalleConsumos { get; set; } = new List<DetalleConsumo>();

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
