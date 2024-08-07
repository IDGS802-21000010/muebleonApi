using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public ProveedorController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("proveedor/{estatus}")]
        public async Task<IActionResult> ListaProveedor(int estatus)
        {
            var listaProveedor = await _baseDatos.Proveedors
                                                .Where(e => e.Estatus == estatus)
                                                .Select(e => new
                                                {
                                                    e.IdProveedor,
                                                    e.NombreProveedor,
                                                    e.Descripcion,
                                                    e.Telefono
                                                })
                                                .ToListAsync();

            return Ok(listaProveedor);
        }

        [HttpPost]
        [Route("registroProveedor")]
        public IActionResult RegistrarProveedor([FromBody] ProveedorDto prv)
        {
            if (_baseDatos.Proveedors.Any(P => P.NombreProveedor == prv.NombreProveedor))
            {
                return BadRequest("El proveedor ya existe");
            }

            var proveedor = new Proveedor
            {
                NombreProveedor = prv.NombreProveedor,
                Descripcion = prv.Descripcion,
                Telefono = prv.Telefono,
                Estatus = 1
            };

            _baseDatos.Proveedors.Add(proveedor);
            _baseDatos.SaveChanges();
            return Ok(new
            {
                idProveedor = proveedor.IdProveedor,
                descripcion = proveedor.Descripcion,
                telefono = proveedor.Telefono
            });
        }

        [HttpPut]
        [Route("editarProveedor/{id:int}")]
        public async Task<IActionResult> EditarProveedor(int id, [FromBody] ProveedorDto request)
        {
            var editarProveedor = await _baseDatos.Proveedors.FindAsync(id);
            if (editarProveedor == null)
            {
                return BadRequest("No existe el proveedor.");
            }

            editarProveedor.NombreProveedor = request.NombreProveedor;
            editarProveedor.Descripcion = request.Descripcion;
            editarProveedor.Telefono = request.Telefono;
            try
            {
                await _baseDatos.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok("Proveedor Editado");
        }

        [HttpPut]
        [Route("estatusProveedor/{id:int}")]
        public async Task<IActionResult> eliminarProveedor(int id)
        {
            var editarProveedor = await _baseDatos.Proveedors.FindAsync(id);
            if (editarProveedor == null)
            {
                return BadRequest("No existe el proveedor.");
            }

            if (editarProveedor.Estatus == 1)
            {
                editarProveedor.Estatus = 0;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Proveedor Eliminado");
            }
            else if (editarProveedor.Estatus == 0)
            {
                editarProveedor.Estatus = 1;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Proveedor Restaurado");
            }
            else
            {
                return BadRequest("Estatus de proveedor no disponible");
            }

        }
    }
}
