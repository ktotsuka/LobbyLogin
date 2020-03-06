using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace VisitDataBase
{
    public class Visit
    {
        public Visitor Visitor { get; set; }
        public Employee Employee { get; set; }
        public DateTime Time { get; set; }
        [Key] public string Id { get; set; }
    }

    public class VisitContext : DbContext
    {
        public DbSet<EmployeeWrapper> Employees { get; set; }
        public DbSet<VisitorWrapper> Visitors { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}
