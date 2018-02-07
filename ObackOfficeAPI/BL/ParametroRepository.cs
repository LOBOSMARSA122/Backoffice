using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class ParametroRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Dropdownlist> GetParametroByGrupoId(int grupoId)
        {           
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Dropdownlist> result = (from a in ctx.Parametros
                                        where a.EsEliminado == NoEliminado && a.GrupoId == grupoId
                                        orderby a.Orden ascending
                                         select new Dropdownlist
                                         {
                                             Id = a.ParametroId,
                                             Value = a.Valor1
                                         }).ToList();
            return result;
        }
    }
}
