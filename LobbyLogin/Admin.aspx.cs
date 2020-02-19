using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace LobbyLogin
{
    public partial class LogIn : System.Web.UI.Page
    {
        public const string correctPassword = "aa";
        public const int MaxTextLength = 50;
        public List<Employee> Employees { get; set; }
        public List<Visitor> Visitors { get; set; }
        public List<Visit> Visits { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<Employee>();
            Visitors = new List<Visitor>();
            Visits = new List<Visit>();
            UpdateEmployeeList();
            UpdateVisitorList();
            UpdateVisitList();
        }

        protected void AdminPasswordSubmitButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format($"password was {adminPassword.Text}"));
            if (adminPassword.Text == correctPassword)
            {
                submitErrorMessage.Text = "";
                AdminPasswordTable.Visible = false;
                AddEmployeeTable.Visible = true;
                RemoveEmployeeTable.Visible = true;
                RemoveVisitorTable.Visible = true;
                RemoveVisitTable.Visible = true;

                UpdateEmployeeDropDownList();
                UpdateVisitorDropDownList();
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
                    Employee new_employee = new Employee
                    {
                        FirstName = firstName.Text.Trim(),
                        LastName = lastName.Text.Trim(),
                        EmailAddress = emailAddress.Text.ToLower().Trim(),
                        CellPhoneNumber = cellPhoneNumber.Text.Trim(),
                        Id = firstName.Text.Trim() + lastName.Text.Trim() + emailAddress.Text.ToLower().Trim()
                    };

                    var duplicate_employees = db.Employees.Where(b => b.Id == new_employee.Id);
                    if (duplicate_employees.Count() == 0)
                    {
                        db.Employees.Add(new_employee);
                        db.SaveChanges();
                        addEmployeeErrorMessage.Text = $"Added: {new_employee.FirstName} {new_employee.LastName}, {new_employee.EmailAddress}, {new_employee.CellPhoneNumber}";
                        UpdateEmployeeList();
                        UpdateEmployeeDropDownList();
                    }
                    else
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
                            orderby b.LastName
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
                            orderby b.LastName
                            select b;
                foreach (var b in query)
                {
                    Visitors.Add(b);
                }
            }
        }

        private void UpdateVisitList()
        {
            Visits.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Visits
                            orderby b.Employee.LastName
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
            foreach (var emp in Employees)
            {
                string employee = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee);
            }
        }

        private void UpdateVisitorDropDownList()
        {
            VisitorsDropDownList.Items.Clear();
            foreach (var visitor in Visitors)
            {
                string visitor_info = $"{visitor.FirstName} {visitor.LastName}, {visitor.CompanyName}, {visitor.EmailAddress}, {visitor.PhoneNumber}";
                VisitorsDropDownList.Items.Add(visitor_info);
            }
        }

        private void UpdateVisitDropDownList()
        {
            VisitsDropDownList.Items.Clear();
            foreach (var visit in Visits)
            {
                string visit_info = $"{visit.Employee.FirstName} {visit.Employee.LastName} was visited by {visit.Visitor.FirstName} {visit.Visitor.LastName} from {visit.Visitor.CompanyName} on {visit.Time}";
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
            Employee selected_employee;

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
                Employee employee_to_remove = (Employee)db.Employees.Where(b => b.Id == selected_employee.Id).First();
                db.Employees.Remove(employee_to_remove);
                db.SaveChanges();
                removeEmployeeMessage.Text = $"Removed: {employee_to_remove.FirstName} {employee_to_remove.LastName}, {employee_to_remove.EmailAddress}, {employee_to_remove.CellPhoneNumber}";
            }
            UpdateEmployeeList();
            UpdateEmployeeDropDownList();
        }

        protected void RemoveVisitorButton_Click(object sender, EventArgs e)
        {
            Visitor selected_visitor;

            try
            {
                selected_visitor = Visitors[VisitorsDropDownList.SelectedIndex];
            }
            catch
            {
                removeVisitorMessage.Text = "No visitor selected";
                return;
            }

            using (var db = new VisitContext())
            {
                Visitor visitor_to_remove = (Visitor)db.Visitors.Where(b => b.Id == selected_visitor.Id).First();
                db.Visitors.Remove(visitor_to_remove);
                db.SaveChanges();
                removeVisitorMessage.Text = $"Removed: {visitor_to_remove.FirstName} {visitor_to_remove.LastName}, {visitor_to_remove.CompanyName}";
            }
            UpdateVisitorList();
            UpdateVisitorDropDownList();
        }

        protected void RemoveVisitButton_Click(object sender, EventArgs e)
        {
            //Visit selected_visit;

            //try
            //{
            //    selected_visit = Visits[VisitsDropDownList.SelectedIndex];
            //}
            //catch
            //{
            //    removeVisitMessage.Text = "No visit selected";
            //    return;
            //}

            //using (var db = new VisitContext())
            //{
            //    Visit visit_to_remove = (Visit)db.Visits.Where(b => b.Id == selected_visitor.Id).First();
            //    db.Visits.Remove(visitor_to_remove);
            //    db.SaveChanges();
            //    removeVisitorMessage.Text = $"Removed: {visitor_to_remove.FirstName} {visitor_to_remove.LastName}, {visitor_to_remove.CompanyName}";
            //}
            //UpdateVisitList();
            //UpdateVisitDropDownList();
        }
    }
}