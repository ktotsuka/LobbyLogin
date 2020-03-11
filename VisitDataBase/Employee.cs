using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace VisitDataBase
{
    public static class GeneralEmployee
    {
        private static readonly ReadOnlyCollection<Employee> generalEmployees = new ReadOnlyCollection<Employee>(new[]
        {
            new Employee() {
                 LastName = "Clark",
                 FirstName = "Jennifer",
                 EmailAddress = "jclark@toyotaagv.com",
                 CellPhoneNumber = "(859)551-1684"
            },
            new Employee() {
                 LastName = "Sparks",
                 FirstName = "Joel",
                 EmailAddress = "jsparks@toyotaagv.com",
                 CellPhoneNumber = "(606)922-7763"
            }
        });

        public static ReadOnlyCollection<Employee> GeneralEmployees
        {
            get { return generalEmployees; }
        }

        private static readonly Employee fakeEmployee = new Employee()
        {
            LastName = "Doe",
            FirstName = "John",
            EmailAddress = "",
            CellPhoneNumber = ""
        };

        public static Employee FakeEmployee
        {
            get { return fakeEmployee; }
        }
    }

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
}
