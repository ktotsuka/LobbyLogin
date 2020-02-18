using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace LobbyLogin
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CellPhoneNumber { get; set; }
        [Key] public int Code { get; set; }
    }

    public class Visitor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        [Key] public int Code { get; set; }
    }

    public class Visit
    {
        public virtual Visitor Visitor { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime Time { get; set; }
        [Key] public int Code { get; set; }
    }

    public class VisitContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}