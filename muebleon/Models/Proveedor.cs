using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public int Estatus { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
