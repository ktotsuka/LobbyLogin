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

namespace LobbyLogin
{
    public partial class _Default : Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void YesButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("WithAppointment.aspx");
        }

        protected void NoButton_Click(object sender, EventArgs e)
        {
            using (var dc = new VisitContext())
            {
                foreach (string employee_emal in Mail.GeneralEmployeeEmails)
                {
                    var employee_w = dc.Employees.Where(a => a.Employee.EmailAddress == employee_emal).FirstOrDefault();
                    if (employee_w != null)
                    {
                        Employee employee = employee_w.Employee;
                        string numeric_phone_number = new String(employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                        List<string> addresses = Mail.GetPhoneEmailAddresses(numeric_phone_number);
                        addresses.Add(employee.EmailAddress);

                        string message = $"A person with no appointment has arrived";
                        Mail.SendEmail(addresses, message);
                    }
                }
            }
            Response.Redirect("ThankYou.aspx");
        }
    }
}