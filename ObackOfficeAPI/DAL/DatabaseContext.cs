using BE.Acceso;
using BE.Administracion;
using BE.Cliente;
using BE.RegistroNotas;
using BE.Comun;
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
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Capacitador> Capacitadores { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoProgramado> CursosProgramados { get; set; }
        public DbSet<EventoSalon> EventoSalones { get; set; }
        public DbSet<SalonProgramado> SalonProgramados { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<EmpleadoCurso> EmpleadoCursos { get; set; }
        public DbSet<EmpleadoAsistencia> EmpleadoAsistencias { get; set; }
        public DbSet<EmpleadoTaller> EmpleadoTalleres { get; set; }
    }
}
