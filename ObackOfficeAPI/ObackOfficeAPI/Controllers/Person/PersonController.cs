using BL;
using System.Collections.Generic;
using System.Web.Http;
using BE.Comun;

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
            List<Parametro> result = pr.GetGeneros();
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult InsertGenero(Parametro Genero)
        {
            if (string.IsNullOrWhiteSpace(Genero.Valor1))
                return BadRequest("Descripción Inválida");

            Parametro response = pr.InsertGenero(Genero.Valor1);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            List<Parametro> result = pr.GetRoles();
            return Ok(result);
        }
    }
}
