using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LobbyLogin
{
    public partial class ConfidentialityAgreement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AgreeButton_Click(object sender, EventArgs e)
        {

        }

        protected void DisagreeButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("UnableToSignIn.aspx");
        }
    }
}