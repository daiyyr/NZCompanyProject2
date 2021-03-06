﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
namespace telco
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                WelcomeBackMessage.Text = "Welcome back!" + HttpContext.Current.User.Identity.Name;
                AuthenticatedMessagePanel.Visible = true;
                AnonymousMessagePanel.Visible = false;
            }
            else
            {
                AuthenticatedMessagePanel.Visible = false;
                AnonymousMessagePanel.Visible = true;
            }
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(Request.UrlReferrer.ToString());
        }
    }
}