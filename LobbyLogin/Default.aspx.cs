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
                using (var db = new VisitContext())
                {
                    Visitor new_visitor = new Visitor
                    {
                        FirstName = firstName.Text.Trim(),
                        LastName = lastName.Text.Trim(),
                        CompanyName = companyName.Text.Trim(),
                        EmailAddress = emailAddress.Text.Trim(),
                        PhoneNumber = phoneNumber.Text.Trim()
                    };

                    var duplicate_visitors = db.Visitors.Where
                        (b => (b.FirstName == new_visitor.FirstName) && (b.LastName == new_visitor.LastName) && (b.CompanyName == new_visitor.CompanyName));
                    if (duplicate_visitors.Count() != 0)
                    {
                        new_visitor = duplicate_visitors.First();
                    }
                    else
                    {
                        db.Visitors.Add(new_visitor);
                        db.SaveChanges();
                    }

                    Visit visit = new Visit
                    {
                        Visitor = new_visitor,
                        Employee = Employees[EmployeesDropDownList.SelectedIndex],
                        Time = DateTime.Now
                    };



                    db.Visits.Add(visit);
                    db.SaveChanges();
                }

                Response.Redirect("ThankYou.aspx");
            }


            //Debug.WriteLine("kenji: in submit click");



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