using BL;
using System.Collections.Generic;
using System.Web.Http;
using BE.Comun;

namespace ObackOfficeAPI.Controllers.Perfiles
{
    public class PerfilesController : ApiController
    {
        private PerfilesRepository pr = new PerfilesRepository();

        [HttpGet]
        public IHttpActionResult GetTreeView(int id)
        {
            List<TreeView> result = pr.GetTreeData(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
