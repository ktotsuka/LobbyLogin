using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace VisitDataBase
{
    public static class DataAccess
    {
        public const int EmployeeNumOfFields = 5;
        public const int VisitorNumOfFields = 7;
        public const int VisitNumOfFields = 12;
        public const string WaitListFileLocation = @"C:\Temp\visit_waiting_list.txt";
        public static object waitingVisitFileLock = new Object();

        public static string GetVisitsString(List<Visit> visits)
        {
            string header = @"""Employee last name"",""Employee first name"",""Employee email address"",""Employee cell phone number"""
                + @",""Visitor last name"",""Visitor First name"",""Visitor company name"",""Visitor email address"",""Visitor phone number"",""Visitor host ID"""
                + @", ""Time"",""ID""";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            foreach (var i in visits)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.Employee.LastName),
                    string.Format(@"""{0}""", i.Employee.FirstName),
                    string.Format(@"""{0}""", i.Employee.EmailAddress),
                    string.Format(@"""{0}""", i.Employee.CellPhoneNumber),
                    string.Format(@"""{0}""", i.Visitor.LastName),
                    string.Format(@"""{0}""", i.Visitor.FirstName),
                    string.Format(@"""{0}""", i.Visitor.CompanyName),
                    string.Format(@"""{0}""", i.Visitor.EmailAddress),
                    string.Format(@"""{0}""", i.Visitor.PhoneNumber),
                    string.Format(@"""{0}""", i.Visitor.HostId),
                    string.Format(@"""{0}""", i.Time),
                    string.Format(@"""{0}""", i.Id)));
            }
            return sb.ToString();
        }

        public static List<Visit> GetVisitFromFile(string fileName)
        {
            string[] Lines = File.ReadAllLines(fileName);
            string[] Fields;

            //Remove Header line
            Lines = Lines.Skip(1).ToArray();
            List<Visit> visits = new List<Visit>();
            foreach (var line in Lines)
            {
                Fields = line.Split(new char[] { ',' });
                if (Fields.Count() != VisitNumOfFields)
                {
                    return visits;
                }
                visits.Add(
                    new Visit
                    {
                        Employee = new Employee
                        {
                            LastName = Fields[0].Replace("\"", ""), // removed "" 
                            FirstName = Fields[1].Replace("\"", ""), // removed "" 
                            EmailAddress = Fields[2].Replace("\"", ""), // removed "" 
                            CellPhoneNumber = Fields[3].Replace("\"", ""), // removed "" 
                        },
                        Visitor = new Visitor
                        {
                            LastName = Fields[4].Replace("\"", ""),
                            FirstName = Fields[5].Replace("\"", ""),
                            CompanyName = Fields[6].Replace("\"", ""),
                            EmailAddress = Fields[7].Replace("\"", ""),
                            PhoneNumber = Fields[8].Replace("\"", ""),
                            HostId = Fields[9].Replace("\"", "")
                        },
                        Time = Fields[10].Replace("\"", ""),
                        Id = Fields[11].Replace("\"", "")
                    });
            }
            return visits;
        }
    }
}
