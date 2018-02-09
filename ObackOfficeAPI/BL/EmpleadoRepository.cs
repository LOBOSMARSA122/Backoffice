using BE.Cliente;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class EmpleadoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<BusquedaEmpleado> GetEmpleados (string valor, int empresaId)
        {
            try
            {
                var query = (from a in ctx.Personas
                             join b in ctx.Empleados on a.PersonaId equals b.PersonaId
                             where a.EsEliminado == 0 && b.EmpresaId == empresaId
                             && (a.Nombres.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.NroDocumento.Contains(valor))
                             select new BusquedaEmpleado
                             {
                                 EmpleadoId = b.EmpleadoId,
                                 PersonaId = b.PersonaId,
                                 EmpresaId = b.EmpresaId,
                                 NombreCompleto = a.Nombres + " " + a.ApellidoPaterno + " " + a.ApellidoMaterno,
                                 NroDocumento = a.NroDocumento
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetEmpleadosString(string valor, int empresaId)
        {
            try
            {
                var query = (from a in ctx.Personas
                             join b in ctx.Empleados on a.PersonaId equals b.PersonaId
                             where a.EsEliminado == 0 && b.EmpresaId == empresaId
                             && (a.Nombres.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.ApellidoPaterno.Contains(valor) || a.NroDocumento.Contains(valor))
                             select new 
                             {
                                 NombreCompleto = a.Nombres + " " + a.ApellidoPaterno + " " + a.ApellidoMaterno + "-" + a.NroDocumento,
                             
                             }).ToList();

                return query.Select(x => x.NombreCompleto).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Empleado GetEmpleadoByDocumento(string nroDocumento)
        {
            try
            {
                var query = (from a in ctx.Empleados
                             join b in ctx.Personas on a.PersonaId equals b.PersonaId
                             where b.NroDocumento == nroDocumento
                             select a).FirstOrDefault();

                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
