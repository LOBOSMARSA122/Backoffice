using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BL
{
    public class PersonRepository : IPerson
    {
        private List<Person> _lPerson = new List<Person>();
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetAll()
        {
            try
            {
                var query = (from a in ctx.People select a).ToList();
                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public Person GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save(Person oPerson)
        {
            throw new NotImplementedException();
        }
    }
}
