using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sowing_O2.Dtos;

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

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Semilla> Semillas { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<TokenRevocado> TokenRevocado { get; set; }
    public virtual DbSet<RecuperacionToken> RecuperacionToken { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.ToTable("Movimiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnteriorUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("anterior_ubi");
            entity.Property(e => e.FechaMovi)
                .HasColumnType("datetime")
                .HasColumnName("fecha_Movi");
            entity.Property(e => e.IdSemilla).HasColumnName("id_Semilla");
            entity.Property(e => e.IdUsuario).HasColumnName("id_Usuario");
            entity.Property(e => e.NuevaUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
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
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaPedido)
                .HasColumnType("datetime")
                .HasColumnName("fecha_Pedido");
            entity.Property(e => e.IdEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Estado");
            entity.Property(e => e.IdUsuario).HasColumnName("id_Usuario");
            entity.Property(e => e.TotalItem).HasColumnName("total_Item");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_User");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Semilla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83FD95D87AF");

            entity.ToTable("Semilla");

            entity.HasIndex(e => e.Codigo, "UQ__Products__40F9A2061968F6B8").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IdCategoria).HasColumnName("id_Categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Semillas)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Semilla_Categoria");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.ToTable("Ubicacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoUbi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_Ubi");
            entity.Property(e => e.IdSemilla).HasColumnName("id_Semilla");

            entity.HasOne(d => d.IdSemillaNavigation).WithMany(p => p.Ubicacions)
                .HasForeignKey(d => d.IdSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ubicacion_Semilla");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.IdRol).HasColumnName("id_Rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumDocumento).HasColumnName("num_Documento");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
