using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CursoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Dropdownlist> ddlCursos()
        {
            try
            {

                var query = (from a in ctx.Cursos
                             where a.EsEliminado == 0
                             select new Dropdownlist
                             {
                                 Id = a.CursoId,
                                 Value = a.NombreCurso
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
