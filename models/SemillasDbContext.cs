using Microsoft.EntityFrameworkCore;
using WebApplication2.models;

namespace WebApplication2.Models
{
    public class SemillasDbContext : DbContext
    {
        public SemillasDbContext(DbContextOptions<SemillasDbContext> options) : base(options) { }

        public DbSet<Semilla> Semillas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MovimientoInventario> Inventario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales si es necesario
        }
    }
}

