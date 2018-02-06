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

        public List<Dropdownlist> GetParametroByGrupoId(int grupoId, string accion)
        {
            Dropdownlist oParametro = new Dropdownlist();
            oParametro.ParametroId = -1;
            if (accion == "seleccionar")
            {               
                oParametro.Valor1 = "--Seleccionar";
            }
            else
            {
                oParametro.Valor1 = "--Todos--";
            }
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Dropdownlist> result = (from a in ctx.Parametros
                                        where a.EsEliminado == NoEliminado && a.GrupoId == grupoId
                                        orderby a.Orden ascending
                                         select new Dropdownlist
                                         {
                                             ParametroId = a.ParametroId,
                                             Valor1 = a.Valor1
                                         }).ToList();
            result.Insert(0, oParametro);
            return result;
        }
    }
}
