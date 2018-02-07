using BE.Administracion;
using BE.Comun;
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

        public List<Dropdownlist> ddlEventos(int sedeId)
        {
            try
            {
              
                var query = (from a in ctx.Eventos
                             where a.SedeId == sedeId
                             select new Dropdownlist
                             {
                                 Id = a.EventoId,
                                 Value = a.Nombre
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }           
        }

        public List<BandejaEventos> GetEventos(string nombreEvento)
        {
            try
            {
                var query = (from a in ctx.Eventos
                             join b in ctx.Parametros on new {a= a.SedeId, b= 106 } equals new {a= b.ParametroId, b =b.GrupoId}
                             select new BandejaEventos
                             {
                                 EventoId = a.EventoId,
                                 Nombre = a.Nombre,
                                 SedeId =  a.SedeId,
                                 Sede = b.Valor1
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
