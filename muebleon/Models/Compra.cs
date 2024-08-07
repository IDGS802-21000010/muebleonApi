using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public int IdEmpleado { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
