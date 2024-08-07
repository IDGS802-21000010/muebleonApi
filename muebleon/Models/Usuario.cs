using System;
using System.Collections.Generic;

namespace muebleon.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Rol { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
