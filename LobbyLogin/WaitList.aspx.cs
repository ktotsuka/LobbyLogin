using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VisitDataBase;
using System.Threading;
using static VisitDataBase.DataAccess;

namespace LobbyLogin
{
    public partial class WaitList : System.Web.UI.Page
    {
        public List<Visit> WaitingVisits { get; set; }
        private string WaitingVisitInfo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            WaitingVisitInfo = WaitingVisitDropDownList.SelectedValue;
            WaitingVisits = new List<Visit>();
            UpdateWaitingVisitList();
            UpdateWaitingVisitDropDownList();
        }

        private void UpdateWaitingVisitList()
        {
            WaitingVisits.Clear();

            List<Visit> visits;

            visits = GetVisitFromFile(WaitListFileLocation);

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
                WaitingVisitDropDownList.Items.Add("None");
            }

            foreach (var visit in WaitingVisits)
            {
                string visitor_info = GetWaitingVisitInfo(visit);
                WaitingVisitDropDownList.Items.Add(visitor_info);
            }

            if (selected >= 1 && (WaitingVisitDropDownList.Items.Count > selected))
            {
                WaitingVisitDropDownList.SelectedIndex = selected;
            }
        }

        private string GetWaitingVisitInfo(Visit visit)
        {
            return $"{visit.Visitor.FirstName} {visit.Visitor.LastName} from {visit.Visitor.CompanyName} on {visit.Time.ToString()}";
        }

        protected void RemoveWaitingVisitButton_Click(object sender, EventArgs e)
        {
            WaitingVisits.RemoveAll(item => GetWaitingVisitInfo(item) == WaitingVisitInfo);

            using (var mutex = new Mutex(false, WaitListMutexName))
            {
                mutex.WaitOne();

                UpdateWaitListFile(WaitingVisits);

                mutex.ReleaseMutex();
            }

            UpdateWaitingVisitList();
            UpdateWaitingVisitDropDownList();
        }
    }
}
