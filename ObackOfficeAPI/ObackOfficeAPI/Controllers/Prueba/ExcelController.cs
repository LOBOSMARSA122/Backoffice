using System.Web.Http;
using BE.Comun;
using BL;

namespace ObackOfficeAPI.Controllers.Prueba
{
    public class ExcelController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Prueba(MultiDataModel data)
        {
            Excel Excel = new Excel();

            return Ok(Excel.Prueba());
        }
    }
}