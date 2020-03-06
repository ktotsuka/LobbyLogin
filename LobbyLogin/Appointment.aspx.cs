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
using System.Text;
using System.IO;
using VisitDataBase;
using SignInMail;

namespace LobbyLogin
{
    public partial class Appointment : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }        

        protected void YesButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        protected void NoButton_Click(object sender, EventArgs e)
        {
            foreach (Employee employee in VisitDataBase.GeneralEmployee.GeneralEmployees)
            {
                string numeric_phone_number = new String(employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                List<string> addresses = Mail.GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(employee.EmailAddress);

                string message = $"A person with no appointment has arrived";
                Mail.SendEmail(addresses, message);
            }
            Response.Redirect("ThankYou.aspx");
        }        
    }
}