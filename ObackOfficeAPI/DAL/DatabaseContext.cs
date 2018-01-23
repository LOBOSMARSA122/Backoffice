using BE;
using System.Data.Entity;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=BDBackoffice") {}
        public DbSet<Person> People { get; set; }
    }
}
