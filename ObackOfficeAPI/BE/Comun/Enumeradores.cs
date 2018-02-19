using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Comun
{
    public class Enumeradores
    {
        public enum GrupoParametros
        {
            Roles = 100,
            TipoDocumentos = 101,
            Generos = 102,
            TipoEmpresas = 104,
            PreguntasTaller = 105,
            Sedes = 106,
            Condición = 107,
            Colores = 108,
            Asistencia = 109,
            Correo = 110
        }

        public enum EsEliminado
        {
            No = 0,
            Si = 1
        }

        public enum Condicion
        {
            Aprobado = 1,
            Desaprobado = 2,
            PorIniciar = 3,
        }

        public enum Asistencia
        {
            Asistio = 1,
            Falto = 0,
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

        public enum Roles
        {
            Administrador = 1,
            Capacitador = 2,
            ClienteMinera = 3,
            Representante = 4,
            Empleado = 5
        }

        public enum Correo
        {
            HostSMTP = 1,
            CorreoSistema = 2,
            ClaveCorreo = 3,
            MailRegistroEmpleado = 4
        }
    }
}
