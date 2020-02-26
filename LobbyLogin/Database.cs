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
    }

    public class EmployeeWrapper
    {
        public Employee Employee { get; set; }
        [Key] public string Id { get; set; }
    }

    public class Visitor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string HostId { get; set; }
    }

    public class VisitorWrapper
    {
        public Visitor Visitor { get; set; }
        [Key] public string Id { get; set; }
    }

    public class Visit
    {
        public Visitor Visitor { get; set; }
        public Employee Employee { get; set; }
        public string Time { get; set; }
        [Key] public string Id { get; set; }
    }

    public class VisitContext : DbContext
    {
        public DbSet<EmployeeWrapper> Employees { get; set; }
        public DbSet<VisitorWrapper> Visitors { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}