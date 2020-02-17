using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace LobbyLogin
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AdminPasswordSubmitButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format($"password was {adminPassword.Text}"));

        }
    }
}