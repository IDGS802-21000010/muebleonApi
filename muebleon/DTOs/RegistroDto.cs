using System.ComponentModel.DataAnnotations;

namespace muebleon.DTOs
{
    public class RegistroDto
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }
    }
}
