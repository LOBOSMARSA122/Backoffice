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
                                selected = accesos.Contains(hijo.MenuId)
                            }
                        };
                        ChildList.Add(Child);
                    }
                    TreeView Father = new TreeView()
                    {
                        text = padre.Descripcion,
                        nodes = ChildList.ToArray(),
                        state = new TreeViewState()
                        {
                            selected = accesos.Contains(padre.MenuId)
                        }
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

        public bool Save(Perfil oPerson)
        {
            throw new NotImplementedException();
        }
    }
}
