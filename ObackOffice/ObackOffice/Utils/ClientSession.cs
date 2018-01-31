using ObackOffice.Models.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Utils
{
    public class ClientSession
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaCaduca { get; set; }
        public int RolId { get; set; }
        public string Rol { get; set; }
        public List<Autorizacion> Autorizacion { get; set; }


        //public int UsuarioId
        //{
        //    get { return int.Parse(_objData[0]); }
        //    set { _objData[0] = value.ToString(); }
        //}

        //public string Usuario
        //{
        //    get { return _objData[1]; }
        //    set { _objData[1] = value.ToString(); }
        //}

        //public int RolId
        //{
        //    get { return int.Parse(_objData[2]); }
        //    set { _objData[2] = value.ToString(); }
        //}

        //private List<string> _objData;

        //public ClientSession()
        //{
        //    _objData = new List<string>(5);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        _objData.Add(null);
        //    }
        //}

        //public List<string> GetAsList()
        //{
        //    return _objData;
        //}

        //public string[] GetAsArray()
        //{
        //    return _objData.ToArray();
        //}
    }
}