using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class ResetReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userID"] == null)
                    Response.Redirect("Login.aspx");

                if (!IsPostBack)
                {
                    List<DdDReportUser> users = DdDReportUser.GetUsers(Session["userType"].ToString(), Session["control"].ToString(), Session["concern"].ToString());
                    foreach (DdDReportUser user in users)
                    {
                        this.ASPxComboBox1.Items.Add(user.Username, user.Id);
                    }

                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines("errors.log", new[] { $@"Error in ResetReports.Page_Load(): {ex.Message}", ex.StackTrace });
                throw;
            }
        }

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                this.ASPxListBox1.Items.Clear();
                DdDReportUser selectedUser = DdDReportUser.GetUser(this.ASPxComboBox1.Text);
                foreach (string s in InformationProvider.getReports(selectedUser.Id))
                {
                    this.ASPxListBox1.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines("errors.log", new[] { $@"Error in ResetReports.ASPxCallbackPanel1_Callback(): {ex.Message}", ex.StackTrace });
                throw;
            }

        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            DdDReportUser selectedUser = DdDReportUser.GetUser(this.ASPxComboBox1.Text);

            if (InformationProvider.DeleteUserReports(selectedUser.Id))
                Response.Write("<h2>Reports has been deleted</h2>");
            else
                Response.Write("<h2>Failed to delete reports. Please contact support</h2>");
        }
    }
}