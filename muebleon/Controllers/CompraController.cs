using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public CompraController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost]
        public async Task<IActionResult> RegistroCompra([FromBody] CompraDto compraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var compra = new Compra
            {
                Fecha = compraDto.Fecha,
                IdEmpleado = compraDto.IdEmpleado,
                IdProveedor = compraDto.IdProveedor,
                Total = compraDto.Detalles.Sum(d => d.Subtotal),
                DetalleCompras = compraDto.Detalles.Select(d => new DetalleCompra
                {
                    IdMateriaPrima = d.IdMateriaPrima,
                    Cantidad = d.Cantidad,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            _baseDatos.Compras.Add(compra);
            await _baseDatos.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetCompraById), new { id = compra.IdCompra }, compra);
            return Ok("Compra Registrada");
        }

        [HttpGet]
        [Route("compras")]
        public async Task<IActionResult> GetAllCompras()
        {
            var compras = await _baseDatos.Compras
                .Include(c => c.DetalleCompras)
                .Select(c => new
                {
                    c.IdCompra,
                    c.Fecha,
                    c.IdEmpleado,
                    c.IdProveedor,
                    Total = c.DetalleCompras.Sum(dc => dc.Subtotal), // Calcular el total de la compra
                    Detalles = c.DetalleCompras.Select(dc => new
                    {
                        dc.IdDetalleCompra,
                        dc.IdMateriaPrima,
                        dc.Cantidad,
                        dc.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            return Ok(compras);
        }

        [HttpGet]
        [Route("compras/{id}")]
        public async Task<IActionResult> GetCompraById(int id)
        {
            var compra = await _baseDatos.Compras
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.IdCompra == id);

            if (compra == null)
            {
                return NotFound();
            }

            var compraDto = new
            {
                compra.IdCompra,
                compra.Fecha,
                compra.IdEmpleado,
                compra.IdProveedor,
                Total = compra.DetalleCompras.Sum(dc => dc.Subtotal), // Calcular el total de la compra
                Detalles = compra.DetalleCompras.Select(dc => new
                {
                    dc.IdDetalleCompra,
                    dc.IdMateriaPrima,
                    dc.Cantidad,
                    dc.Subtotal
                }).ToList()
            };

            return Ok(compraDto);
        }
    }
}
