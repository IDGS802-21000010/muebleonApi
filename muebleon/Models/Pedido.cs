using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdCliente { get; set; }

    public int? IdEmpleado { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Total { get; set; }

    public string Estatus { get; set; } = null!;

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
