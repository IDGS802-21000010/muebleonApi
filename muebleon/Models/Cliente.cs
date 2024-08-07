using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public int Estatus { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
