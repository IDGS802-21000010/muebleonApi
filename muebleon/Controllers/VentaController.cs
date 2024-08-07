using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public VentaController(MuebleonContext context)
        {
            _baseDatos = context;
        }

        [HttpGet]
        [Route("Ventas")]
        public async Task<IActionResult> GetAllVentas()
        {
            var ventas = await _baseDatos.Venta
                .Include(v => v.IdPedidoNavigation)
                .ThenInclude(p => p.DetallePedidos)
                .Select(v => new VentaDto
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    IdPedido = v.IdPedido,
                    Total = v.IdPedidoNavigation.DetallePedidos.Sum(dp => dp.Subtotal),
                    Pedido = new PedidoDto
                    {
                        idPedido = v.IdPedidoNavigation.IdPedido,
                        idCliente = v.IdPedidoNavigation.IdCliente,
                        Fecha = v.IdPedidoNavigation.Fecha,
                        Estatus = v.IdPedidoNavigation.Estatus,
                        Detalles = v.IdPedidoNavigation.DetallePedidos.Select(dp => new DetallePedidoDto
                        {
                            IdDetallePedido = dp.IdDetallePedido,
                            IdProducto = dp.IdProducto,
                            Cantidad = dp.Cantidad,
                            Subtotal = dp.Subtotal
                        }).ToList()
                    }
                })
                .ToListAsync();

            return Ok(ventas);
        }
    }
}
