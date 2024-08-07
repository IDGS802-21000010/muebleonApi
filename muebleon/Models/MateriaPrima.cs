using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class MateriaPrima
{
    public int IdMateriaPrima { get; set; }

    public string Nombre { get; set; } = null!;

    public int Cantidad { get; set; }

    public int Estatus { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleConsumo> DetalleConsumos { get; set; } = new List<DetalleConsumo>();
}
