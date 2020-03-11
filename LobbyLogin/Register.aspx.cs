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
using static SignInMail.Mail;

namespace LobbyLogin
{
    public partial class Register : Page
    {
        public List<EmployeeWrapper> Employees { get; set; }
        public List<VisitorWrapper> Visitors { get; set; }
        const string BackUpLocation = @"C:\Users\bavge\OneDrive\Documents\DatabaseBackup\";

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<EmployeeWrapper>();
            Visitors = new List<VisitorWrapper>();
            ProcessInputs();
            UpdateEmployeeList();
            UpdateVisitorList();
            UpdateEmployeeDropDownList();
            UpdateVisitorDropDownList();
        }

        private void ProcessInputs()
        {
            lastName.Text = lastName.Text.Trim();
            firstName.Text = firstName.Text.Trim();
            companyName.Text = companyName.Text.Trim();
            emailAddress.Text = emailAddress.Text.ToLower().Trim();
            phoneNumber.Text = phoneNumber.Text.Trim();
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

            if (lastName.Text != "")
            {
                using (var db = new VisitContext())
                {
                    var query = from b in db.Visitors
                                where b.Visitor.LastName.StartsWith(lastName.Text)
                                orderby b.Visitor.FirstName, b.Visitor.CompanyName
                                select b;
                    foreach (var b in query)
                    {
                        Visitors.Add(b);
                    }
                }
            }
        }        

        private void UpdateEmployeeDropDownList()
        {
            int selected = EmployeesDropDownList.SelectedIndex;
            EmployeesDropDownList.Items.Clear();
            EmployeesDropDownList.Items.Add("Please select an employee");
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
            else if (Visitors.Count > 0)
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

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (VerifyInputs())
            {
                Employee employee;
                if (VisitingAnEmployeeDropDownList.SelectedIndex == 1)
                {
                    employee = Employees[EmployeesDropDownList.SelectedIndex - 1].Employee;
                }
                else
                {
                    employee = null;
                }
                
                Visitor visitor = new Visitor
                {
                    FirstName = firstName.Text,
                    LastName = lastName.Text,
                    CompanyName = companyName.Text,
                    EmailAddress = emailAddress.Text,
                    PhoneNumber = phoneNumber.Text,
                };

                AddVisitorToDatabase(visitor);

                DateTime time = DateTime.Now;
                Visit new_visit = new Visit
                {
                    Visitor = visitor,
                    Employee = employee,
                    Time = time,
                    Purpose = PurposeDropDownList.SelectedValue,
                    Id = time.ToString()
                };
                AddVisitToDatabase(new_visit);
                AddVisitToWaitingList(new_visit);

                List<string> addresses;
                string message;
                if (employee != null)
                {
                    string numeric_phone_number = new String(employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                    addresses = GetPhoneEmailAddresses(numeric_phone_number);
                    addresses.Add(employee.EmailAddress);
                    message = $"{visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} has arrived for you";
                    SendEmail(addresses, message);
                }
                else
                {
                    foreach (Employee general_employee in VisitDataBase.GeneralEmployee.GeneralEmployees)
                    {
                        string numeric_phone_number = new String(general_employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                        addresses = GetPhoneEmailAddresses(numeric_phone_number);
                        addresses.Add(general_employee.EmailAddress);

                        message = $"{visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} has arrived";
                        SendEmail(addresses, message);
                    }                    
                }

                HandleBackup();

                Response.Redirect("ThankYou.aspx");
            }
        }

        private void AddVisitToWaitingList(Visit visit)
        {
            using (var mutex = new Mutex(false, WaitListMutexName))
            {
                mutex.WaitOne();

                List<Visit> visits = GetVisitFromFile(WaitListFileLocation);
                visits.Add(visit);
                UpdateWaitListFile(visits);

                mutex.ReleaseMutex();
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
            List<EmployeeWrapper> employees = new List<EmployeeWrapper>();

            using (var db = new VisitContext())
            {
                employees = db.Employees.ToList();
            }

            string employees_string = GetEmployeesString(employees);

            string fileName = BackUpLocation + "employees.csv";
            File.WriteAllText(fileName, employees_string);
        }

        protected void BackupVisitors()
        {
            List<VisitorWrapper> visitors = new List<VisitorWrapper>();

            using (var db = new VisitContext())
            {
                visitors = db.Visitors.ToList();
            }

            string visitors_string = GetVisitorsString(visitors);

            string fileName = BackUpLocation + "visitors.csv";
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

            string fileName = BackUpLocation + "visits.csv";
            File.WriteAllText(fileName, visits_string);
        }

        protected void LastNameOnTextChanged(object sender, EventArgs e)
        {
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

            VisitorsDropDownList.SelectedIndex = 0;

            UpdateVisitorList();
            UpdateVisitorDropDownList();
        }

        private void AddVisitorToDatabase(Visitor visitor)
        {
            VisitorWrapper visitor_w = new VisitorWrapper
            {
                Visitor = visitor,
                Id = GetVisitorId(visitor)
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
                submitMessage.Text = "No comma allowed";
                return false;
            }
            else if((emailAddress.Text != "")
                     &&
                    (!IsValidEmail(emailAddress.Text)))
            {
                submitMessage.Text = "Invalid email addrress";
                return false;
            }
            else if (PurposeDropDownList.SelectedIndex == 0)
            {
                submitMessage.Text = "Purpose for visit must be selected";
                return false;
            }
            else if (VisitingAnEmployeeDropDownList.SelectedIndex == 0)
            {
                submitMessage.Text = "Please answer if you are visiting a specific employee";
                return false;
            }
            else if ((VisitingAnEmployeeDropDownList.SelectedIndex == 1) && (EmployeesDropDownList.SelectedIndex == 0))
            {
                submitMessage.Text = "An employee must be selected";
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