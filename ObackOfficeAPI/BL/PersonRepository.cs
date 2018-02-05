using System;
using System.Collections.Generic;
using System.Linq;
using BE.Comun;
using BE.Acceso;
using BE;
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
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending select a).ToList();

            return result;
        }

        public Parametro InsertGenero(string Descripcion, int UsuarioID)
        {
            try
            {
                int grupo = (int)Enumeradores.GrupoParametros.Generos;
                int NoEliminado = (int)Enumeradores.EsEliminado.No;

                List<Parametro> Listado = (from a in ctx.Parametros where a.GrupoId == grupo select a).ToList();
                int parametroId = (from a in Listado orderby a.ParametroId descending select a.ParametroId).FirstOrDefault() + 1;
                int orden = (from a in Listado orderby a.Orden descending select a.Orden).FirstOrDefault() + 1;

                Parametro Parametro = (from a in Listado where a.Valor1 == Descripcion select a).FirstOrDefault();

                if(Parametro != null)
                    return null;

                Parametro = new Parametro()
                {
                    GrupoId = grupo,
                    ParametroId = parametroId,
                    Valor1 = Descripcion,
                    PadreParametroId = -1,
                    Orden = orden,
                    EsEliminado = NoEliminado,
                    FechaGraba = DateTime.Now,
                    UsuGraba = UsuarioID
                };

                ctx.Parametros.Add(Parametro);

                int rows = ctx.SaveChanges();
                if (rows > 0)
                    return Parametro;

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
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending select a).ToList();

            return result;
        }

        public bool InsertNewPerson(Persona Persona, Usuario Usuario, int UsuarioID)
        {
            try
            {
                if ((from a in ctx.Personas where a.TipoDocumentoId == Persona.TipoDocumentoId select a).FirstOrDefault() != null || (from a in ctx.Usuarios where a.NombreUsuario == Usuario.NombreUsuario select a).FirstOrDefault() != null)
                    return false;

                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

                Persona.UsuGraba = UsuarioID;
                Persona.FechaGraba = DateTime.Now;
                Persona.EsEliminado = NoEsEliminado;

                ctx.Personas.Add(Persona);
                int rows = ctx.SaveChanges();

                Usuario.UsuGraba = UsuarioID;
                Usuario.FechaGraba = DateTime.Now;
                Usuario.EsEliminado = NoEsEliminado;
                Usuario.FechaCaduca = DateTime.Now.AddYears(1);
                Usuario.Contrasenia = Utils.Encrypt(Usuario.Contrasenia);
                Usuario.RespuestaSecreta = Utils.Encrypt(Usuario.RespuestaSecreta);
                Usuario.PersonaId = Persona.PersonaId;

                ctx.Usuarios.Add(Usuario);

                rows += ctx.SaveChanges();

                if (rows > 1)
                    return true;

                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public List<Parametro> GetTipoDocumentos()
        {
            int grupo = (int)Enumeradores.GrupoParametros.TipoDocumentos;
            int NoEliminado = (int)Enumeradores.EsEliminado.No;
            List<Parametro> result = (from a in ctx.Parametros where a.EsEliminado == NoEliminado && a.GrupoId == grupo orderby a.Orden ascending select a).ToList();

            return result;
        }
    }
}
