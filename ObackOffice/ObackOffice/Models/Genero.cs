using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObackOffice.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int EsEliminado { get; set; }
    }
}