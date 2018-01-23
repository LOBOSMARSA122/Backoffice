using BE;
using System.Data.Entity;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=DefaultConnection") {}
        public virtual DbSet<Person> People { get; set; }
    }
}
