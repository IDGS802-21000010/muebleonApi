using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class DetalleConsumo
{
    public int IdDetalleConsumo { get; set; }

    public int IdProducto { get; set; }

    public int IdMateriaPrima { get; set; }

    public int Cantidad { get; set; }

    public virtual MateriaPrima IdMateriaPrimaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
