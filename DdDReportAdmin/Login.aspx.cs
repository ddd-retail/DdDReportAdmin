using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string user = Page.Request.Params["username"];
            string pass = Page.Request.Params["password"];

            if (!IsPostBack && user != null && pass != null)
            {

                if (InformationProvider.Authenticate(user, pass))
                {
                    Response.Redirect("Default.aspx");
                    //   e.Authenticated = true;
                }
                //else
                //    e.Authenticated = false;
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {


            if (InformationProvider.Authenticate(LoginControl.UserName, LoginControl.Password))
            {
                this.LoginControl.DestinationPageUrl = "Default.aspx";
                e.Authenticated = true;
            }
            else
                e.Authenticated = false;
        }
    }
}