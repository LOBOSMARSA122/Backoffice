using BL;
using System.Collections.Generic;
using System.Web.Http;

namespace ObackOfficeAPI.Controllers.Person
{
    public class PersonController : ApiController
    {
        private PersonRepository pr = new PersonRepository();

        [HttpGet]
        public IEnumerable<BE.Person> GetAll()
        {
            return pr.GetAll();
        }
    }
}
