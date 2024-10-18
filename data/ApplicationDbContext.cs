using Microsoft.EntityFrameworkCore;
using WebApplication2.models;

namespace WebApplication2.Data // Asegúrate de que el namespace sea correcto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Semilla> Semillas { get; set; } // Asegúrate de agregar esto si tienes la entidad Semilla

        public DbSet<MovimientoInventario> Inventario { get; set; }

        // Agrega aquí otras tablas como Semillas y Categorías si las tienes
        // Ejemplo:
        // public DbSet<Semilla> Semillas { get; set; }
        // public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones específicas de las entidades, si es necesario
            base.OnModelCreating(modelBuilder);
        }
    }
}
