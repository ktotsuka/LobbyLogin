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
                        FirstName = firstName.Text.Trim(),
                        LastName = lastName.Text.Trim(),
                        EmailAddress = emailAddress.Text.ToLower().Trim(),
                        CellPhoneNumber = cellPhoneNumber.Text.Trim()
                    };

                    try
                    {
                        db.Employees.Add(new_employee);
                        db.SaveChanges();
                    }
                    catch
                    {
                        addEmployeeErrorMessage.Text = "Can't add an employee.  Duplicate email address!";
                        return;
                    }

                    addEmployeeErrorMessage.Text = $"Added: {new_employee.FirstName} {new_employee.LastName}, {new_employee.EmailAddress}, {new_employee.CellPhoneNumber}";
                    UpdateEmployeeList();
                    UpdateDropDownList();
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
                string employee = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee);
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