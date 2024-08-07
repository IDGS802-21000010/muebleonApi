using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace muebleon.Models;

public partial class MuebleonContext : DbContext
{
    public MuebleonContext()
    {
    }

    public MuebleonContext(DbContextOptions<MuebleonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleConsumo> DetalleConsumos { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<MateriaPrima> MateriaPrimas { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-T9E5A1CA; Initial Catalog=muebleon; user id=sa; password=root;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__cliente__885457EE7563DF59");

            entity.ToTable("cliente");

            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cliente__idUsuar__398D8EEE");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__compra__48B99DB7D6F80E36");

            entity.ToTable("compra");

            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__idEmplea__45F365D3");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compra__idProvee__44FF419A");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PK__detalleC__62C252C1C744D41C");

            entity.ToTable("detalleCompra");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("idDetalleCompra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.IdMateriaPrima).HasColumnName("idMateriaPrima");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleCo__subto__48CFD27E");

            entity.HasOne(d => d.IdMateriaPrimaNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdMateriaPrima)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleCo__idMat__49C3F6B7");
        });

        modelBuilder.Entity<DetalleConsumo>(entity =>
        {
            entity.HasKey(e => e.IdDetalleConsumo).HasName("PK__detalleC__539790262DED099B");

            entity.ToTable("detalleConsumo");

            entity.Property(e => e.IdDetalleConsumo).HasColumnName("idDetalleConsumo");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdMateriaPrima).HasColumnName("idMateriaPrima");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");

            entity.HasOne(d => d.IdMateriaPrimaNavigation).WithMany(p => p.DetalleConsumos)
                .HasForeignKey(d => d.IdMateriaPrima)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleCo__idMat__571DF1D5");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleConsumos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detalleCo__idPro__5812160E");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__detalleP__610F00560A7B1611");

            entity.ToTable("detallePedido");

            entity.Property(e => e.IdDetallePedido).HasColumnName("idDetallePedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallePe__idPed__5070F446");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detallePe__idPro__5165187F");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__empleado__5295297CECA1F9F6");

            entity.ToTable("empleado");

            entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__empleado__idUsua__3C69FB99");
        });

        modelBuilder.Entity<MateriaPrima>(entity =>
        {
            entity.HasKey(e => e.IdMateriaPrima).HasName("PK__materiaP__97EB058F6ED9ABB5");

            entity.ToTable("materiaPrima");

            entity.Property(e => e.IdMateriaPrima).HasColumnName("idMateriaPrima");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__pedido__A9F619B7892EBBAC");

            entity.ToTable("pedido");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Estatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("estatus");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("idCliente");
            entity.Property(e => e.IdEmpleado).HasColumnName("idEmpleado");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido__idClient__4D94879B");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK__pedido__idEmplea__4CA06362");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__07F4A1328DC1CA8D");

            entity.ToTable("producto");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Categoria)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__proveedo__A3FA8E6B87AE6524");

            entity.ToTable("proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.NombreProveedor)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreProveedor");
            entity.Property(e => e.Telefono)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__645723A611C15738");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Rol)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__venta__077D561428C85ED5");

            entity.ToTable("venta");

            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__venta__idPedido__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
