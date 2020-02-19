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
namespace LobbyLogin
{
    public partial class _Default : Page
    {
        public const int MaxTextLength = 50;
        public List<Employee> Employees { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<Employee>();
            UpdateEmployeeList();
            UpdateDropDownList();
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

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (VerifyInputs())
            {
                Visitor new_visitor = new Visitor
                {
                    FirstName = firstName.Text.Trim(),
                    LastName = lastName.Text.Trim(),
                    CompanyName = companyName.Text.Trim(),
                    EmailAddress = emailAddress.Text.Trim(),
                    PhoneNumber = phoneNumber.Text.Trim(),
                    Id = firstName.Text.Trim() + lastName.Text.Trim() + companyName.Text.Trim()
                };
                TryAddVisitorToDatabase(new_visitor);

                Visit new_visit = new Visit
                {
                    Visitor = new_visitor,
                    Employee = Employees[EmployeesDropDownList.SelectedIndex],
                    Time = DateTime.Now
                };
                AddVisitToDatabase(new_visit);             

                Response.Redirect("ThankYou.aspx");
            }
        }

        private void TryAddVisitorToDatabase(Visitor new_visitor)
        {
            using (var db = new VisitContext())
            {
                var duplicate_visitors = db.Visitors.Where(b => b.Id == new_visitor.Id);
                if (duplicate_visitors.Count() == 0)
                {
                    db.Visitors.Add(new_visitor);
                    db.SaveChanges();
                }
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
                submitErrorMessage.Text = "All required fields need to be filled";
                return false;
            }
            if (!LogIn.IsValidEmail(emailAddress.Text))
            {
                submitErrorMessage.Text = "Invalid email address";
                return false;
            }
            else
            {
                submitErrorMessage.Text = "";
                return true;
            }
        }
    }
}