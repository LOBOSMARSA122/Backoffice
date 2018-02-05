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
            List<Parametro> result = pr.GetRoles();
            return Ok(result);
        }
    }
}
