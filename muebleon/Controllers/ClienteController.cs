using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public ClienteController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("cliente/{estatus}")]
        public async Task<IActionResult> ListaClientes(int estatus)
        {
            var listaClientes = await _baseDatos.Clientes
                                            .Include(e => e.IdUsuarioNavigation)
                                            .Where(e => e.Estatus == estatus)
                                            .Select(e => new
                                            {
                                                e.IdCliente,
                                                e.Nombre,
                                                NombreUsuario = e.IdUsuarioNavigation.Usuario1,
                                                Contrasenia = e.IdUsuarioNavigation.Contrasenia,
                                                Rol = e.IdUsuarioNavigation.Rol
                                            })
                                            .ToListAsync();
            return Ok(listaClientes);
        }

        [HttpPost]
        [Route("registroCliente")]
        public IActionResult RegistrarCliente([FromBody] RegistroDto registroDto)
        {

            if (_baseDatos.Usuarios.Any(u => u.Usuario1 == registroDto.Usuario))
            {
                return BadRequest("El nombre de usuario ya existe");
            }

            var usuario = new Usuario
            {
                Usuario1 = registroDto.Usuario,
                Contrasenia = registroDto.Contrasenia,
                Rol = "cliente"
            };

            _baseDatos.Usuarios.Add(usuario);
            _baseDatos.SaveChanges();

            var cliente = new Cliente
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = registroDto.Nombre,
                Estatus = 1
            };

            _baseDatos.Clientes.Add(cliente);
            _baseDatos.SaveChanges();

            return Ok(new
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Usuario = usuario.Usuario1
            });
        }

        [HttpPut]
        [Route("estatusCliente/{id:int}")]
        public async Task<IActionResult> estatusCliente(int id)
        {
            var estatusCliente = await _baseDatos.Clientes.FindAsync(id);
            if (estatusCliente == null)
            {
                return BadRequest("No existe el Cliente.");
            }

            if (estatusCliente.Estatus == 1)
            {
                estatusCliente.Estatus = 0;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Cliente Eliminado");
            }
            else if (estatusCliente.Estatus == 0)
            {
                estatusCliente.Estatus = 1;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Cliente Restaurado");
            }
            else
            {
                return BadRequest("Estatus de cliente no disponible");
            }
        }

        [HttpPut]
        [Route("actualizarCliente/{id:int}")]
        public async Task<IActionResult> actualizarCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            var cliente = await _baseDatos.Clientes.Include(e => e.IdUsuarioNavigation)
                                                      .FirstOrDefaultAsync(e => e.IdCliente == id);
            if (cliente == null)
            {
                return BadRequest("Empleado no encontrado.");
            }

            // Empleado
            cliente.Nombre = clienteDto.Nombre;

            // Usuario
            var usuario = cliente.IdUsuarioNavigation;
            usuario.Usuario1 = clienteDto.Usuario;
            usuario.Contrasenia = clienteDto.Contrasenia;

            try
            {
                await _baseDatos.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok("Cliente Editado");
        }
        
    }
}
