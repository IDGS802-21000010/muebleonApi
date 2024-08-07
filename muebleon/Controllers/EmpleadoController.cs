using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public EmpleadoController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("empleado/{estatus}")]
        public async Task<IActionResult> ListaEmpleados(int estatus)
        {
            var listaEmpleados = await _baseDatos.Empleados
                                            .Include(e => e.IdUsuarioNavigation)
                                            .Where(e => e.Estatus == estatus)
                                            .Select(e => new
                                            {
                                                e.IdEmpleado,
                                                e.Nombre,
                                                NombreUsuario = e.IdUsuarioNavigation.Usuario1,
                                                Contrasenia = e.IdUsuarioNavigation.Contrasenia,
                                                Rol = e.IdUsuarioNavigation.Rol
                                            })
                                            .ToListAsync();
            return Ok(listaEmpleados);
        }

        [HttpPost]
        [Route("registroEmpleado")]
        public IActionResult RegistrarEmpleado([FromBody] RegistroDto registroDto)
        {
            if (_baseDatos.Usuarios.Any(u => u.Usuario1 == registroDto.Usuario))
            {
                return BadRequest("El nombre de usuario ya existe");
            }

            var usuario = new Usuario
            {
                Usuario1 = registroDto.Usuario,
                Contrasenia = registroDto.Contrasenia,
                Rol = "admin"
            };

            _baseDatos.Usuarios.Add(usuario);
            _baseDatos.SaveChanges();

            var empleado = new Empleado
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = registroDto.Nombre,
                Estatus = 1
            };

            _baseDatos.Empleados.Add(empleado);
            _baseDatos.SaveChanges();

            return Ok(new
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombre = empleado.Nombre,
                Usuario = usuario.Usuario1
            });
        }

        [HttpPut]
        [Route("estatusEmpleado/{id:int}")]
        public async Task<IActionResult> estatusEmpleado(int id)
        {
            var estatusEmpleado = await _baseDatos.Empleados.FindAsync(id);
            if (estatusEmpleado == null)
            {
                return BadRequest("No existe el Empleado.");
            }

            if (estatusEmpleado.Estatus == 1)
            {
                estatusEmpleado.Estatus = 0;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Empleado Eliminado");
            }
            else if (estatusEmpleado.Estatus == 0)
            {
                estatusEmpleado.Estatus = 1;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Empleado Restaurado");
            }
            else
            {
                return BadRequest("Estatus de empleado no disponible");
            }
        }

        [HttpPut]
        [Route("actualizarEmpleado/{id:int}")]
        public async Task<IActionResult> actualizarEmpleado(int id, [FromBody] EmpleadoDto empleadoDto)
        {
            var empleado = await _baseDatos.Empleados.Include(e => e.IdUsuarioNavigation)
                                                      .FirstOrDefaultAsync(e => e.IdEmpleado == id);
            if (empleado == null)
            {
                return BadRequest("Empleado no encontrado.");
            }

            // Empleado
            empleado.Nombre = empleadoDto.Nombre;

            // Usuario
            var usuario = empleado.IdUsuarioNavigation;
            usuario.Usuario1 = empleadoDto.Usuario;
            usuario.Contrasenia = empleadoDto.Contrasenia;
            usuario.Rol = empleadoDto.Rol;

            try
            {
                await _baseDatos.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok("Empleado Editado");
        }


    }
}
