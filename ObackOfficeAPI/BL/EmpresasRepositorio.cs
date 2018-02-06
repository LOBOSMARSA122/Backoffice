using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.Comun;
using BE.Administracion;
using DAL;

namespace BL
{
    public class EmpresasRepositorio
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Parametro> GetTipoEmpresas()
        {
            int grupo = (int)Enumeradores.GrupoParametros.TipoEmpresas;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending select a).ToList();

            return result;
        }

        public List<Empresa> GetEmpresas()
        {
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Empresa> result = (from a in ctx.Empresas where a.EsEliminado == NoEliminado select a).ToList();

            return result;
        }

        public List<Empresa> GetEmpresas(int TipoEmpresa)
        {
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Empresa> result = (from a in ctx.Empresas where a.EsEliminado == NoEliminado && a.TipoEmpresaId == TipoEmpresa select a).ToList();

            return result;
        }

        public Empresa GetEmpresa(int ID)
        {
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            return (from a in ctx.Empresas where a.EsEliminado == NoEliminado && a.EmpresaId == ID select a).FirstOrDefault();
        }
    }
}
