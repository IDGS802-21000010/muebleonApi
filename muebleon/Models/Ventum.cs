using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public int IdPedido { get; set; }

    public DateOnly Fecha { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
