using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CapacitadorRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Dropdownlist> ddlCapacitadores()
        {
            try
            {

                var query = (from a in ctx.Capacitadores
                             join b in ctx.Personas on a.PersonaId equals b.PersonaId
                             where a.EsEliminado == 0
                             select new Dropdownlist
                             {
                                 Id = a.CapacitadorId,
                                 Value = b.Nombres + " " + b.ApellidoPaterno + " " + b.ApellidoMaterno
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
