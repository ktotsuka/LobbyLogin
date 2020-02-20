using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Ch18CardLibStandard;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Threading;

namespace LobbyLogin
{
    public partial class _Default : Page
    {
        public const int MaxTextLength = 50;
        public List<EmployeeWrapper> Employees { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<EmployeeWrapper>();
            UpdateEmployeeList();
            UpdateEmployeeDropDownList();
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

        private void UpdateEmployeeDropDownList()
        {
            int selected = EmployeesDropDownList.SelectedIndex;
            EmployeesDropDownList.Items.Clear();
            foreach (var employee in Employees)
            {
                Employee emp = employee.Employee;
                string employee_info = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee_info);
            }
            
            if (selected >= 0)
            {
                EmployeesDropDownList.SelectedIndex = selected;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (VerifyInputs())
            {
                Thread.Sleep(5000);

                //Visitor visitor = new Visitor
                //{
                //    FirstName = firstName.Text.Trim(),
                //    LastName = lastName.Text.Trim(),
                //    CompanyName = companyName.Text.Trim(),
                //    EmailAddress = emailAddress.Text.Trim(),
                //    PhoneNumber = phoneNumber.Text.Trim(),
                //};

                //Employee employee = Employees[EmployeesDropDownList.SelectedIndex].Employee;
                //DateTime time = DateTime.Now;

                //Visit new_visit = new Visit
                //{
                //    Visitor = visitor,
                //    Employee = employee,
                //    Time = time,
                //    Id = $"{visitor}" + $"{employee}" + $"{time}"
                //};
                //AddVisitToDatabase(new_visit);

                //string numeric_phone_number = new String(employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                //string message = $"{visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} has arrived for you";
                //List<string> addresses = new List<string>
                //{
                //    employee.EmailAddress
                //};
                //Mail.SendEmail(addresses, message);
                //Mail.SendText(numeric_phone_number, message);

                //Response.Redirect("ThankYou.aspx");
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
            else
            {
                submitMessage.ForeColor = System.Drawing.Color.Green;
                submitMessage.Text = "Processing, please wait...";
                return true;
            }
        }
    }
}