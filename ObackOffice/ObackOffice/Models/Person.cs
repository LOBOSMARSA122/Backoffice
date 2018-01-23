using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ObackOffice.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public int GenderId { get; set; }
    }
}