using System;
using System.Collections.Generic;
using System.Linq;
using BE.Acceso;
using BE.Comun;
using DAL;

namespace BL
{
    public class PerfilesRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<TreeView> GetTreeData(int id)
        {
            try
            {
                var menus = (from a in ctx.Menus select a).ToList();
                List<int> accesos = (from a in ctx.Perfiles where a.RolId == id select a.MenuId).ToList();

                List<TreeView> FatherList = new List<TreeView>();
                foreach (var padre in menus.Where(x => x.PadreId == -1))
                {
                    List<TreeView> ChildList = new List<TreeView>();
                    foreach(var hijo in menus.Where(x => x.PadreId == padre.MenuId))
                    {
                        TreeView Child = new TreeView()
                        {
                            text = hijo.Descripcion,
                            state = new TreeViewState()
                            {
                                @checked = accesos.Contains(hijo.MenuId)
                            },
                            MenuId = hijo.MenuId
                        };
                        ChildList.Add(Child);
                    }
                    TreeView Father = new TreeView()
                    {
                        text = padre.Descripcion,
                        nodes = ChildList.ToArray(),
                        state = new TreeViewState()
                        {
                            @checked = accesos.Contains(padre.MenuId)
                        },
                        MenuId = padre.MenuId
                    };
                    FatherList.Add(Father);
                }


                return FatherList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Parametro InsertRol(string Nombre, List<TreeView> Tree, int UserID)
        {
            try
            {
                int grupo = (int)Enumeradores.GrupoParametros.Roles;
                int NoEliminado = (int)Enumeradores.EsEliminado.No;
                List<Parametro> Listado = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo select a).ToList();
                int parametroId = (from a in Listado orderby a.ParametroId descending select a.ParametroId).FirstOrDefault() + 1;
                int orden = (from a in Listado orderby a.Orden descending select a.Orden).FirstOrDefault() + 1;
                Parametro data = new Parametro()
                {
                    GrupoId = grupo,
                    ParametroId = parametroId,
                    Valor1 = Nombre,
                    PadreParametroId = -1,
                    Orden = orden,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now,
                    UsuGraba = UserID
                };

                List<Perfil> ListPerfiles = new List<Perfil>();
                foreach(var P in Tree)
                {
                    Perfil perfil = new Perfil()
                    {
                        RolId = parametroId,
                        MenuId = P.MenuId,
                        EsEliminado = NoEliminado,
                        FechaGraba = DateTime.Now,
                        UsuGraba = UserID
                    };

                    ListPerfiles.Add(perfil);
                }

                ctx.Parametros.Add(data);
                ctx.Perfiles.AddRange(ListPerfiles);


                int rows = ctx.SaveChanges();
                if (rows > 0)
                    return data;


                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Save(Perfil oPerson)
        {
            throw new NotImplementedException();
        }
    }
}
