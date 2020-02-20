using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace LobbyLogin
{
    public partial class AdminTool : System.Web.UI.Page
    {
        public const string correctPassword = "Georgetown@4321!";
        public const int MaxTextLength = 50;
        public List<EmployeeWrapper> Employees { get; set; }
        public List<Visit> Visits { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<EmployeeWrapper>();
            Visits = new List<Visit>();
            UpdateEmployeeList();
            UpdateVisitList();
        }

        protected void AdminPasswordSubmitButton_Click(object sender, EventArgs e)
        {
            if (adminPassword.Text == correctPassword)
            {
                submitErrorMessage.Text = "";
                AdminPasswordTable.Visible = false;
                AddEmployeeTable.Visible = true;
                RemoveEmployeeTable.Visible = true;
                RemoveVisitTable.Visible = true;

                UpdateEmployeeDropDownList();
                UpdateVisitDropDownList();
            }
            else
            {
                submitErrorMessage.Text = "password is incorrect";
            }
        }

        protected void AddEmployeeButton_Click(object sender, EventArgs e)
        {
            if (VerifyAddEmployeeInputs())
            {
                using (var db = new VisitContext())
                {
                    EmployeeWrapper new_employee = new EmployeeWrapper
                    {
                        Employee = new Employee
                        {
                            FirstName = firstName.Text.Trim(),
                            LastName = lastName.Text.Trim(),
                            EmailAddress = emailAddress.Text.ToLower().Trim(),
                            CellPhoneNumber = cellPhoneNumber.Text.Trim()
                        },                        
                        Id = firstName.Text.Trim() + lastName.Text.Trim() + emailAddress.Text.ToLower().Trim()
                    };

                    try
                    {
                        db.Employees.Add(new_employee);
                        db.SaveChanges();
                        addEmployeeErrorMessage.Text = 
                            $"Added: {new_employee.Employee.FirstName} {new_employee.Employee.LastName}, " +
                            $"{new_employee.Employee.EmailAddress}, {new_employee.Employee.CellPhoneNumber}";
                        UpdateEmployeeList();
                        UpdateEmployeeDropDownList();
                    }
                    catch
                    {
                        addEmployeeErrorMessage.Text = "The employee already exists in the system!";
                    }
                }
            };
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

        private void UpdateVisitList()
        {
            Visits.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Visits
                            orderby b.Employee.LastName, b.Employee.FirstName, b.Visitor.FirstName, b.Visitor.LastName, b.Visitor.CompanyName
                            select b;
                foreach (var b in query)
                {
                    Visits.Add(b);
                }
            }
        }

        private void UpdateEmployeeDropDownList()
        {
            EmployeesDropDownList.Items.Clear();
            foreach (var employee in Employees)
            {
                Employee emp = employee.Employee;
                string employee_info = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee_info);
            }
        }

        private void UpdateVisitDropDownList()
        {
            VisitsDropDownList.Items.Clear();
            foreach (var visit in Visits)
            {
                Employee emp = visit.Employee;
                Visitor vis = visit.Visitor;
                string visit_info = $"{emp.FirstName} {emp.LastName} ({emp.EmailAddress},{emp.CellPhoneNumber}) was visited by {vis.FirstName} {vis.LastName} ({vis.EmailAddress},{vis.PhoneNumber}) from {vis.CompanyName} on {visit.Time}";
                VisitsDropDownList.Items.Add(visit_info);
            }
        }

        private bool VerifyAddEmployeeInputs()
        {
            if ((firstName.Text == "")
                ||
                (lastName.Text == "")
                ||
                (emailAddress.Text == "")
                ||
                (cellPhoneNumber.Text == ""))
            {
                addEmployeeErrorMessage.Text = "All required fields need to be filled";
                return false;
            }
            else if (!IsValidEmail(emailAddress.Text))
            {
                addEmployeeErrorMessage.Text = "Invalid email address";
                return false;
            }
            else
            {
                submitErrorMessage.Text = "";
                return true;
            }
        }

        public static bool IsValidEmail(string email_address)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email_address);
                return addr.Address == email_address;
            }
            catch
            {
                return false;
            }
        }

        protected void RemoveEmployeeButton_Click(object sender, EventArgs e)
        {
            EmployeeWrapper selected_employee;

            try
            {
                selected_employee = Employees[EmployeesDropDownList.SelectedIndex];
            }
            catch
            {
                removeEmployeeMessage.Text = "No employee selected";
                return;
            }

            using (var db = new VisitContext())
            {
                EmployeeWrapper employee_to_remove = (EmployeeWrapper)db.Employees.Where(b => b.Id == selected_employee.Id).First();
                db.Employees.Remove(employee_to_remove);
                db.SaveChanges();
                Employee emp = employee_to_remove.Employee;
                removeEmployeeMessage.Text = $"Removed: {emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
            }
            UpdateEmployeeList();
            UpdateEmployeeDropDownList();
        }

        protected void RemoveVisitButton_Click(object sender, EventArgs e)
        {
            Visit selected_visit;

            try
            {
                selected_visit = Visits[VisitsDropDownList.SelectedIndex];
            }
            catch
            {
                removeVisitMessage.Text = "No visit selected";
                return;
            }

            using (var db = new VisitContext())
            {
                Visit visit_to_remove = (Visit)db.Visits.Where(b => b.Id == selected_visit.Id).First();
                db.Visits.Remove(visit_to_remove);
                db.SaveChanges();
                Employee emp = visit_to_remove.Employee;
                Visitor vis = visit_to_remove.Visitor;
                removeVisitMessage.Text = 
                    $"Removed: {emp.FirstName} {emp.LastName} was visited by {vis.FirstName} {vis.LastName} from {vis.CompanyName} on {visit_to_remove.Time}";
            }
            UpdateVisitList();
            UpdateVisitDropDownList();
        }
    }
}