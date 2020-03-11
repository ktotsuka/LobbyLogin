using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace VisitDataBase
{
    public class Visitor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class VisitorWrapper
    {
        public Visitor Visitor { get; set; }
        [Key] public string Id { get; set; }
    }
}
