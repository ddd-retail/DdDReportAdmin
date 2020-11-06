using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            string user = Page.Request.Params["back"];

            if (!IsPostBack && user != null)
            {
                string userid = Session["users2ID"].ToString();
                string cred = InformationProvider.GetDdDReportInformation(Convert.ToInt32(userid));
                string[] vals = cred.Split(';');
                if (ReportLibrary.ConnectionHandler.debug)
                    Response.Redirect(string.Format("Login.aspx?username={0}&password={1}", vals[0], vals[1]));
                else
                    Response.Redirect(string.Format("http://www.dddreport.com/Login.aspx?username={0}&password={1}", vals[0], vals[1]));

                System.Web.HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
            }
            else
            {

                System.Web.HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("Login.aspx");

            }
        }
    }
}