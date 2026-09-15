using System.Data.Entity;

namespace Sputiffy.Models
{
    public class SputiffyContext : DbContext
    {
        // "SputiffyContext" debe coincidir con el nombre de la cadena
        // de conexión en Web.config
        public SputiffyContext() : base("name=SputiffyContext")
        {
        }

        public DbSet<Cancion> Canciones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Evita que EF intente crear/migrar la base automáticamente,
            // porque tú ya la vas a crear con el script SQL
            Database.SetInitializer<SputiffyContext>(null);

            base.OnModelCreating(modelBuilder);
        }
    }
}