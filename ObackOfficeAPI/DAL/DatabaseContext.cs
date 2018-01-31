using BE;
using System.Data.Entity;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=BDBackoffice") {}
        public DbSet<Person> People { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Persona> Personas  { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
