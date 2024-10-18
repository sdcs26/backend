using Arch.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication2.models;

namespace WebApplication2.Models
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options) : base(options)
        {
        }

        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

        // Otros DbSets si tienes más entidades
    }
}


