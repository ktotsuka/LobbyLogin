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
using SignInMail;
using static VisitDataBase.DataAccess;

namespace LobbyLogin
{
    public partial class _Default : Page
    {
        public List<Visit> WaitingVisits { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            WaitingVisits = new List<Visit>();
            UpdateWaitingVisitList();
            UpdateWaitingVisitDropDownList();
        }

        private void UpdateWaitingVisitList()
        {
            WaitingVisits.Clear();

            List<Visit> visits;

            while (true)
            {
                try
                {
                    visits = GetVisitFromFile(WaitListFileLocation);
                    break;
                }
                catch
                {
                    Thread.Sleep(FileAccessRetryWait);
                }
            }

            foreach (var i in visits)
            {
                WaitingVisits.Add(i);
            }
        }

        private void UpdateWaitingVisitDropDownList()
        {
            int selected = WaitingVisitDropDownList.SelectedIndex;

            WaitingVisitDropDownList.Items.Clear();

            if (WaitingVisits.Count == 0)
            {
                WaitingVisitDropDownList.Items.Add("No visitor is waiting");
            }

            foreach (var visit in WaitingVisits)
            {
                string visitor_info = $"{visit.Visitor.FirstName} {visit.Visitor.LastName} from {visit.Visitor.CompanyName} "
                                     + $"visiting {visit.Employee.FirstName} {visit.Employee.LastName} on {visit.Time}";
                WaitingVisitDropDownList.Items.Add(visitor_info);
            }

            if (selected >= 1 && (WaitingVisitDropDownList.Items.Count > selected))
            {
                WaitingVisitDropDownList.SelectedIndex = selected;
            }
        }

        protected void YesButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("WithAppointment.aspx");
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

        protected void RemoveWaitingVisitButton_Click(object sender, EventArgs e)
        {
            try
            {
                WaitingVisits.RemoveAt(WaitingVisitDropDownList.SelectedIndex);
            }
            catch
            {
                return;
            }

            using (var mutex = new Mutex(false, WaitListMutexName))
            {
                UpdateWaitListFile(WaitingVisits);

                mutex.ReleaseMutex();
            }
            
            UpdateWaitingVisitList();
            UpdateWaitingVisitDropDownList();
        }
    }
}