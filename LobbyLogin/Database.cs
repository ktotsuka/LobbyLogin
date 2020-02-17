using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace LobbyLogin
{
    public class Visit
    {
        public string Guest { get; set; }
        public string Employee { get; set; }
        public DateTime Time { get; set; }
        [Key] public int Code { get; set; }
    }

    public class VisitContext : DbContext
    {
        public DbSet<Visit> Visits { get; set; }
    }
}