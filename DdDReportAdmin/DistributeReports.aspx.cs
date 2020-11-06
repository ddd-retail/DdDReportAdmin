using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DdDReportAdmin
{
    public partial class DistributeReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                List<string> users = DdDReportUser.Usernames(Session["userType"].ToString(), Session["control"].ToString(), Session["concern"].ToString());
                foreach (string user in users)
                {
                    this.ASPxComboBox1.Items.Add(user);
                }

            }
            else
            {
                if (this.ASPxComboBox1.Text != "")
                {
                    DdDReportUser selectedUser = DdDReportUser.GetUser(this.ASPxComboBox1.Text);
                    CreateTree(selectedUser);
                }
            }
        }

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            //Reset selection?
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList1.Nodes)
                node.Selected = false;
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList2.Nodes)
                node.Selected = false;
            //Populate report tree
            //foreach(DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList1.Nodes)
            //    node.Selected = Convert.ToBoolean(e.Parameter);

        }

        void CreateTree(DdDReportUser user)
        {
            int index = 0;
            foreach (string report in InformationProvider.getReports(user.Id))
            {
                CreateClientTree(index++, report, null);
            }

            List<DdDReportUser> users = DdDReportUser.GetUsers("1", user.Cubename, user.Concern);
            foreach (DdDReportUser u in users)
            {
                if (user.Id != u.Id)
                    CreateReceipentTree(Convert.ToInt32(u.Id), u.Username, null);
            }
            // TreeListNode parentNode = CreateNodeCore(0, "<b>Local Folders</b>", null);
            //TreeListNode searchFolders = CreateNodeCore(5, "<b>Search Folders</b>", null);
            //CreateNodeCore(6, "Categorized Mail", searchFolders);
            //CreateNodeCore(7, "Large Mail", searchFolders);
        }

        TreeListNode CreateClientTree(int key, string text, TreeListNode parentNode)
        {
            TreeListNode node = ASPxTreeList1.AppendNode(key, parentNode);
            node["Client"] = text;
            return node;
        }

        TreeListNode CreateReceipentTree(int key, string text, TreeListNode parentNode)
        {
            TreeListNode node = ASPxTreeList2.AppendNode(key, parentNode);
            node["Receipent"] = text;
            return node;
        }

        protected void ASPxTreeList1_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList1.Nodes)
                node.Selected = Convert.ToBoolean(e.Argument);
        }

        protected void ASPxTreeList2_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList2.Nodes)
                node.Selected = Convert.ToBoolean(e.Argument);
        }

        protected void ASPxButton3_Click(object sender, EventArgs e)
        {
            List<string> reports = new List<string>();
            List<string> clients = new List<string>();

            string selected = "";
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList1.Nodes)
                if (node.Selected)
                {
                    reports.Add(node.GetValue("Client").ToString());
                    //selected += node.GetValue("Client").ToString();
                }
            string receipents = "";
            foreach (DevExpress.Web.ASPxTreeList.TreeListNode node in ASPxTreeList2.Nodes)
                if (node.Selected)
                {
                    clients.Add(node.GetValue("Receipent").ToString());
                    // receipents += node.GetValue("Receipent").ToString();
                }

            DdDReportUser selectedUser = DdDReportUser.GetUser(this.ASPxComboBox1.Text);
            try
            {
                foreach (string report in reports)
                    foreach (string client in clients)
                    {
                        using (System.IO.StreamWriter l = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\copyreportlog.txt", true))
                        {
                            l.AutoFlush = true;
                            l.WriteLine("Copying report " + report + " to " + client);
                        }
                        InformationProvider.CopyReport(report, selectedUser.Id, client);
                    }

                Response.Write(string.Format("<h3>Distribution has been completed.</h3>", selected, receipents));
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<h3>Could not save reports. Please contact support. </h3>", selected, receipents));
            }
        }
    }
}