using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.IO;
using VisitDataBase;
using SignInMail;
using static VisitDataBase.DataAccess;

namespace LobbyLogin
{
    public partial class WithAppointment : Page
    {
        public List<EmployeeWrapper> Employees { get; set; }
        public List<VisitorWrapper> Visitors { get; set; }
        public List<Visit> WaitingVisits { get; set; }
        const string BackUpLocation = @"C:\Users\bavge\OneDrive\Documents\DatabaseBackup\";

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<EmployeeWrapper>();
            Visitors = new List<VisitorWrapper>();
            WaitingVisits = new List<Visit>();
            UpdateEmployeeList();
            UpdateVisitorList();
            UpdateWaitingVisitList();
            UpdateEmployeeDropDownList();
            UpdateVisitorDropDownList();
            UpdateWaitingVisitDropDownList();
        }

        private void UpdateEmployeeList()
        {
            Employees.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Employees
                            orderby b.Employee.LastName, b.Employee.FirstName
                            select b;
                foreach (var b in query)
                {
                    Employees.Add(b);
                }
            }
        }

        private void UpdateVisitorList()
        {
            Visitors.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Visitors
                            where b.Visitor.LastName.StartsWith(lastName.Text.Trim())
                            orderby b.Visitor.FirstName, b.Visitor.CompanyName
                            select b;
                foreach (var b in query)
                {
                    Visitors.Add(b);
                }
            }
        }

        private void UpdateWaitingVisitList()
        {
            WaitingVisits.Clear();

            List<Visit> visits;
            lock (waitingVisitFileLock)
            {
                visits = GetVisitFromFile(WaitListFileLocation);
            }

            foreach (var i in visits)
            {
                WaitingVisits.Add(i);
            }
        }

        private void UpdateEmployeeDropDownList()
        {
            int selected = EmployeesDropDownList.SelectedIndex;
            EmployeesDropDownList.Items.Clear();
            foreach (var employee in Employees)
            {
                Employee emp = employee.Employee;
                string employee_info = $"{emp.FirstName} {emp.LastName}";
                EmployeesDropDownList.Items.Add(employee_info);
            }

            if (selected >= 0)
            {
                EmployeesDropDownList.SelectedIndex = selected;
            }
        }

        private void UpdateVisitorDropDownList()
        {
            int selected = VisitorsDropDownList.SelectedIndex;

            VisitorsDropDownList.Items.Clear();

            if (Visitors.Count == 0)
            {
                if (lastName.Text == "")
                {
                    VisitorsDropDownList.Items.Add("Please fill out the last name");
                }
                else
                {
                    VisitorsDropDownList.Items.Add("No record found");
                }
            }
            else if (Visitors.Count > 1)
            {
                VisitorsDropDownList.Items.Add("Select a visitor");
            }

            foreach (var visitor in Visitors)
            {
                Visitor vis = visitor.Visitor;
                string visitor_info = $"{vis.FirstName} {vis.LastName} from {vis.CompanyName}";
                VisitorsDropDownList.Items.Add(visitor_info);
            }

            if (selected >= 1 && (VisitorsDropDownList.Items.Count > selected))
            {
                VisitorsDropDownList.SelectedIndex = selected;
            }
        }

        private void UpdateWaitingVisitDropDownList()
        {
            int selected = WaitingVisitDropDownList.SelectedIndex;

            WaitingVisitDropDownList.Items.Clear();

            if (WaitingVisits.Count == 0)
            {
                WaitingVisitDropDownList.Items.Add("No waiting visitor");
            }

            foreach (var visit in WaitingVisits)
            {
                string visitor_info = $"{visit.Visitor.FirstName} {visit.Visitor.LastName} from {visit.Visitor.CompanyName} "
                                     + $"visiting {visit.Employee.FirstName} {visit.Employee.LastName} on {visit.Time}";
                WaitingVisitDropDownList.Items.Add(visitor_info);
            }

            if (selected >= 1 && (WaitingVisitDropDownList.Items.Count > selected))
            {
                WaitingVisitDropDownList.SelectedIndex = selected;
            }
        }

        protected void RemoveWaitingVisitButton_Click(object sender, EventArgs e)
        {
            try
            {
                WaitingVisits.RemoveAt(WaitingVisitDropDownList.SelectedIndex);
            }
            catch
            {
                return;
            }
            UpdateVisitWaitingList();

            UpdateWaitingVisitList();
            UpdateWaitingVisitDropDownList();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (VerifyInputs())
            {
                Employee employee = Employees[EmployeesDropDownList.SelectedIndex].Employee;
                string time = DateTime.Now.ToString();

                Visitor visitor = new Visitor
                {
                    FirstName = firstName.Text.Trim(),
                    LastName = lastName.Text.Trim(),
                    CompanyName = companyName.Text.Trim(),
                    EmailAddress = emailAddress.Text.ToLower().Trim(),
                    PhoneNumber = phoneNumber.Text.Trim(),
                    HostId = employee.FirstName + employee.LastName + employee.EmailAddress
                };

                AddVisitorToDatabase(visitor);

                Visit new_visit = new Visit
                {
                    Visitor = visitor,
                    Employee = employee,
                    Time = time,
                    Id = $"{visitor.LastName}" + $"{visitor.FirstName}" + $"{visitor.CompanyName}"
                        + $"{employee.LastName}" + $"{employee.FirstName}" + $"{employee.EmailAddress}" + $"{time}"
                };
                AddVisitToDatabase(new_visit);
                WaitingVisits.Add(new_visit);
                UpdateVisitWaitingList();

                string numeric_phone_number = new String(employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                List<string> addresses = Mail.GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(employee.EmailAddress);

                string message = $"{visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} has arrived for you";

                /////////////
                //Mail.SendEmail(addresses, message);
                Thread.Sleep(3000);
                //////////////////

                HandleBackup();

                Response.Redirect("ThankYou.aspx");

            }
        }

        private void UpdateVisitWaitingList()
        {
            lock (waitingVisitFileLock)
            {
                FileStream waitListFile = new FileStream(WaitListFileLocation, FileMode.Create);
                StreamWriter sw = new StreamWriter(waitListFile);

                string visit_str = GetVisitsString(WaitingVisits);

                sw.WriteLine(visit_str);
                sw.Close();
            }
        }

        protected void HandleBackup()
        {
            BackupEmployees();
            BackupVisitors();
            BackupVisits();
        }

        protected void BackupEmployees()
        {
            string employees_string = AdminTool.GetEmployeesString();

            string fileName = BackUpLocation + "employees" + ".csv";
            File.WriteAllText(fileName, employees_string);
        }

        protected void BackupVisitors()
        {
            string visitors_string = AdminTool.GetVisitorsString();

            string fileName = BackUpLocation + "visitors" + ".csv";
            File.WriteAllText(fileName, visitors_string);
        }

        protected void BackupVisits()
        {
            List<Visit> visits = new List<Visit>();

            using (var db = new VisitContext())
            {
                visits = db.Visits.ToList();
            }

            string visits_string = GetVisitsString(visits);

            string fileName = BackUpLocation + "visits" + ".csv";
            File.WriteAllText(fileName, visits_string);
        }

        protected void LastNameOnTextChanged(object sender, EventArgs e)
        {
            if (Visitors.Count == 1)
            {
                Visitor visitor = Visitors.First().Visitor;

                lastName.Text = visitor.LastName;
                firstName.Text = visitor.FirstName;
                companyName.Text = visitor.CompanyName;
                emailAddress.Text = visitor.EmailAddress;
                phoneNumber.Text = visitor.PhoneNumber;

                UpdateVisitorList();
                UpdateVisitorDropDownList();

                int index_employee = Employees.FindIndex(b => b.Id == visitor.HostId);
                EmployeesDropDownList.SelectedIndex = index_employee;

                int index_visitor = Visitors.FindIndex(b => b.Id == (visitor.FirstName + visitor.LastName + visitor.CompanyName));
                VisitorsDropDownList.SelectedIndex = index_visitor;
            }
        }

        protected void VisitorsOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (VisitorsDropDownList.SelectedIndex == 0)
                return;

            Visitor visitor;

            try
            {
                visitor = Visitors[VisitorsDropDownList.SelectedIndex - 1].Visitor;
            }
            catch
            {
                return;
            }

            lastName.Text = visitor.LastName;
            firstName.Text = visitor.FirstName;
            companyName.Text = visitor.CompanyName;
            emailAddress.Text = visitor.EmailAddress;
            phoneNumber.Text = visitor.PhoneNumber;

            UpdateVisitorList();
            UpdateVisitorDropDownList();

            int index_employee = Employees.FindIndex(b => b.Id == visitor.HostId);
            EmployeesDropDownList.SelectedIndex = index_employee;
        }

        private void AddVisitorToDatabase(Visitor visitor)
        {
            VisitorWrapper visitor_w = new VisitorWrapper
            {
                Visitor = visitor,
                Id = visitor.FirstName + visitor.LastName + visitor.CompanyName
            };

            using (var db = new VisitContext())
            {
                var visitors_to_remove = db.Visitors.Where(b => b.Id == visitor_w.Id);
                foreach (VisitorWrapper vis in visitors_to_remove)
                {
                    db.Visitors.Remove(vis);
                }

                db.Visitors.Add(visitor_w);
                db.SaveChanges();
            }
        }

        private void AddVisitToDatabase(Visit new_visit)
        {
            using (var db = new VisitContext())
            {
                db.Visits.Add(new_visit);
                db.SaveChanges();
            }
        }

        private bool VerifyInputs()
        {
            if ((firstName.Text == "")
                ||
                (lastName.Text == "")
                ||
                (companyName.Text == ""))
            {
                submitMessage.ForeColor = System.Drawing.Color.Red;
                submitMessage.Text = "All required fields need to be filled";
                return false;
            }
            else if ((firstName.Text.Contains(","))
                ||
                (lastName.Text.Contains(","))
                ||
                (companyName.Text.Contains(","))
                ||
                (emailAddress.Text.Contains(","))
                ||
                (phoneNumber.Text.Contains(",")))
            {
                submitMessage.ForeColor = System.Drawing.Color.Red;
                submitMessage.Text = "No comma allowed";
                return false;
            }
            else
            {
                submitMessage.Text = "";
                return true;
            }
        }
    }
}