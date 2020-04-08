using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitDataBase;
using SignInMail;
using static VisitDataBase.DataAccess;
using static VisitDataBase.GeneralEmployee;
using static SignInMail.Mail;

namespace LobbyLogin
{
    public partial class DeliveryMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void NotifyButton_Click(object sender, EventArgs e)
        {
            List<string> addresses;
            string message;

            foreach (Employee delivery_employee in VisitDataBase.GeneralEmployee.DeliveryEmployees)
            {
                string numeric_phone_number = new String(delivery_employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                addresses = GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(delivery_employee.EmailAddress);

                message = $"delivery notice at the back";
                SendEmail(addresses, message);
            }
            Response.Redirect("DeliveryThankYou.aspx");
        }
    }
}