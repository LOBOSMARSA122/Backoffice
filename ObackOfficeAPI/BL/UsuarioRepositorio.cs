using System;
using System.Collections.Generic;
using System.Linq;
using BE.Acceso;
using BE.Comun;
using DAL;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BL
{
    public class UsuarioRepositorio
    {
        private DatabaseContext ctx = new DatabaseContext();

        public UsuarioAutorizado LoginUsuario(string usuario, string contrasenia)
        {
            try
            {
                UsuarioAutorizado oUsuarioAutorizado = new UsuarioAutorizado();
                var qUsuario = (from a in ctx.Usuarios
                                join b in ctx.Personas on a.PersonaId equals b.PersonaId
                                join c in ctx.Parametros on new {a = a.RolId, b=100} equals new {a = c.ParametroId, b= c.GrupoId}
                                where a.NombreUsuario == usuario && a.Contrasenia == contrasenia
                                select new UsuarioAutorizado {
                                    UsuarioId = a.UsuarioId,
                                    PersonaId = a.PersonaId,
                                    NombreUsuario = a.NombreUsuario,
                                    NombreCompleto = b.Nombres + " " + b.ApellidoPaterno,
                                    FechaCaduca = a.FechaCaduca,
                                    RolId = a.RolId,
                                    Rol = c.Valor1

                             }).FirstOrDefault();
                if (qUsuario != null)
                {
                    var qAutu = GetAutorizacion(qUsuario.RolId);
                    qUsuario.Autorizacion = qAutu;
                }
                else
                {
                    return null;
                }               

                return qUsuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Autorizacion> GetAutorizacion(int rolId)
        {

            var perfil = (from a in ctx.Perfiles
                          join b in ctx.Menus on a.MenuId equals b.MenuId
                          where a.RolId == rolId && a.EsEliminado == 0
                          select new SubMenu
                          {
                              MenuId = a.MenuId,
                              Descripcion = b.Descripcion,
                              PadreId = b.PadreId,
                              Icono = b.Icono,
                              Uri = b.Uri
                          }).ToList();

            var query = (from a in ctx.Perfiles
                        join b in ctx.Menus on a.MenuId equals b.MenuId
                         where a.RolId == rolId && a.EsEliminado == 0 && b.PadreId ==-1
                         select new Autorizacion
                         {
                             PerfilId = a.PerfilId,
                             RolId = a.RolId,
                             MenuId = b.MenuId,
                             Descripcion = b.Descripcion,
                             PadreId = b.PadreId,
                             Icono = b.Icono,
                             Uri = b.Uri
                         }).ToList();
            query.ForEach(a =>
            {
                a.SubMenus = perfil.FindAll(p => p.PadreId == a.MenuId);
            });

            return query;


        }

        public BandejaUsuario GetUsuarios(BandejaUsuario data)
        {
            try
            {
                int NoEliminado = (int)Enumeradores.EsEliminado.No;
                int GrupoRol = (int)Enumeradores.GrupoParametros.Roles;
                int GrupoEmpresa = (int)Enumeradores.GrupoParametros.TipoEmpresas;
                string NombreUsuario = string.IsNullOrWhiteSpace(data.NombreUsuario) ? "" : data.NombreUsuario;
                string NombrePersona = string.IsNullOrWhiteSpace(data.NombrePersona) ? "" : data.NombrePersona;
                int skip = (data.Index -1) * data.Take;

                var Lista = (from U in ctx.Usuarios
                             join P in ctx.Personas on U.PersonaId equals P.PersonaId
                             join R in ctx.Parametros on U.RolId equals R.ParametroId
                             join E in ctx.Empresas on U.EmpresaId equals E.EmpresaId
                             join TE in ctx.Parametros on E.TipoEmpresaId equals TE.ParametroId
                             where U.EsEliminado == NoEliminado &&
                             P.EsEliminado == NoEliminado &&
                             R.GrupoId == GrupoRol &&
                             TE.GrupoId == GrupoEmpresa &&
                             U.NombreUsuario.Contains(NombreUsuario) &&
                             (P.Nombres.Contains(NombrePersona) ||
                             P.ApellidoPaterno.Contains(NombrePersona) ||
                             P.ApellidoMaterno.Contains(NombrePersona))
                             select new BandejaUsuarioLista()
                             {
                                 UsuarioId = U.UsuarioId,
                                 NombreCompleto = P.Nombres + " " + P.ApellidoPaterno + " " + P.ApellidoMaterno,
                                 NombreUsuario = U.NombreUsuario,
                                 Empresa = E.RazonSocial,
                                 Rol = R.Valor1,
                                 TipoEmpresa = TE.Valor1
                             }).ToList();

                int TotalRegistros = Lista.Count;

                Lista = Lista.Skip(skip).Take(data.Take).ToList();

                BandejaUsuario returnData = new BandejaUsuario()
                {
                    Lista = Lista,
                    TotalRegistros = TotalRegistros
                };

                return returnData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Usuario GetUsuario(int id)
        {
            try
            {
                int NoEliminado = (int)Enumeradores.EsEliminado.No;
                return (from a in ctx.Usuarios where a.EsEliminado == NoEliminado && a.UsuarioId == id
                        select a).ToList().Select(a => new Usuario()
                        {
                            EmpresaId = a.EmpresaId,
                            EsEliminado = a.EsEliminado,
                            FechaActualiza = a.FechaActualiza,
                            FechaCaduca = a.FechaCaduca,
                            PersonaId = a.PersonaId,
                            FechaGraba = a.FechaGraba,
                            NombreUsuario = a.NombreUsuario,
                            PreguntaSecreta = a.PreguntaSecreta,
                            RolId = a.RolId,
                            UsuActualiza = a.UsuActualiza,
                            UsuarioId = a.UsuarioId,
                            UsuGraba = a.UsuGraba
                        }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DeleteUser(int DeleteId, int UserId)
        {
            try
            {
                var Usuario = (from a in ctx.Usuarios where a.UsuarioId == DeleteId select a).FirstOrDefault();

                Usuario.UsuActualiza = UserId;
                Usuario.FechaActualiza = DateTime.Now;
                Usuario.EsEliminado = (int)Enumeradores.EsEliminado.Si;

                int rows = ctx.SaveChanges();
                return rows > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public MemoryStream BandejaUsuarioExcel(BandejaUsuario data)
        {
            try
            {
                List<BandejaUsuarioLista> Lista = GetUsuarios(data).Lista;

                IWorkbook Book = new XSSFWorkbook();
                ISheet Sheet = Book.CreateSheet("Lista");
                int index = 0;
                IRow Row = Sheet.CreateRow(index);


                Row.CreateCell(0).SetCellValue("Nombre Usuario");
                Row.CreateCell(0).SetCellValue("Nombre Completo");
                Row.CreateCell(0).SetCellValue("Rol");
                Row.CreateCell(0).SetCellValue("Empresa");
                Row.CreateCell(0).SetCellValue("Tipo Empresa");
                foreach (var Usuario in Lista)
                {
                    index++;
                    Row = Sheet.CreateRow(index);
                    Row.CreateCell(0).SetCellValue(Usuario.NombreUsuario);
                    Row.CreateCell(1).SetCellValue(Usuario.NombreCompleto);
                    Row.CreateCell(2).SetCellValue(Usuario.Rol);
                    Row.CreateCell(3).SetCellValue(Usuario.Empresa);
                    Row.CreateCell(4).SetCellValue(Usuario.TipoEmpresa);
                }
                Sheet.AutoSizeColumn(0);
                Sheet.AutoSizeColumn(1);
                Sheet.AutoSizeColumn(2);
                Sheet.AutoSizeColumn(3);
                Sheet.AutoSizeColumn(4);

                MemoryStream ms = new MemoryStream();
                using (MemoryStream tempStream = new MemoryStream())
                {
                    Book.Write(tempStream);
                    var byteArray = tempStream.ToArray();
                    ms.Write(byteArray, 0, byteArray.Length);
                }

                return ms;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
