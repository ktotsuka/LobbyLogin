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

namespace LobbyLogin
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("kenji: in submit click");


            using (var db = new VisitContext())
            {
                Visit visit = new Visit
                {
                    Guest = guestName.Text,
                    Employee = employees.Text,
                    Time = DateTime.Now
                };

                db.Visits.Add(visit);
                db.SaveChanges();

                var query = from b in db.Visits
                            orderby b.Employee
                            select b;
                Debug.WriteLine("All visits in the database:");
                foreach (var b in query)
                {
                    Debug.WriteLine(string.Format($"{b.Employee} was visited by {b.Guest} on {b.Time}"));



                }
            }
        }
    }
}