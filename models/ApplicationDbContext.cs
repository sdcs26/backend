using Microsoft.EntityFrameworkCore;
using WebApplication2.models;
using WebApplication2.Models; // Asegúrate de que el namespace sea correcto

namespace WebApplication2.Data // Asegúrate de que el namespace sea correcto
{
    public class ApplicationDbContext2 : DbContext
    {
        public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options)
            : base(options)
        {
        }

        public DbSet<MovimientoInventario> Inventario { get; set; }
        // Agrega aquí otras tablas como Semillas y Categorías
    }
}
