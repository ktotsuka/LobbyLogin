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
            Debug.Print("kenji: in submit click");


            //using (var db = new VisitContext())
            //{
                Visit visit = new Visit();
                visit.Visitor = guestName.Text;
                visit.Employee = employees.Text;
                visit.Time = DateTime.Now;



                //{
                //    Title = "Beginning C# 7",
                //    Author = "Perkins, Reid, and Hammer"
                //};
                //db.Books.Add(book1);
                //Book book2 = new Book
                //{
                //    Title = "Beginning XML",
                //    Author = "Fawcett, Quin, and Ayers"
                //};
                //db.Books.Add(book2);
                //db.SaveChanges();
                //var query = from b in db.Books
                //            orderby b.Title
                //            select b;
                //WriteLine("All books in the database:");
                //foreach (var b in query)
                //{
                //    WriteLine($"{b.Title} by {b.Author}, code={b.Code}");
                //}
                //WriteLine("Press a key to exit...");
                //ReadKey();
                // }
        }
    }
}