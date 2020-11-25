using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class MaintainUsersOld : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                this.HiddenField1.Value = Session["userType"].ToString();
                this.HiddenField2.Value = Session["save"].ToString();

            }
            catch (Exception ex)
            {
            }
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsCallback && !Page.IsPostBack)
            {
                this.cb_search.Items.Clear();
                string usertype = Session["userType"].ToString();
                string control = Session["control"].ToString();
                string concern = Session["concern"].ToString();


                Dictionary<string, string> validUsers = DdDReportUser.Userlist(usertype, control, concern);
                foreach (string k in validUsers.Keys)
                {
                    if (DdDReportUser.UserHigherRightsThanMe(k, Convert.ToInt32(usertype)))
                        continue;
                    this.cb_search.Items.Add(validUsers[k], k);
                }

                //fill cubename
                if (usertype == "0")
                {
                    List<string> cubes = InformationProvider.GetAllCubes();
                    cubes.Sort();
                    foreach (string s in cubes)
                        cb_searchCube.Items.Add(s);
                }

                //  this.txt_Email.ReadOnly = true;
            }
        }

        private string fetchValueFromCombobox(DevExpress.Web.ASPxComboBox box, string text)
        {

            for (int a = 0; a < box.Items.Count; a++)
                if (box.Items[a].Text == text)
                    return box.Items[a].Value.ToString();

            return "";
        }



        protected void ASPxCallbackPanel1_Callback1(object source, DevExpress.Web.CallbackEventArgsBase e)
        {


            //check validation.
            Msg.Value = "secret message";
            if (e.Parameter == "new")
            {
                this.txt_id.Text = InformationProvider.GetNextId().ToString();
                txt_Email.Text = "";
                txt_password.Text = "";
                txt_username.Text = "";
                cb_search.Items.Clear();
                Msg.Value = "new";
                FillComboboxes(null, false);
            }
            else if (e.Parameter == "delete")
            {
                if (cb_search.SelectedItem.Text == "")
                {
                    DdDReportUser user = DdDReportUser.GetUser(txt_username.Text);
                    DdDReportUser.DeleteUser(user);
                }
                else
                {
                    DdDReportUser user = DdDReportUser.GetUser(cb_search.SelectedItem.Text);
                    DdDReportUser.DeleteUser(user);
                }
            }
            else if (e.Parameter.StartsWith("save"))
            {
                using (var log = new StreamWriter(@"C:\temp\adminReportLog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("SHOULD SAVE A USER");
                }
                //validate
                if (!txt_id.IsValid || !txt_Email.IsValid || !txt_username.IsValid || !txt_password.IsValid ||
                    !cb_cubename.IsValid || !cb_currency.IsValid || !cb_language.IsValid || !cb_user.IsValid)
                {
                    FillComboboxes(null, false);
                    return;

                }
                using (var log = new StreamWriter(@"C:\temp\adminReportLog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("Getting the user");
                }
                DdDReportUser u2 = DdDReportUser.GetUser(txt_username.Text);
                if (u2.Id != null && u2.Id != txt_id.Text)
                {
                    txt_username.ValidationSettings.ErrorText = "Username is in use. Enter another";
                    txt_username.IsValid = false;
                    Msg.Value = "usernameTaken";
                    return;
                }
                using (var log = new StreamWriter(@"C:\temp\adminReportLog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("Got HIM");
                }
                //Tjek if exists else update.
                DdDReportUser user = new DdDReportUser();

                //dummy fill
                FillComboboxes(null, false);

                string[] args = e.Parameter.Split('#');
                user.Id = txt_id.Text;
                user.Language = fetchValueFromCombobox(cb_language, args[1]);
                user.Username = txt_username.Text;
                user.Password = txt_password.Text;
                user.Currency = fetchValueFromCombobox(cb_currency, args[5]);
                user.Cubename = args[2];
                user.Concern = args[3];
                user.UserType = fetchValueFromCombobox(cb_user, args[4]);
                user.Email = txt_Email.Text;



                DdDReportUser user2 = DdDReportUser.GetUser(Convert.ToInt32(user.Id));
                using (var log = new StreamWriter(@"C:\temp\adminReportLog.txt", true))
                {
                    log.AutoFlush = true;
                    log.WriteLine("Got HIM again");
                }
                if (user2.Id == null || user2.Id == "")
                    DdDReportUser.InsertUser(user);
                else
                    DdDReportUser.UpdateUser(user);

                this.img_status.ImageUrl = GetImageName(user.Id);

                ///add to serach box
                this.cb_search.Items.Add(user.Username);
                Session["Save"] = "save";
                this.HiddenField2.Value = "Save";
                Msg.Value = "save";


            }
            else if (e.Parameter.StartsWith("cubeFilter"))
            {
                //var cubename = cb_searchCube.Text;
                //this.cb_search.Items.Clear();
                //string usertype = Session["userType"].ToString();
                //string control = Session["control"].ToString();
                //string concern = Session["concern"].ToString();


                //Dictionary<string, string> validUsers = DdDReportUser.UserlistByCube(usertype, control, concern,cubename);
                //foreach (string k in validUsers.Keys)
                //{
                //    if (DdDReportUser.UserHigherRightsThanMe(k, Convert.ToInt32(usertype)))
                //        continue;
                //    this.cb_search.Items.Add(validUsers[k], k);
                //}
            }
            else
            {
                //this.lst_clients.EnableDefaultAppearance = true;
                int a = cb_search.SelectedIndex;
                DdDReportUser user = new DdDReportUser();
                if (e.Parameter == "reload")
                    user = DdDReportUser.GetUser(txt_username.Text);
                else
                    user = DdDReportUser.GetUser(cb_search.SelectedItem.Text);
                //this.txt_cubename.Text = user.Cubename;
                // this.txt_currency.Text = user.Currency;
                this.txt_id.Text = user.Id;
                this.txt_Email.Text = user.Email;
                //this.lbl_language.Text = user.Language;
                this.txt_password.Text = user.Password;
                this.txt_username.Text = user.Username;
                FillComboboxes(user, true);
                this.img_status.ImageUrl = GetImageName(user.Id);
                //foreach (Client c in user.ClientObjects)
                //    this.lst_clients.Items.Add(c.Name + " (" + c.ClientID + ") ");
                //if (e.Parameter == "trans")
                //{
                //    user.GetLastTransactionDate();
                //    this.lbl_lasttransaction0.Text = user.LastTransactionDate.ToString();

                //}

                // user.GetLog(this.ASPxListBox1);
                // this.ASPxCallbackPanel1.ClientVisible = true;
            }
        }

        public void FillComboboxes(DdDReportUser user, bool focus)
        {
            //Clear:
            cb_concern.Items.Clear();
            cb_cubename.Items.Clear();
            cb_currency.Items.Clear();
            cb_language.Items.Clear();
            cb_user.Items.Clear();

            switch (HttpContext.Current.Session["userType"].ToString())
            {
                case "0":
                    {
                        cb_user.Items.Add("Normal user", "-1");
                        cb_user.Items.Add("DdD Support", "0");
                        cb_user.Items.Add("Cube administrator", "1");
                        cb_user.Items.Add("Concern administrator", "2"); break;
                    }
                case "1":
                    {
                        cb_user.Items.Add("Normal user", "-1");
                        cb_user.Items.Add("Cube administrator", "1");
                        cb_user.Items.Add("Concern administrator", "2"); break;
                    }
                case "2":
                    {
                        cb_user.Items.Add("Normal user", "-1");
                        cb_user.Items.Add("Concern administrator", "2"); break;
                    }
            }
            //Select the right.
            // cb_userrights.DataBind();
            if (focus)
            {
                int a1 = 0;
                for (int b = 0; b < cb_user.Items.Count; b++)
                    if (((string)cb_user.Items[b].Value) == user.UserType)
                        a1 = b;
                cb_user.SelectedIndex = a1;
            }
            switch (HttpContext.Current.Session["userType"].ToString())
            {
                case "0":
                    {

                        List<string> cubes = InformationProvider.GetAllCubes();
                        cubes.Sort();
                        foreach (string s in cubes)
                            cb_cubename.Items.Add(s);

                        break;
                        //should chould choose
                    }
                case "1":
                    {
                        string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));

                        cb_cubename.Items.Add(cname);
                        cb_cubename.SelectedIndex = 0;
                        cb_cubename.ReadOnly = true;
                        break;
                    }
                case "2":
                    {
                        string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                        cb_cubename.Items.Add(cname);
                        cb_cubename.SelectedIndex = 0;
                        cb_cubename.ReadOnly = true;
                        break;
                    }
            }

            if (focus)
            {
                for (int a = 0; a < cb_cubename.Items.Count; a++)
                {
                    if (cb_cubename.Items[a].Text == user.Cubename)
                        cb_cubename.SelectedIndex = a;
                }
            }
            //Language:
            cb_language.Items.Add("Danish", "da-DK");
            cb_language.Items.Add("English", "en-GB");
            cb_language.Items.Add("Swedish", "sv-SE");
            cb_language.Items.Add("Norwegian", "nn-NO");
            cb_language.Items.Add("German", "de-DE");
            cb_language.Items.Add("French", "fr-FR");
            if (focus)
            {
                for (int a = 0; a < cb_language.Items.Count; a++)
                {
                    if (((string)cb_language.Items[a].Value) == user.Language)
                        cb_language.SelectedIndex = a;
                }
            }
            //currency
            cb_currency.Items.Add("DKK", "dk");
            cb_currency.Items.Add("EUR", "eu");
            cb_currency.Items.Add("SKR", "se");
            cb_currency.Items.Add("NRK", "no");
            cb_currency.Items.Add("GBP", "uk");
            cb_currency.Items.Add("CHF", "ch");
            cb_currency.Items.Add("USD", "us");
            cb_currency.Items.Add("MUR", "mu");
            cb_currency.Items.Add("CAD", "ca");
            if (focus)
            {
                for (int a = 0; a < cb_currency.Items.Count; a++)
                {
                    if (((string)cb_currency.Items[a].Value) == user.Currency)
                        cb_currency.SelectedIndex = a;
                }
            }
            //concern:

            if (user != null)
            {
                cb_concern.Items.Add(user.Concern);
                cb_concern.SelectedIndex = 0;
            }
            else
            {
                string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                List<string> concerns = DdDReportUser.GetConcerns(Session["userType"].ToString(), cname);
                foreach (string s in concerns)
                    cb_concern.Items.Add(s);

                if (cb_concern.Items.Count > 0)
                    cb_concern.SelectedIndex = 0;
            }
        }

        protected string GetImageName(object dataValue)
        {
            string val = string.Empty;
            try
            {
                val = (string)dataValue;

                DdDReportUser user = DdDReportUser.GetUser(Convert.ToInt32(val));
                if (user.ClientsConnected)
                    return "~/Images/ok.png";
                else
                    return "~/Images/cancel.png";

            }
            catch (Exception ex)
            {
            }
            return "";

        }
        protected void txt_text_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            //Check for null values
            if (e.Value == null || e.Value == "")
            {
                e.ErrorText = "You must provide a value";
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }
        protected void cb_combo_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            if (e.Value == null || e.Value == "")
            {
                e.ErrorText = "You must select a value";
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }
        protected void cb_concern_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            cb_concern.Items.Clear();
            string cubename = e.Parameter;
            foreach (string s in DdDReportUser.GetConcerns(Session["userType"].ToString(), cubename))
                cb_concern.Items.Add(s);

        }
        protected void ASPxCallbackPanel2_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            var cubename = cb_searchCube.Text;
            this.cb_search.Items.Clear();
            string usertype = Session["userType"].ToString();
            string control = Session["control"].ToString();
            string concern = Session["concern"].ToString();


            Dictionary<string, string> validUsers = DdDReportUser.UserlistByCube(usertype, control, concern, cubename);
            foreach (string k in validUsers.Keys)
            {
                if (DdDReportUser.UserHigherRightsThanMe(k, Convert.ToInt32(usertype)))
                    continue;
                this.cb_search.Items.Add(validUsers[k], k);
            }
        }
    }
}