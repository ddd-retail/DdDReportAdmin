using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class ClientSelector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            string username = Page.Request.Params["userid"];
            Session["username"] = username;
            if (username == null || username == "")
                return;

            DdDReportUser user = DdDReportUser.GetUser(Convert.ToInt32(username));

            //MGA 19.09.2010: Sort
            user.ClientObjects.Sort(delegate (Client c1, Client c2)
            {
                return c1.Name.CompareTo(c2.Name);
            });

            int index = 0;
            foreach (Client c in user.ClientObjects)
            {
                CreateClientTree(index++, c.Name, c.ClientID.ToString(), null, true);
            }

            List<Client> availableClients = InformationProvider.AvailableClients(user, user.Clients);
            //MGA 19.09.2010: Sort
            availableClients.Sort(delegate (Client c1, Client c2)
            {
                return c1.Name.CompareTo(c2.Name);
            });

            foreach (Client c in availableClients)
            {
                CreateClientTree(index++, c.Name, c.ClientID.ToString(), null, false);
            }


        }

        TreeListNode CreateClientTree(int key, string text, string value, TreeListNode parentNode, bool selected)
        {
            TreeListNode node = ASPxTreeList1.AppendNode(key, parentNode);
            node["Client"] = text + " (" + value + ")";
            node["Clientid"] = value;
            if (!IsPostBack)
                node.Selected = selected;

            return node;
        }


        protected void ASPxTreeList1_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            foreach (TreeListNode node in ASPxTreeList1.Nodes)
                node.Selected = Convert.ToBoolean(e.Argument);
        }
        protected void btn_save_Click(object sender, EventArgs e)
        {

            DdDReportUser user = DdDReportUser.GetUser(Convert.ToInt32(Session["username"]));
            List<string> selected = new List<string>();

            foreach (TreeListNode node in ASPxTreeList1.Nodes)
                if (node.Selected)
                    selected.Add(node["Clientid"].ToString());
            try
            {
                InformationProvider.UpdateUserClientRelation(user, selected);
                Response.Write("Save sucess");
            }
            catch (Exception ex)
            {
                Response.Write("Save failed. Please contact support");
            }
        }
    }
}