using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdMateriaPrima { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual MateriaPrima IdMateriaPrimaNavigation { get; set; } = null!;
}
