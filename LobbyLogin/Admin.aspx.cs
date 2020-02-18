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

        public LogIn()
        {
            Employees = new List<Employee>();
            UpdateEmployeeList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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

                UpdateDropDownList();

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
                        FirstName = firstName.Text,
                        LastName = lastName.Text,
                        EmailAddress = emailAddress.Text,
                        CellPhoneNumber = cellPhoneNumber.Text
                    };

                    var employee_query = db.Employees.Where(b => b.EmailAddress == new_employee.EmailAddress);
                    if (employee_query.Count() != 0)
                    {
                        addEmployeeErrorMessage.Text = "Can't add an employee.  Duplicate email address!";
                    }
                    else
                    {
                        db.Employees.Add(new_employee);
                        db.SaveChanges();

                        var query = from b in db.Employees
                                    orderby b.LastName
                                    select b;
                        Debug.WriteLine("All employees in the database:");
                        foreach (var b in query)
                        {
                            Debug.WriteLine(string.Format($"Employee: {b.FirstName} {b.LastName}, email = {b.EmailAddress}, cell phone = {b.CellPhoneNumber}"));
                            Employees.Add(b);
                        }
                        UpdateDropDownList();
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

        private void UpdateDropDownList()
        {
            EmployeesDropDownList.Items.Clear();
            foreach (var emp in Employees)
            {
                string employee_full_name = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee_full_name);
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
            else if ((firstName.Text.Length > MaxTextLength)
                     ||
                     (lastName.Text.Length > MaxTextLength)
                     ||
                     (emailAddress.Text.Length > MaxTextLength)
                     ||
                     (cellPhoneNumber.Text.Length > MaxTextLength))
            {
                submitErrorMessage.Text = "Text exceeded max number of characters";
                return false;
            }
            else
            {
                submitErrorMessage.Text = "";
                return true;
            }
        }

        protected void RemoveEmployeeButton_Click(object sender, EventArgs e)
        {
            Employee selected_employee = Employees[EmployeesDropDownList.SelectedIndex];

            using (var db = new VisitContext())
            {
                Employee employee_to_remove = (Employee)db.Employees.Where(b => b.EmailAddress == selected_employee.EmailAddress).First();
                db.Employees.Remove(employee_to_remove);
                db.SaveChanges();
                removeEmployeeMessage.Text = $"Removed: {employee_to_remove.FirstName} {employee_to_remove.LastName}, {employee_to_remove.EmailAddress}, {employee_to_remove.CellPhoneNumber}";
            }
            UpdateEmployeeList();
            UpdateDropDownList();
        }
    }
}