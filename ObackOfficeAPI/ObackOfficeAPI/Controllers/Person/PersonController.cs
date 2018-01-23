using BL;
using System.Collections.Generic;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Person
{
    public class PersonController : ApiController
    {
        private PersonRepository pr = new PersonRepository();

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<BE.Person> result = pr.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
