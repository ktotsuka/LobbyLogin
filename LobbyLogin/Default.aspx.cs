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
            bool valid;

            valid = VerifyInputs();
            if (valid)
            {
                Response.Redirect("ThankYou.aspx");

                //Server.Transfer("ThankYou.aspx");
            }


            //Debug.WriteLine("kenji: in submit click");


            //using (var db = new VisitContext())
            //{
            //    Visit visit = new Visit
            //    {
            //        Guest = guestName.Text,
            //        Employee = employees.Text,
            //        Time = DateTime.Now
            //    };

            //    db.Visits.Add(visit);
            //    db.SaveChanges();

            //    var query = from b in db.Visits
            //                orderby b.Employee
            //                select b;
            //    Debug.WriteLine("All visits in the database:");
            //    foreach (var b in query)
            //    {
            //        Debug.WriteLine(string.Format($"{b.Employee} was visited by {b.Guest} on {b.Time}"));



            //    }
            //}
        }

        private bool VerifyInputs()
        {
            //Style style = new Style();

            if ((firstName.Text == "")
                ||
                (lastName.Text == "")
                ||
                (companyName.Text == ""))
            {
                //style.ForeColor = System.Drawing.Color.Red;
                //style.BackColor = System.Drawing.Color.Red;
                //submitErrorMessage.ApplyStyle(style);
                submitErrorMessage.Text = "All required fields need to be filled";
                //submitErrorMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if ((firstName.Text.Length > MaxTextLength)
                     ||
                     (lastName.Text.Length > MaxTextLength)
                     ||
                     (companyName.Text.Length > MaxTextLength))
            {
                //style.ForeColor = System.Drawing.Color.Red;
                //submitErrorMessage.ApplyStyle(style);
                submitErrorMessage.Text = "Text exceeded max number of characters";
                //submitErrorMessage.ForeColor = System.Drawing.Color.Red;

                return false;
            }
            else
            {
                //style.ForeColor = System.Drawing.Color.Black;
                //submitErrorMessage.ApplyStyle(style);
                submitErrorMessage.Text = "";
                return true;
            }
        }
    }
}