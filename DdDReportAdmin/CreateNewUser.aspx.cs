using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class CreateNewUser : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Literal clientidLitera2l;
        object newKey = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            List<Currency> availableCurs;
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");


            availableCurs = Currency.GetValues();
            this.Grid_Clients.Columns[1].Visible = false;
            this.Grid_Clients.Columns[7].Visible = false;

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

        protected string GetUserId(object dataValue)
        {
            return dataValue.ToString();
        }

        /// <summary>
        /// Bind Currency, Language & Usertype
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ASPxGridView1_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Currency" || e.Column.FieldName == "Language")
            {
                ASPxComboBox combo = (ASPxComboBox)e.Editor;
                combo.DataBind();
            }

            if (e.Column.FieldName == "UserType")
            {
                //filter items.
                ASPxComboBox combo = (ASPxComboBox)e.Editor;
                combo.Items.Clear();
                combo.Items.Add("", "");

                switch (HttpContext.Current.Session["userType"].ToString())
                {
                    case "0":
                        {
                            combo.Items.Add("Normal user", "-1");
                            combo.Items.Add("DdD Support", "0");
                            combo.Items.Add("Cube administrator", "1");
                            combo.Items.Add("Concern administrator", "2"); break;
                        }
                    case "1":
                        {
                            combo.Items.Add("Normal user", "-1");
                            combo.Items.Add("Cube administrator", "1");
                            combo.Items.Add("Concern administrator", "2"); break;
                        }
                    case "2":
                        {
                            combo.Items.Add("Normal user", "-1");
                            combo.Items.Add("Concern administrator", "2"); break;
                        }
                }

                combo.DataBind();
            }
            if (e.Column.FieldName == "Cubename")
            {
                ASPxComboBox cubeCombo = (ASPxComboBox)e.Editor;
                cubeCombo.Items.Clear();
                switch (HttpContext.Current.Session["userType"].ToString())
                {
                    case "0":
                        {
                            foreach (string s in InformationProvider.GetAllCubes())
                                cubeCombo.Items.Add(s);

                            break;
                            //should chould choose
                        }
                    case "1":
                        {
                            string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));

                            cubeCombo.Items.Add(cname);
                            cubeCombo.SelectedIndex = 0;
                            cubeCombo.ReadOnly = true;
                            break;
                        }
                    case "2":
                        {
                            string cname = InformationProvider.GetCubeName(Convert.ToInt32(HttpContext.Current.Session["users2ID"]));
                            cubeCombo.Items.Add(cname);
                            cubeCombo.SelectedIndex = 0;
                            cubeCombo.ReadOnly = true;
                            break;
                        }
                }
            }
        }

        protected void ASPxGridView1_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            if (this.Grid_Clients.IsEditing)
                this.Grid_Clients.Settings.ShowFilterRow = false;

            if (this.Grid_Clients.Columns[1].Visible == false)
            {
                this.Grid_Clients.Columns[1].Visible = true;
                this.Grid_Clients.Columns[7].Visible = true;
            }
            e.NewValues["Id"] = InformationProvider.GetNextId();

        }

        /// <summary>
        /// Validate all fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_Clients_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (!e.IsNewRow)
                return;
            //Check for null values
            bool oneIsNull = false;

            if (e.NewValues["Username"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Username"] as GridViewDataColumn;
                e.Errors.Add(col, "You must provide a username");
            }

            if (e.NewValues["Password"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Password"] as GridViewDataColumn;
                e.Errors.Add(col, "You must provide a password");
            }

            if (e.NewValues["Email"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Email"] as GridViewDataColumn;
                e.Errors.Add(col, "You must provide a valid email adress");
            }
            if (e.NewValues["UserType"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["UserType"] as GridViewDataColumn;
                e.Errors.Add(col, "You must select a usertype");
            }

            if (e.NewValues["Language"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Language"] as GridViewDataColumn;
                e.Errors.Add(col, "You must select a language");
            }
            if (e.NewValues["Currency"] == null)
            {
                oneIsNull = true;
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Currency"] as GridViewDataColumn;
                e.Errors.Add(col, "You must select a currency");
            }

            if (oneIsNull)
            {

                //MGA: new
                if (this.Grid_Clients.IsEditing)
                    this.Grid_Clients.Settings.ShowFilterRow = false;

                if (this.Grid_Clients.Columns[1].Visible == false)
                {
                    this.Grid_Clients.Columns[1].Visible = true;
                    this.Grid_Clients.Columns[7].Visible = true;
                }
                e.NewValues["Id"] = InformationProvider.GetNextId();
                return;
            }
            if (!InformationProvider.ValidUsername(e.NewValues["Username"].ToString()))
            {
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Username"] as GridViewDataColumn;
                e.Errors.Add(col, "Username exists");

            }
            if (!e.NewValues["Email"].ToString().Contains("@"))
            {
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["Email"] as GridViewDataColumn;
                e.Errors.Add(col, "You must provide a valid email adress");
            }

            //MGa new
            if (this.Grid_Clients.IsEditing)
                this.Grid_Clients.Settings.ShowFilterRow = false;

            if (this.Grid_Clients.Columns[1].Visible == false)
            {
                this.Grid_Clients.Columns[1].Visible = true;
                this.Grid_Clients.Columns[7].Visible = true;
            }
            e.NewValues["Id"] = InformationProvider.GetNextId();


        }

        [WebMethod()]
        public static string RequestClients(string id)
        {
            var user = DdDReportUser.GetUser(Convert.ToInt32(id));
            //  var user = (DdDReportUser)HttpContext.Current.Session["userObject"];
            List<Client> clients = InformationProvider.AvailableClients(user, user.Clients);
            var returnValues = new Dictionary<string, string>();
            returnValues.Add("selected_clients", LitJson.JsonMapper.ToJson(user.Clients));
            returnValues.Add("unselected_clients", LitJson.JsonMapper.ToJson(clients.Select(x => x.ClientID).ToArray()));
            return LitJson.JsonMapper.ToJson(returnValues);
        }

        protected void Grid_Clients_DataBound(object sender, EventArgs e)
        {


            if (this.Grid_Clients.PageCount > 1)
                this.Grid_Clients.Settings.ShowFilterRow = true;
            else
                this.Grid_Clients.Settings.ShowFilterRow = false;
        }

        protected void Grid_Clients_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }

        protected void Grid_Clients_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            if (this.Grid_Clients.IsEditing)
                this.Grid_Clients.Settings.ShowFilterRow = false;

            if (this.Grid_Clients.Columns[1].Visible == false)
            {
                this.Grid_Clients.Columns[1].Visible = true;
                this.Grid_Clients.Columns[7].Visible = true;
            }
        }

        protected void Grid_Clients_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            newKey = e.NewValues["Id"];

        }

        protected void Grid_Clients_DataBound1(object sender, EventArgs e)
        {
            Grid_Clients.FocusedRowIndex = Grid_Clients.FindVisibleIndexByKeyValue(newKey);
            newKey = null;

        }
    }
}