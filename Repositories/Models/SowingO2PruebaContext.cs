using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sowing_O2.Repositories.Models;

public partial class SowingO2PruebaContext : DbContext
{
    public SowingO2PruebaContext()
    {
    }

    public SowingO2PruebaContext(DbContextOptions<SowingO2PruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<RecuperacionToken> RecuperacionTokens { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Semilla> Semillas { get; set; }

    public virtual DbSet<TokenRevocado> TokenRevocados { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__E9F1DAD41DF514B2");

            entity.ToTable(tb => tb.HasComment("Registro de las acciones realizadas sobre los usuarios"));

            entity.Property(e => e.IdAuditoria)
                .HasComment("Identificador único para la auditoría")
                .HasColumnName("Id_Auditoria");
            entity.Property(e => e.Accion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Tipo de acción realizada (Inserción, Eliminación)");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Apellido del usuario afectado");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Contraseña del usuario afectado");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Correo electrónico del usuario afectado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha y hora en que se realizó la acción")
                .HasColumnType("datetime");
            entity.Property(e => e.IdRol)
                .HasComment("ID del rol asignado al usuario afectado")
                .HasColumnName("Id_Rol");
            entity.Property(e => e.IdUsuario)
                .HasComment("ID del usuario sobre el que se realizó la acción")
                .HasColumnName("Id_Usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Nombre del usuario afectado");
            entity.Property(e => e.NumDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Número de documento del usuario afectado")
                .HasColumnName("Num_Documento");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Tabla que almacena las categorías de las semillas."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único de la categoría.")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre de la categoría.")
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado", tb => tb.HasComment("Tabla que almacena los diferentes estados que pueden tener el sistema de informacion."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único del estado.")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre del estado.")
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.ToTable("Movimiento", tb => tb.HasComment("Tabla que almacena los movimientos de los elementos de un lugar a otro."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único del movimiento.")
                .HasColumnName("id");
            entity.Property(e => e.AnteriorUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Ubicación anterior del elemento antes del movimiento.")
                .HasColumnName("anterior_ubi");
            entity.Property(e => e.FechaMovi)
                .HasComment("Fecha en la que se realizó el movimiento.")
                .HasColumnType("datetime")
                .HasColumnName("fecha_Movi");
            entity.Property(e => e.IdSemilla)
                .HasComment("ID del elemento o semilla que ha sido movido.")
                .HasColumnName("id_Semilla");
            entity.Property(e => e.IdUsuario)
                .HasComment("ID del usuario que realizó el movimiento.")
                .HasColumnName("id_Usuario");
            entity.Property(e => e.NuevaUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nueva ubicación del elemento después del movimiento.")
                .HasColumnName("nueva_Ubi");

            entity.HasOne(d => d.IdSemillaNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimiento_Semilla");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimiento_User");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Tabla que almacena los pedidos realizados por los usuarios."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único del pedido.")
                .HasColumnName("id");
            entity.Property(e => e.FechaPedido)
                .HasComment("Fecha en la que se realizó el pedido.")
                .HasColumnType("datetime")
                .HasColumnName("fecha_Pedido");
            entity.Property(e => e.IdEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Estado actual del pedido (por ejemplo: En proceso, Enviado, Entregado).")
                .HasColumnName("id_Estado");
            entity.Property(e => e.IdUsuario)
                .HasComment("ID del usuario que realizó el pedido.")
                .HasColumnName("id_Usuario");
            entity.Property(e => e.TotalItem)
                .HasComment("Número total de ítems en el pedido.")
                .HasColumnName("total_Item");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_User");
        });

        modelBuilder.Entity<RecuperacionToken>(entity =>
        {
            entity.ToTable("RecuperacionToken", tb => tb.HasComment("Tabla que almacena los tokens de recuperación de contraseña."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único para el token de recuperación.")
                .HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Correo electrónico del usuario que solicita la recuperación.")
                .HasColumnName("correo");
            entity.Property(e => e.FechaExp)
                .HasComment("Fecha de expiración del token de recuperación.")
                .HasColumnType("datetime")
                .HasColumnName("fechaExp");
            entity.Property(e => e.Token)
                .HasMaxLength(512)
                .HasComment("Token único generado para la recuperación de la contraseña.")
                .HasColumnName("token");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol", tb => tb.HasComment("Tabla que almacena los diferentes roles que pueden ser asignados a los usuarios."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único del rol.")
                .HasColumnName("id");
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre del rol asignado (por ejemplo: Administrador, Usuario).")
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Semilla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83FD95D87AF");

            entity.ToTable("Semilla", tb => tb.HasComment("Tabla que almacena información sobre las semillas."));

            entity.HasIndex(e => e.Codigo, "UQ__Products__40F9A2061968F6B8").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("Identificador único de la semilla.")
                .HasColumnName("id");
            entity.Property(e => e.Cantidad)
                .HasComment("Cantidad de semillas disponibles.")
                .HasColumnName("cantidad");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("Código único para identificar la semilla.")
                .HasColumnName("codigo");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasComment("Descripción de la semilla.")
                .HasColumnName("description");
            entity.Property(e => e.IdCategoria)
                .HasComment("ID de la categoría a la que pertenece la semilla.")
                .HasColumnName("id_Categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre de la semilla.")
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Semillas)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Semilla_Categoria");
        });

        modelBuilder.Entity<TokenRevocado>(entity =>
        {
            entity.ToTable("TokenRevocado", tb => tb.HasComment("Tabla que almacena los tokens que han sido revocados."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único del token revocado.")
                .HasColumnName("id");
            entity.Property(e => e.FechaRevocado)
                .HasComment("Fecha en la que el token fue revocado.")
                .HasColumnType("datetime")
                .HasColumnName("fechaRevocado");
            entity.Property(e => e.Token)
                .HasMaxLength(512)
                .HasComment("Token revocado que ya no es válido para el acceso.")
                .HasColumnName("token");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.ToTable("Ubicacion", tb => tb.HasComment("Tabla que almacena las ubicaciones de las semillas."));

            entity.Property(e => e.Id)
                .HasComment("Identificador único de la ubicación.")
                .HasColumnName("id");
            entity.Property(e => e.CodigoUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Código que identifica la ubicación de la semilla.")
                .HasColumnName("codigo_Ubi");
            entity.Property(e => e.IdSemilla)
                .HasComment("ID de la semilla relacionada a la ubicación.")
                .HasColumnName("id_Semilla");

            entity.HasOne(d => d.IdSemillaNavigation).WithMany(p => p.Ubicacions)
                .HasForeignKey(d => d.IdSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ubicacion_Semilla");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("Usuario", tb =>
                {
                    tb.HasComment("Tabla que almacena los datos de los usuarios registrados en el sistema.");
                    tb.HasTrigger("TR_Auditoria_Usuarios_Delete");
                    tb.HasTrigger("TR_Auditoria_Usuarios_Insert");
                });

            entity.Property(e => e.Id)
                .HasComment("Identificador único del usuario.")
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Apellido del usuario.")
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .HasComment("Contraseña cifrada del usuario.")
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Correo electrónico del usuario.")
                .HasColumnName("correo");
            entity.Property(e => e.IdRol)
                .HasComment("ID del rol asignado al usuario.")
                .HasColumnName("id_Rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Nombre del usuario.")
                .HasColumnName("nombre");
            entity.Property(e => e.NumDocumento)
                .HasComment("Número de documento del usuario.")
                .HasColumnName("num_Documento");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
