using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public PedidoController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost]
        [Route("pedidoRegistro")]
        public async Task<IActionResult> RegistroPedido([FromBody] PedidoDto pedidoDto)
        {
            if (pedidoDto == null || pedidoDto.Detalles == null || pedidoDto.Detalles.Count == 0)
            {
                return BadRequest("Pedido inválido.");
            }

            var pedido = new Pedido
            {
                IdCliente = pedidoDto.idCliente,
                IdEmpleado = pedidoDto.idEmpleado,
                Fecha = pedidoDto.Fecha,
                Total = pedidoDto.Total,
                Estatus = "Pendiente" // Ejemplo de estatus inicial
            };

            _baseDatos.Pedidos.Add(pedido);
            await _baseDatos.SaveChangesAsync();

            foreach (var detalleDto in pedidoDto.Detalles)
            {
                var detallePedido = new DetallePedido
                {
                    IdPedido = pedido.IdPedido,
                    IdProducto = detalleDto.IdProducto,
                    Cantidad = detalleDto.Cantidad,
                    Subtotal = detalleDto.Subtotal
                };

                _baseDatos.DetallePedidos.Add(detallePedido);
            }

            await _baseDatos.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.IdPedido }, pedido);
        }

        [HttpGet]
        [Route("pedidos")]
        public async Task<IActionResult> GetAllPedidos()
        {
            var pedidos = await _baseDatos.Pedidos
                .Include(p => p.DetallePedidos)
                .Select(p => new PedidoDto
                {
                    idPedido = p.IdPedido,
                    idCliente = p.IdCliente,
                    Fecha = p.Fecha,
                    Estatus = p.Estatus,
                    idEmpleado = p.IdEmpleado,
                    Detalles = p.DetallePedidos.Select(dp => new DetallePedidoDto
                    {
                        IdDetallePedido = dp.IdDetallePedido,
                        IdProducto = dp.IdProducto,
                        Cantidad = dp.Cantidad,
                        Subtotal = dp.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet]
        [Route("pedido/{id}")]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            var pedido = await _baseDatos.Pedidos
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoDto = new PedidoDto
            {
                idPedido = pedido.IdPedido,
                idCliente = pedido.IdCliente,
                Fecha = pedido.Fecha,
                Estatus = pedido.Estatus,
                idEmpleado = pedido.IdEmpleado,
                Detalles = pedido.DetallePedidos.Select(dp => new DetallePedidoDto
                {
                    IdDetallePedido = dp.IdDetallePedido,
                    IdProducto = dp.IdProducto,
                    Cantidad = dp.Cantidad,
                    Subtotal = dp.Subtotal
                }).ToList()
            };

            return Ok(pedidoDto);
        }

        [HttpGet]
        [Route("pedido/empleado/{id}")]
        public async Task<IActionResult> GetPedidosByEmpleado(int idEmpleado)
        {
            var pedidos = await _baseDatos.Pedidos
                .Where(p => p.IdEmpleado == idEmpleado)
                .Include(p => p.DetallePedidos)
                .Select(p => new PedidoDto
                {
                    idPedido = p.IdPedido,
                    idCliente = p.IdCliente,
                    Fecha = p.Fecha,
                    Estatus = p.Estatus,
                    idEmpleado = p.IdEmpleado,
                    Detalles = p.DetallePedidos.Select(dp => new DetallePedidoDto
                    {
                        IdDetallePedido = dp.IdDetallePedido,
                        IdProducto = dp.IdProducto,
                        Cantidad = dp.Cantidad,
                        Subtotal = dp.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpPut]
        [Route("pedido/{id}/asignar")]
        public async Task<IActionResult> AsignarEmpleadoAPedido(int id, [FromBody] int empleadoId)
        {
            var pedido = await _baseDatos.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.IdEmpleado = empleadoId;

            _baseDatos.Pedidos.Update(pedido);
            await _baseDatos.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("procesar")]
        public async Task<IActionResult> ProcesarPedido([FromBody] ActualizarPedidoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await _baseDatos.Pedidos
                .Include(p => p.DetallePedidos)
                .FirstOrDefaultAsync(p => p.IdPedido == dto.idPedido);

            if (pedido == null)
            {
                return NotFound("Pedido no encontrado");
            }

            foreach (var detalle in pedido.DetallePedidos)
            {
                var producto = await _baseDatos.Productos
                    .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);

                if (producto == null || producto.Cantidad < detalle.Cantidad)
                {
                    return BadRequest($"No hay suficiente stock para el producto {producto?.Nombre}");
                }
            }

            foreach (var detalle in pedido.DetallePedidos)
            {
                var producto = await _baseDatos.Productos
                    .FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);

                producto.Cantidad -= detalle.Cantidad;
            }

            pedido.Estatus = "Procesado";

            await _baseDatos.SaveChangesAsync();

            return Ok("Pedido procesado exitosamente");
        }
    }
}
