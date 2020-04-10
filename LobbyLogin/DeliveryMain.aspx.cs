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
            if (InputValid())
            {
                string message = CreateNoticeMessage();
                SendDeliveryNotice(message);
            }
            else
            {
                notifyMessage.Text = "Please make selections";
            }
        }

        private bool InputValid()
        {
            if (!PickUpRadioButton.Checked && !DropOffRadioButton.Checked)
            {
                return false;
            }
            if (!ForkLiftRequiredRadioButton.Checked && !ForkLiftNotRequiredRadioButton.Checked)
            {
                return false;
            }

            return true;
        }

        private string CreateNoticeMessage()
        {
            string message = "Delivery notic: ";

            if (PickUpRadioButton.Checked)
            {
                message += "Pick-up, ";
            }
            else
            {
                message += "Drop-off, ";
            }

            if (ForkLiftRequiredRadioButton.Checked)
            {
                message += "Forklift required";
            }
            else
            {
                message += "Forklift not required";
            }

            return message;
        }

        private void SendDeliveryNotice(string message)
        {
            List<string> addresses;
            List<EmployeeWrapper> employees = ReadEmployeesFromFile(DeliveryNotificationListFileLocation);

            foreach (EmployeeWrapper e in employees)
            {
                string numeric_phone_number = new String(e.Employee.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                addresses = GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(e.Employee.EmailAddress);

                SendEmail(addresses, message);
            }
            Response.Redirect("DeliveryThankYou.aspx");
        }
    }
}