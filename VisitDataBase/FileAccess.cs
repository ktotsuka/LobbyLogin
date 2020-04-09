using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace VisitDataBase
{
    public static class DataAccess
    {
        public const int EmployeeNumOfFields = 4;
        public const int VisitorNumOfFields = 5;
        public const int VisitNumOfFields = 11;
        public const string WaitListFileLocation = @"C:\Temp\visit_waiting_list.txt";
        public const string DeliveryNotificationListFileLocation = @"C:\Temp\delivery_notification_list.txt";
        public const string WaitListMutexName = @"Global\WaitListMutex";
        public const int FileAccessRetryWait = 100;

        public static string GetVisitsString(List<Visit> visits)
        {
            string header = @"""Employee last name"",""Employee first name"",""Employee email address"",""Employee cell phone number"""
                + @",""Visitor last name"",""Visitor First name"",""Visitor company name"",""Visitor email address"",""Visitor phone number"""
                + @", ""Time"",""Purpose of visit""";
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
                    string.Format(@"""{0}""", i.Time.ToString()),
                    string.Format(@"""{0}""", i.Purpose)));
            }
            return sb.ToString();
        }

        public static List<Visit> GetVisitFromFile(string fileName)
        {
            string[] Lines;
            while (true)
            {
                try
                {
                    Lines = File.ReadAllLines(fileName);
                    break;
                }
                catch
                {
                    Thread.Sleep(FileAccessRetryWait);
                }
            }
            string[] Fields;

            // Remove Header line
            Lines = Lines.Skip(1).ToArray();

            List<Visit> visits = new List<Visit>();
            foreach (var line in Lines)
            {
                Fields = line.Split(new char[] { ',' });
                if (Fields.Count() != VisitNumOfFields)
                {
                    return visits;
                }

                Employee employee = new Employee
                {
                    LastName = Fields[0].Replace("\"", ""),
                    FirstName = Fields[1].Replace("\"", ""),
                    EmailAddress = Fields[2].Replace("\"", ""),
                    CellPhoneNumber = Fields[3].Replace("\"", ""),
                };

                Visitor visitor = new Visitor
                {
                    LastName = Fields[4].Replace("\"", ""),
                    FirstName = Fields[5].Replace("\"", ""),
                    CompanyName = Fields[6].Replace("\"", ""),
                    EmailAddress = Fields[7].Replace("\"", ""),
                    PhoneNumber = Fields[8].Replace("\"", ""),
                };

                string time = Fields[9].Replace("\"", "");
                string purpose = Fields[10].Replace("\"", "");

                visits.Add(
                    new Visit
                    {
                        Employee = employee,
                        Visitor = visitor,
                        Time = DateTime.Parse(time),
                        Purpose = purpose,
                        Id = time
                    });
            }
            return visits;
        }

        public static void UpdateWaitListFile(List<Visit> visits)
        {
            while (true)
            {
                try
                {
                    FileStream waitListFile = new FileStream(WaitListFileLocation, FileMode.Create);
                    StreamWriter sw = new StreamWriter(waitListFile);

                    string visit_str = GetVisitsString(visits);

                    sw.Write(visit_str);
                    sw.Close();
                    break;
                }
                catch
                {
                    Thread.Sleep(FileAccessRetryWait);
                }
            }
        }

        public static string GetEmployeesString(List<EmployeeWrapper> employees)
        {
            string header = @"""Last name"",""First name"",""Email address"",""Cell phone number""";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);

            foreach (var i in employees)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.Employee.LastName),
                    string.Format(@"""{0}""", i.Employee.FirstName),
                    string.Format(@"""{0}""", i.Employee.EmailAddress),
                    string.Format(@"""{0}""", i.Employee.CellPhoneNumber)));
            }
            return sb.ToString();
        }

        public static string GetVisitorsString(List<VisitorWrapper> visitors)
        {
            string header = @"""Last name"",""First name"",""Company name"",""Email address"",""Phone number""";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            foreach (var i in visitors)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.Visitor.LastName),
                    string.Format(@"""{0}""", i.Visitor.FirstName),
                    string.Format(@"""{0}""", i.Visitor.CompanyName),
                    string.Format(@"""{0}""", i.Visitor.EmailAddress),
                    string.Format(@"""{0}""", i.Visitor.PhoneNumber)));
            }
            return sb.ToString();
        }

        public static string GetVisitorId(Visitor visitor)
        {
            return visitor.FirstName + visitor.LastName + visitor.CompanyName;
        }

        public static string GetEmployeeId(Employee employee)
        {
            return employee.FirstName + employee.LastName + employee.EmailAddress;
        }
    }
}
