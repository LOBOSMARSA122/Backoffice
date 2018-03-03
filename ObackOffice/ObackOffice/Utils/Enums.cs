using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Utils
{
    public class Enums
    {
       public enum Parametros
        {
            Roles = 101,
            TipoDocumento = 102,
            Genero = 103,
            TipoEmpresa = 104,
            PreguntaTaller = 105,
            Sedes =106,
            Condicion = 107,
            Asistencia = 109
        }

        public enum RecordType
        {
            Temporal = 1,
            NoTemporal = 2
        }

        public enum RecordStatus
        {
            Grabado = 1,
            Agregar = 2,
            Editar = 3,
            Eliminar = 4
        }
    }
}