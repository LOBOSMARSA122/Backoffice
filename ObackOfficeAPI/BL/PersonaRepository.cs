using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   
    public class PersonaRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public byte[] getFoto (int personaId)
        {
            try
            {
                var result = (from a in ctx.Personas where a.PersonaId == personaId select a).FirstOrDefault();

                return result.Foto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
