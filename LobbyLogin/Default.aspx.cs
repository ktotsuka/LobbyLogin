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

        }

    }
}