using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using muebleon.DTOs;
using muebleon.Models;

namespace muebleon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly MuebleonContext _baseDatos;

        public ProductoController(MuebleonContext baseDatos)
        {
            _baseDatos = baseDatos;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarProducto([FromBody] ProductoDto productoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                Categoria = productoDto.Categoria,
                Precio = productoDto.Precio,
                Estatus = productoDto.Estatus,
                DetalleConsumos = productoDto.DetallesConsumo.Select(dc => new DetalleConsumo
                {
                    IdMateriaPrima = dc.IdMateriaPrima,
                    Cantidad = dc.Cantidad
                }).ToList()
            };

            _baseDatos.Productos.Add(producto);
            await _baseDatos.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetProductoById), new { id = producto.IdProducto }, producto);
            return Ok("Producto Registrado");
        }

        [HttpGet]
        [Route("productos")]
        //[Route("producto/{estatus}")]
        //public async Task<IActionResult> ListaProductos(int estatus)
        public async Task<IActionResult> GetAllProductos()
        {
            var productos = await _baseDatos.Productos
                .Include(p => p.DetalleConsumos)
                .ThenInclude(dc => dc.IdMateriaPrimaNavigation)
                .ToListAsync();

            var productoDtos = productos.Select(p => new
            {
                p.IdProducto,
                p.Nombre,
                p.Descripcion,
                p.Categoria,
                p.Precio,
                p.Estatus,
                DetallesConsumo = p.DetalleConsumos.Select(dc => new
                {
                    dc.IdDetalleConsumo,
                    dc.IdMateriaPrima,
                    dc.Cantidad,
                    MateriaPrimaNombre = dc.IdMateriaPrimaNavigation.Nombre
                }).ToList()
            });

            return Ok(productoDtos);
        }

        [HttpGet]
        [Route("producto/{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var producto = await _baseDatos.Productos
                .Include(p => p.DetalleConsumos)
                .ThenInclude(dc => dc.IdMateriaPrimaNavigation)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            var productoDto = new
            {
                producto.IdProducto,
                producto.Nombre,
                producto.Descripcion,
                producto.Categoria,
                producto.Precio,
                producto.Estatus,
                DetallesConsumo = producto.DetalleConsumos.Select(dc => new
                {
                    dc.IdDetalleConsumo,
                    dc.IdMateriaPrima,
                    dc.Cantidad,
                    MateriaPrimaNombre = dc.IdMateriaPrimaNavigation.Nombre
                }).ToList()
            };

            return Ok(productoDto);
        }
        /*
                [HttpGet]
                [Route("producto/{estatus}")]
                public async Task<IActionResult> ListaProductos(int estatus)
                {
                    var listaProductos = await _baseDatos.Productos
                                                    .Where(e => e.Estatus == estatus)
                                                    .Select(e => new
                                                    {
                                                        e.IdProducto,
                                                        e.Nombre,
                                                        e.Categoria,
                                                        e.Precio,
                                                        e.Cantidad
                                                    })
                                                    .ToListAsync();
                    return Ok(listaProductos);
                }

                [HttpPost]
                [Route("registroProducto")]
                public IActionResult RegistrarProducto([FromBody] ProductoDto prt)
                {
                    if(_baseDatos.Productos.Any(p => p.Nombre == prt.Nombre))
                    {
                        return BadRequest("El producto ya existe");
                    }

                    var producto = new Producto
                    {
                        Nombre = prt.Nombre,
                        Descripcion = prt.Descripcion,
                        Categoria = prt.Categoria,
                        Precio = prt.Precio,
                        Cantidad = prt.Cantidad,
                        Estatus = 1
                    };

                    _baseDatos.Productos.Add(producto);
                    _baseDatos.SaveChanges();
                    return Ok(new
                    {
                        idProducto = producto.IdProducto,
                        descripcion = producto.Descripcion,
                        categoria = producto.Categoria,
                        precio = producto.Precio,
                        cantidad = producto.Cantidad
                    });
                }

                [HttpPut]
                [Route("editarProducto/{id:int}")]
                public async Task<IActionResult> EditarProducto(int id, [FromBody] ProductoDto request)
                {
                    var editarProducto = await _baseDatos.Productos.FindAsync(id);
                    if (editarProducto == null)
                    {
                        return BadRequest("No existe el producto.");
                    }

                    editarProducto.Nombre = request.Nombre;
                    editarProducto.Descripcion = request.Descripcion;
                    editarProducto.Categoria = request.Categoria;
                    editarProducto.Precio = request.Precio;
                    editarProducto.Cantidad = request.Cantidad;
                    try
                    {
                        await _baseDatos.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return NotFound();
                    }
                    return Ok("Producto Editado");
                }

                [HttpPut]
                [Route("estatusProducto/{id:int}")]
                public async Task<IActionResult> estatusProducto(int id)
                {
                    var editarProducto = await _baseDatos.Productos.FindAsync(id);
                    if (editarProducto == null)
                    {
                        return BadRequest("No existe el producto.");
                    }

                    if (editarProducto.Estatus == 1)
                    {
                        editarProducto.Estatus = 0;
                        try
                        {
                            await _baseDatos.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return NotFound();
                        }
                        return Ok("Producto Eliminado");
                    }
                    else if (editarProducto.Estatus == 0)
                    {
                        editarProducto.Estatus = 1;
                        try
                        {
                            await _baseDatos.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            return NotFound();
                        }
                        return Ok("Producto Restaurado");
                    }
                    else
                    {
                        return BadRequest("Estatus de producto no disponible");
                    }
                }
                */
    }
}
