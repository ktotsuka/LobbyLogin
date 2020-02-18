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
        public const string correctPassword = "aa";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AdminPasswordSubmitButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format($"password was {adminPassword.Text}"));
            if (adminPassword.Text == correctPassword)
            {
                submitErrorMessage.Text = "";
                AdminPasswordTable.Visible = false;
                AddEmployeeTable.Visible = true;
                RemoveEmployeeTable.Visible = true;

                //adminPasswordLabel.Visible = false;
                //submitButton.Visible = false;
                //adminPassword.Visible = false;
                //firstNameLabel.Visible = true;
                //firstName.Visible = true;
            }
            else
            {
                submitErrorMessage.Text = "password is incorrect";
            }

        }

        protected void AddEmployeeButton_Click(object sender, EventArgs e)
        {

        }

        protected void RemoveEmployeeButton_Click(object sender, EventArgs e)
        {

        }

    }
}