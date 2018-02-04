using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Comun
{
    [Table("tblGenero")]
    public class Genero
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int EsEliminado { get; set; }
    }
}
