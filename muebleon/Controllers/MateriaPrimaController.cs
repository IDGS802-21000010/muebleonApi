using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public MateriaPrimaController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpGet]
        [Route("materiaprima/{estatus}")]
        public async Task<IActionResult> ListaMateriPrima(int estatus)
        {
            var listaMateriaPrima = await _baseDatos.MateriaPrimas
                                            .Where(e => e.Estatus == estatus)
                                            .Select(e => new
                                            {
                                                e.IdMateriaPrima,
                                                e.Nombre,
                                                e.Cantidad
                                            })
                                            .ToListAsync();
                                            
            return Ok(listaMateriaPrima);
        }

        [HttpPost]
        [Route("registroMateriaPrima")]
        public IActionResult RegistrarMateriaPrima([FromBody] MateriaPrimaDto mtDto)
        {
            if (_baseDatos.MateriaPrimas.Any(p => p.Nombre == mtDto.Nombre))
            {
                return BadRequest("La materia prima ya existe");
            }

            var materiaprima = new MateriaPrima
            {
                Nombre = mtDto.Nombre,
                Cantidad = mtDto.Cantidad,
                Estatus = 1
            };

            _baseDatos.MateriaPrimas.Add(materiaprima);
            _baseDatos.SaveChanges();

            return Ok(new
            {
                idMateriaprima = materiaprima.IdMateriaPrima,
                nombre = materiaprima.Nombre,
                cantidad = materiaprima.Cantidad
            });
        }

        [HttpPut]
        [Route("editarMateriaPrima/{id:int}")]
        public async Task<IActionResult> editarMateriaPrima(int id, [FromBody] MateriaPrimaDto request)
        {
            var editarMateriaprima = await _baseDatos.MateriaPrimas.FindAsync(id);
            if (editarMateriaprima == null)
            {
                return BadRequest("No existe la materia prima.");
            }

            editarMateriaprima.Nombre = request.Nombre;
            editarMateriaprima.Cantidad = request.Cantidad;
            try
            {
                await _baseDatos.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok("Materia Prima Editada");
        }

        [HttpPut]
        [Route("estatuMateriaPrima/{id:int}")]
        public async Task<IActionResult> estatusMateriaPrima(int id)
        {
            var editarMateriaPrima = await _baseDatos.MateriaPrimas.FindAsync(id);
            if (editarMateriaPrima == null)
            {
                return BadRequest("No existe el producto.");
            }
            if (editarMateriaPrima.Estatus == 1)
            {
                editarMateriaPrima.Estatus = 0;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Materia Prima Eliminada");
            }
            else if (editarMateriaPrima.Estatus == 0)
            {
                editarMateriaPrima.Estatus = 1;
                try
                {
                    await _baseDatos.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
                return Ok("Materia Prima Restaurada");
            }
            else
            {
                return BadRequest("Estatus de materia prima no disponible");
            }
        }
        
    }
}
