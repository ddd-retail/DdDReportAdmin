using DdDRetail.Common.Logger.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class MaintainUsers : System.Web.UI.Page
    {
        private static NLogger logger = new NLogger(nameof(MaintainUsers));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.HiddenField1.Value = Session["userType"]?.ToString();
                this.HiddenField2.Value = Session["save"]?.ToString();

            }
            catch (Exception ex)
            {
                logger.Warn($"Error in PageLoad: {ex.Message}");
            }
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsCallback && !Page.IsPostBack)
            {
                this.cbSearch.Items.Clear();
                string usertype = Session["userType"]?.ToString();
                string control = Session["control"]?.ToString();
                string concern = Session["concern"]?.ToString();


                Dictionary<string, string> validUsers = DdDReportUser.Userlist(usertype, control, concern);
                foreach (string k in validUsers.Keys)
                {
                    if (DdDReportUser.UserHigherRightsThanMe(k, Convert.ToInt32(usertype)))
                        continue;
                    this.cbSearch.Items.Add(validUsers[k], k);
                }

                //fill cubename
                if (usertype == "0")
                {
                    List<string> cubes = InformationProvider.GetAllCubes();
                    cubes.Sort();
                    foreach (string s in cubes)
                        cbSearchCube.Items.Add(s);
                }
            }
        }

        protected void cpUser_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                //check validation.
                Msg.Value = "secret message";
                lblActionResult.Text = string.Empty;
                lblActionResult.ForeColor = System.Drawing.Color.Transparent;
                if (e.Parameter == "new")
                {
                    this.txtUserId.Text = InformationProvider.GetNextId().ToString();
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Text = "";
                    cbSearch.Items.Clear();
                    Msg.Value = "new";
                    FillComboboxes(null, false);
                }
                else if (e.Parameter == "delete")
                {
                    if (cbSearch.SelectedItem.Text == "")
                    {
                        DdDReportUser user = DdDReportUser.GetUser(txtUsername.Text);
                        DdDReportUser.DeleteUser(user);
                    }
                    else
                    {
                        DdDReportUser user = DdDReportUser.GetUser(cbSearch.SelectedItem.Text);
                        DdDReportUser.DeleteUser(user);
                    }
                    lblActionResult.Text = "Delete sucess.";
                    lblActionResult.ForeColor = System.Drawing.Color.Green;
                }
                else if (e.Parameter.StartsWith("save"))
                {
                    System.IO.File.AppendAllLines(@"C:\temp\adminReportLog.txt", new string[] { @"SHOULD SAVE A USER" });
                    //validate
                    if (!txtUserId.IsValid || !txtEmail.IsValid || !txtUsername.IsValid || !txtPassword.IsValid ||
                        !cbCubename.IsValid || !cbCurrency.IsValid || !cbLanguage.IsValid || !cbUser.IsValid)
                    {
                        FillComboboxes(null, false);
                        lblActionResult.Text = "Could not save. Please correct errors";
                        lblActionResult.ForeColor = System.Drawing.Color.Red;
                        return;

                    }
                    System.IO.File.AppendAllLines(@"C:\temp\adminReportLog.txt", new string[] { @"Getting the User" });
                    if (DdDReportUser.IsDuplicateUsername(txtUsername.Text))
                    {
                        txtUsername.ValidationSettings.ErrorText = "Username is in use. Enter another";
                        txtUsername.IsValid = false;
                        Msg.Value = "usernameTaken";
                        lblActionResult.Text = "Could not save. Please correct errors";
                        lblActionResult.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    System.IO.File.AppendAllLines(@"C:\temp\adminReportLog.txt", new string[] { @"Got HIM" });
                    //Tjek if exists else update.
                    DdDReportUser user = new DdDReportUser();

                    //dummy fill
                    FillComboboxes(null, false);

                    string[] args = e.Parameter.Split('#');
                    user.Id = txtUserId.Text;
                    user.Language = FetchValueFromCombobox(cbLanguage, args[1]);
                    user.Username = txtUsername.Text;
                    user.Password = txtPassword.Text;
                    user.Currency = FetchValueFromCombobox(cbCurrency, args[5]);
                    user.Cubename = args[2];
                    user.Concern = args[3];
                    user.UserType = FetchValueFromCombobox(cbUser, args[4]);
                    user.Email = txtEmail.Text;

                    DdDReportUser user2 = DdDReportUser.GetUser(Convert.ToInt32(user.Id));
                    System.IO.File.AppendAllLines(@"C:\temp\adminReportLog.txt", new string[] { @"Got HIM again" });
                    if (user2.Id == null || user2.Id == "")
                        DdDReportUser.InsertUser(user);
                    else
                        DdDReportUser.UpdateUser(user);

                    this.img_status.ImageUrl = GetImageName(user.Id);

                    ///add to serach box
                    this.cbSearch.Items.Add(user.Username);
                    Session["Save"] = "save";
                    this.HiddenField2.Value = "Save";
                    Msg.Value = "save";
                    lblActionResult.Text = "Saved sucess.";
                    lblActionResult.ForeColor = System.Drawing.Color.Green;

                }
                else
                {
                    int a = cbSearch.SelectedIndex;
                    DdDReportUser user = new DdDReportUser();
                    if (e.Parameter == "reload")
                        user = DdDReportUser.GetUser(txtUsername.Text);
                    else
                        user = DdDReportUser.GetUser(cbSearch.SelectedItem.Text);
                    this.txtUserId.Text = user.Id;
                    this.txtEmail.Text = user.Email;
                    this.txtPassword.Text = user.Password;
                    this.txtUsername.Text = user.Username;
                    FillComboboxes(user, true);
                    this.img_status.ImageUrl = GetImageName(user.Id);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(cpUser_Callback)}: {ex.Message}");
                throw;
            }
        }

        protected void cpControlPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                var cubename = cbSearch.Text;
                this.cbSearch.Items.Clear();
                string usertype = Session["userType"].ToString();
                string control = Session["control"].ToString();
                string concern = Session["concern"].ToString();


                Dictionary<string, string> validUsers = DdDReportUser.UserlistByCube(usertype, control, concern, cubename);
                foreach (string k in validUsers.Keys)
                {
                    if (DdDReportUser.UserHigherRightsThanMe(k, Convert.ToInt32(usertype)))
                        continue;
                    this.cbSearch.Items.Add(validUsers[k], k);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(cpControlPanel_Callback)}: {ex.Message}");
                throw;
            }
        }

        public void FillComboboxes(DdDReportUser user, bool focus)
        {
            try
            {
                //Clear:
                cbConcern.Items.Clear();
                cbCubename.Items.Clear();
                cbCurrency.Items.Clear();
                cbLanguage.Items.Clear();
                cbUser.Items.Clear();

                switch (HttpContext.Current.Session["userType"].ToString())
                {
                    case "0":
                        {
                            cbUser.Items.Add("Normal user", "-1");
                            cbUser.Items.Add("DdD Support", "0");
                            cbUser.Items.Add("Cube administrator", "1");
                            cbUser.Items.Add("Concern administrator", "2"); break;
                        }
                    case "1":
                        {
                            cbUser.Items.Add("Normal user", "-1");
                            cbUser.Items.Add("Cube administrator", "1");
                            cbUser.Items.Add("Concern administrator", "2"); break;
                        }
                    case "2":
                        {
                            cbUser.Items.Add("Normal user", "-1");
                            cbUser.Items.Add("Concern administrator", "2"); break;
                        }
                }

                if (focus)
                {
                    int a1 = 0;
                    for (int b = 0; b < cbUser.Items.Count; b++)
                        if (((string)cbUser.Items[b].Value) == user.UserType)
                            a1 = b;
                    cbUser.SelectedIndex = a1;
                }
                switch (HttpContext.Current.Session["userType"].ToString())
                {
                    case "0":
                        {

                            List<string> cubes = InformationProvider.GetAllCubes();
                            cubes.Sort();
                            foreach (string s in cubes)
                                cbCubename.Items.Add(s);

                            break;
                            //should chould choose
                        }
                    case "1":
                        {
                            string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));

                            cbCubename.Items.Add(cname);
                            cbCubename.SelectedIndex = 0;
                            cbCubename.ReadOnly = true;
                            break;
                        }
                    case "2":
                        {
                            string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                            cbCubename.Items.Add(cname);
                            cbCubename.SelectedIndex = 0;
                            cbCubename.ReadOnly = true;
                            break;
                        }
                }

                if (focus)
                {
                    for (int a = 0; a < cbCubename.Items.Count; a++)
                    {
                        if (cbCubename.Items[a].Text == user.Cubename)
                            cbCubename.SelectedIndex = a;
                    }
                }
                //Language:
                cbLanguage.Items.Add("Danish", "da-DK");
                cbLanguage.Items.Add("English", "en-GB");
                cbLanguage.Items.Add("Swedish", "sv-SE");
                cbLanguage.Items.Add("Norwegian", "nn-NO");
                cbLanguage.Items.Add("German", "de-DE");
                cbLanguage.Items.Add("French", "fr-FR");
                if (focus)
                {
                    for (int a = 0; a < cbLanguage.Items.Count; a++)
                    {
                        if (((string)cbLanguage.Items[a].Value) == user.Language)
                            cbLanguage.SelectedIndex = a;
                    }
                }
                //currency
                cbCurrency.Items.Add("DKK", "dk");
                cbCurrency.Items.Add("EUR", "eu");
                cbCurrency.Items.Add("SKR", "se");
                cbCurrency.Items.Add("NRK", "no");
                cbCurrency.Items.Add("GBP", "uk");
                cbCurrency.Items.Add("CHF", "ch");
                cbCurrency.Items.Add("USD", "us");
                cbCurrency.Items.Add("MUR", "mu");
                cbCurrency.Items.Add("CAD", "ca");
                if (focus)
                {
                    for (int a = 0; a < cbCurrency.Items.Count; a++)
                    {
                        if (((string)cbCurrency.Items[a].Value) == user.Currency)
                            cbCurrency.SelectedIndex = a;
                    }
                }
                //concern:

                if (user != null)
                {
                    cbConcern.Items.Add(user.Concern);
                    cbConcern.SelectedIndex = 0;
                }
                else
                {
                    string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                    List<string> concerns = DdDReportUser.GetConcerns(Session["userType"].ToString(), cname);
                    foreach (string s in concerns)
                        cbConcern.Items.Add(s);

                    if (cbConcern.Items.Count > 0)
                        cbConcern.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(FillComboboxes)}: {ex.Message}");
                throw;
            }
        }

        protected void txtText_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            //Check for null values
            if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
            {
                e.ErrorText = "You must provide a value";
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }

        protected void cbCombo_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
            {
                e.ErrorText = "You must select a value";
                e.IsValid = false;
            }
            else
                e.IsValid = true;
        }

        protected void cbConcern_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                cbConcern.Items.Clear();
                string cubename = e.Parameter;
                foreach (string s in DdDReportUser.GetConcerns(Session["userType"].ToString(), cubename))
                    cbConcern.Items.Add(s);
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {nameof(cbConcern_Callback)}: {ex.Message}");
                throw;
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
                logger.Error($"Error in {nameof(GetImageName)}({dataValue}): {ex.Message}");
            }
            return "";

        }

        private string FetchValueFromCombobox(DevExpress.Web.ASPxComboBox box, string text)
        {

            for (int a = 0; a < box.Items.Count; a++)
                if (box.Items[a].Text == text)
                    return box.Items[a].Value.ToString();

            return "";
        }
    }
}