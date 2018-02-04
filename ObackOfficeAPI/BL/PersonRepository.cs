using System;
using System.Collections.Generic;
using System.Linq;
using BE.Comun;
using DAL;

namespace BL
{
    public class PersonRepository
    {
        private List<Person> _lPerson = new List<Person>();
        private DatabaseContext ctx = new DatabaseContext();

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Person> GetAll()
        {
            try
            {
                var query = (from a in ctx.People select a).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public Person GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(Person oPerson)
        {
            throw new NotImplementedException();
        }

        public List<Parametro> GetGeneros()
        {
            int grupo = (int)Enumeradores.GrupoParametros.Generos;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo select a).ToList();

            return result;
        }

        public Parametro InsertGenero(string Descripcion)
        {
            try
            {
                int grupo = (int)Enumeradores.GrupoParametros.Generos;
                int NoEliminado = (int)Enumeradores.EsEliminado.No;
                List<Parametro> Listado = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo select a).ToList();
                int parametroId = (from a in Listado orderby a.ParametroId descending select a.ParametroId).FirstOrDefault() + 1;
                int orden = (from a in Listado orderby a.Orden descending select a.Orden).FirstOrDefault() + 1;

                Parametro data = new Parametro()
                {
                    GrupoId = grupo,
                    ParametroId = parametroId,
                    Valor1 = Descripcion,
                    PadreParametroId = -1,
                    Orden = orden,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now
                };

                ctx.Parametros.Add(data);
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

        public List<Parametro> GetRoles()
        {
            int grupo = (int)Enumeradores.GrupoParametros.Roles;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo select a).ToList();

            return result;
        }

        public Parametro InsertRol(string Descripcion)
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
                    Valor1 = Descripcion,
                    PadreParametroId = -1,
                    Orden = orden,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now
                };

                ctx.Parametros.Add(data);
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
    }
}
