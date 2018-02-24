using BE.Cliente;
using BE.Comun;
using BE.RegistroNotas;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class EmpleadoCursoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public bool InsertarEmpleadoCurso(string empleado, int salonProgramadoId, int userId, int empresaId)
        {
            EmpleadoRepository oEmpleadoRepository = new EmpleadoRepository();
            EmpleadoAsistencia oEmpleadoAsistencia = new EmpleadoAsistencia();
            ParametroRepository oParametroRepository = new ParametroRepository();
            EmpleadoTaller oEmpleadoTaller = new EmpleadoTaller();
            try
            {
                //Obtener Id Empleado
                var datosEmpleado = empleado.Split('*');
                var oEmpleado = oEmpleadoRepository.GetEmpleadoByDocumento(datosEmpleado[1].ToString());
                var empleadoId = oEmpleado.EmpleadoId;

                //Actualizar Empresa
                oEmpleadoRepository.ActualizarEmpleadoEmpresa(empleadoId, empresaId);

                //Insertar en tabla EmpleadoCurso
                EmpleadoCurso oEmpleadoCurso = new EmpleadoCurso();
                oEmpleadoCurso.EmpleadoId = empleadoId;
                oEmpleadoCurso.SalonProgramadoId = salonProgramadoId;
                oEmpleadoCurso.CondicionId = (int)Enumeradores.Condicion.PorIniciar;
                oEmpleadoCurso.EsEliminado = 0;
                oEmpleadoCurso.UsuGraba = userId;
                oEmpleadoCurso.FechaGraba = DateTime.Now;
                ctx.EmpleadoCursos.Add(oEmpleadoCurso);
                ctx.SaveChanges();
                int idEmpleadoCurso = oEmpleadoCurso.EmpleadoCursoId;

                //Obtener lista de clases relacionadas al curso programado
                var lclases = (from a in ctx.SalonClases
                               where a.SalonProgramadoId == salonProgramadoId
                               select a).ToList();

                foreach (var clase in lclases)
                {
                    oEmpleadoAsistencia.EmpleadoCursoId = idEmpleadoCurso;
                    oEmpleadoAsistencia.FechaClase = clase.FechaInicio;
                    oEmpleadoAsistencia.EsEliminado = 0;
                    oEmpleadoAsistencia.Asistio = null;
                    oEmpleadoAsistencia.UsuGraba = userId;
                    oEmpleadoAsistencia.FechaGraba = DateTime.Now;
                    ctx.EmpleadoAsistencias.Add(oEmpleadoAsistencia);
                    ctx.SaveChanges();
                }

                //Insertar en tabla EmpleadoTaller
                
                var lPreguntas = oParametroRepository.GetParametroByGrupoId(105);
                foreach (var item in lPreguntas)
                {
                    oEmpleadoTaller.EmpleadoCursoId = idEmpleadoCurso;
                    oEmpleadoTaller.PreguntaId = item.Id;
                    oEmpleadoTaller.EsEliminado = 0;
                    oEmpleadoTaller.UsuGraba = userId;
                    oEmpleadoTaller.FechaGraba = DateTime.Now;
                    ctx.EmpleadoTalleres.Add(oEmpleadoTaller);
                    ctx.SaveChanges();
                }

               //int rows = ctx.SaveChanges();
               // if (rows > 0) return true;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EliminarEmpleadoCurso(int empleadoCursoId, int userId)
        {
            try
            {
                var oEmpleadocuros = (from a in ctx.EmpleadoCursos where a.EmpleadoCursoId == empleadoCursoId select a).FirstOrDefault();
                oEmpleadocuros.EsEliminado = 1;
                oEmpleadocuros.FechaActualiza = DateTime.Now;
                oEmpleadocuros.UsuActualiza = userId;

                var lAsistencia = (from a in ctx.EmpleadoAsistencias where a.EmpleadoCursoId == empleadoCursoId select a).ToList();
                foreach (var asistencia in lAsistencia)
                {
                    asistencia.EsEliminado = 1;
                    asistencia.FechaActualiza = DateTime.Now;
                    asistencia.UsuActualiza = userId;
                }

                var lTaller = (from a in ctx.EmpleadoTalleres where a.EmpleadoCursoId == empleadoCursoId select a).ToList();
                foreach (var taller in lTaller)
                {
                    taller.EsEliminado = 1;
                    taller.FechaActualiza = DateTime.Now;
                    taller.UsuActualiza = userId;
                }

                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
