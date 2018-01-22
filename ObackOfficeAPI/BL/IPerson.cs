using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IPerson
    {
        IEnumerable<Person> GetAll();

        Person GetById(int id);

        bool Save(Person oPerson);

        bool Delete(int id);
    }
}
