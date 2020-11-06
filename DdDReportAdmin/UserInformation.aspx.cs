using System;
using System.Web.UI;

namespace DdDReportAdmin
{
    public partial class UserInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsCallback && !Page.IsPostBack)
            {
                this.ASPxComboBox2.Items.Clear();
                string usertype = Session["userType"].ToString();
                string control = Session["control"].ToString();
                string concern = Session["concern"].ToString();

                foreach (string users in DdDReportUser.Usernames(usertype, control, concern))
                    this.ASPxComboBox2.Items.Add(users);
            }
        }
        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            this.lst_clients.EnableDefaultAppearance = true;
            DdDReportUser user = DdDReportUser.GetUser(ASPxComboBox2.SelectedItem.Text);

            this.lbl_cubename.Text = user.Cubename;
            this.lbl_currency.Text = user.Currency;
            this.lbl_email.Text = user.Email;
            this.lbl_language.Text = user.Language;
            this.lbl_password.Text = user.Password;
            this.lbl_username.Text = user.Username;
            foreach (Client c in user.ClientObjects)
                this.lst_clients.Items.Add(c.Name + " (" + c.ClientID + ") ");
            if (e.Parameter == "trans")
            {
                user.GetLastTransactionDate();
                this.lbl_lasttransaction0.Text = user.LastTransactionDate.ToString();

            }

            user.GetLog(this.ASPxListBox1);
            this.ASPxCallbackPanel1.ClientVisible = true;
        }
        protected void btn_LoginAsUser_Click(object sender, EventArgs e)
        {
            if (ASPxComboBox2.SelectedItem.Text != null)
            {
                DdDReportUser user = DdDReportUser.GetUser(ASPxComboBox2.SelectedItem.Text);

                Response.Redirect(string.Format("http://www.dddreport.com/Login.aspx?username={0}&password={1}", user.Username, user.Password));

                System.Web.HttpContext.Current.Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();

            }
        }
    }
}