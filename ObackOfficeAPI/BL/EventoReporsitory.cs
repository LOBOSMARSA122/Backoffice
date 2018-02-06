using BE.Administracion;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class EventoReporsitory
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<BandejaEventos> GetEventos(int empresaId, string nombreEvento)
        {
            try
            {
                var query = (from a in ctx.Eventos
                             //where (a.Nombre == null || a.Nombre.Contains(nombreEvento)) 
                             //       && (a.EmpresaId == -1 ||a.EmpresaId == empresaId)
                             select new BandejaEventos
                             {
                                 EventoId = a.EventoId,
                                 Nombre = a.Nombre
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
