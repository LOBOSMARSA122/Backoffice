﻿using BL;
using System.Collections.Generic;
using System.Web.Http;
using BE.Comun;
using Newtonsoft.Json;
using System.IO;

namespace ObackOfficeAPI.Controllers.Person
{
    public class PersonController : ApiController
    {
        private PersonRepository pr = new PersonRepository();

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<BE.Comun.Person> result = pr.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetGeneros()
        {
            List<Dropdownlist> result = pr.GetGeneros();
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult InsertGenero(MultiDataModel data)
        {
            if (string.IsNullOrWhiteSpace(data.String1))
                return BadRequest("Descripción Inválida");

            if (data.Int1 == 0)
                return BadRequest("Sesión Expirada");

            Parametro response = pr.InsertGenero(data.String1,data.Int1);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            List<Dropdownlist> result = pr.GetRoles();
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult InsertNewPerson(MultiDataModel data)
        {
            Persona Persona = JsonConvert.DeserializeObject<Persona>(data.String1);
            BE.Acceso.Usuario Usuario = JsonConvert.DeserializeObject<BE.Acceso.Usuario>(data.String2);

            if (string.IsNullOrWhiteSpace(Persona.ApellidoMaterno) || string.IsNullOrWhiteSpace(Persona.ApellidoPaterno) || string.IsNullOrWhiteSpace(Persona.Nombres) || Persona.TipoDocumentoId == -1 || string.IsNullOrWhiteSpace(Persona.NroDocumento) || string.IsNullOrWhiteSpace(Usuario.NombreUsuario) || string.IsNullOrWhiteSpace(Usuario.Contrasenia) || data.Int1 == 0 || Usuario.EmpresaId == -1 || Usuario.RolId == -1)
                return BadRequest("Datos Incompletos");

            bool response = pr.InsertNewPerson(Persona,Usuario,data.Int1);
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult EditPerson(MultiDataModel data)
        {
            Persona Persona = JsonConvert.DeserializeObject<Persona>(data.String1);
            BE.Acceso.Usuario Usuario = JsonConvert.DeserializeObject<BE.Acceso.Usuario>(data.String2);

            if (string.IsNullOrWhiteSpace(Persona.ApellidoMaterno) || string.IsNullOrWhiteSpace(Persona.ApellidoPaterno) || string.IsNullOrWhiteSpace(Persona.Nombres) || Persona.TipoDocumentoId == 0 || string.IsNullOrWhiteSpace(Persona.NroDocumento) || string.IsNullOrWhiteSpace(Usuario.NombreUsuario) || data.Int1 == 0 || Usuario.EmpresaId == 0)
                return BadRequest("Datos Incompletos");

            bool response = pr.EditPerson(Persona, Usuario, data.Int1);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetTipoDocumentos()
        {
            List<Dropdownlist> result = pr.GetTipoDocumentos();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPersona(int id)
        {
            Persona result = pr.GetPersona(id);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult CargaMasivaArchivo()
        {
            System.Threading.Tasks.Task<byte[]> bytes = Request.Content.ReadAsByteArrayAsync();

            MemoryStream stream = new MemoryStream(bytes.Result);
            MultiDataModel response = pr.CargaMasivaArchivo(stream);

            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
