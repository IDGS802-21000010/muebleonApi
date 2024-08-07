using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public ProduccionController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost]
        public async Task<IActionResult> ProducirProducto([FromBody] ProduccionDto produccionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _baseDatos.Productos
                .Include(p => p.DetalleConsumos)
                .FirstOrDefaultAsync(p => p.IdProducto == produccionDto.idProducto);

            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado" });
            }

            foreach (var detalleConsumo in producto.DetalleConsumos)
            {
                var materiaPrima = await _baseDatos.MateriaPrimas
                    .FirstOrDefaultAsync(mp => mp.IdMateriaPrima == detalleConsumo.IdMateriaPrima);

                if (materiaPrima == null || materiaPrima.Cantidad < detalleConsumo.Cantidad * produccionDto.Cantidad)
                {
                    return BadRequest(new { mensaje = "No hay suficiente materia prima para producir el producto" });
                }
            }

            foreach (var detalleConsumo in producto.DetalleConsumos)
            {
                var materiaPrima = await _baseDatos.MateriaPrimas
                    .FirstOrDefaultAsync(mp => mp.IdMateriaPrima == detalleConsumo.IdMateriaPrima);

                materiaPrima.Cantidad -= detalleConsumo.Cantidad * produccionDto.Cantidad;
            }

            await _baseDatos.SaveChangesAsync();

            return Ok("Producto producido exitosamente");
        }
    }
}
