<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="UserInformation.aspx.cs" Inherits="DdDReportAdmin.UserInformation" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="Server">
    <style type="text/css">
        .style3 {
            width: 163px;
        }

        .style4 {
            width: 241px;
        }

        .style5 {
            width: 243px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="Server">
    <dx:ASPxRoundPanel ID="rpUserInformation" runat="server" ShowCollapseButton="true" Width="200px">
        <HeaderTemplate>
            Choose user:
        </HeaderTemplate>
        <PanelCollection>
            <dx:PanelContent>
                <table class="style1">
                    <tr>
                        <td>
                            <table class="style1">
                                <tr>
                                    <td class="style4">
                                        <dx:ASPxComboBox ID="ASPxComboBox2" runat="server"
                                            ValueType="System.String" DropDownHeight="500px">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	cpanel.PerformCallback();
}" />
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td class="style5">
                                        <dx:ASPxButton ID="btn_transdate" runat="server" AutoPostBack="False"
                                            Text="Show last transaction date" Width="198px">
                                            <ClientSideEvents Click="function(s, e) {
	cpanel.PerformCallback(&quot;trans&quot;);
}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_LoginAsUser" runat="server" AutoPostBack="False"
                                            Text="Logon to client account" Width="198px"
                                            OnClick="btn_LoginAsUser_Click">
                                            <ClientSideEvents Click="function(s, e) {
	cpanel.PerformCallback(&quot;trans&quot;);
}" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Height="497px"
                                Width="1012px" ClientInstanceName="cpanel"
                                OnCallback="ASPxCallbackPanel1_Callback">
                                <PanelCollection>
                                    <dx:PanelContent runat="server">
                                        <table class="style1">
                                            <tr>
                                                <td class="style3">Username:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_username" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td align="left" style="font-weight: bold">Available Clients:</td>
                                            </tr>
                                            <tr>
                                                <td class="style3">Password:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_password" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td rowspan="5">
                                                    <dx:ASPxListBox ID="lst_clients" runat="server"
                                                        Rows="10">
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3">Email:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_email" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3">Language:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_language" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3">Currency:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_currency" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3">Cube name:</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_cubename" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3" valign="top">Last transaction date</td>
                                                <td>
                                                    <dx:ASPxLabel ID="lbl_lasttransaction0" runat="server">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3">&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3" style="font-size: medium; font-weight: bold">Log:</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style3" colspan="3" style="font-size: medium; font-weight: bold">
                                                    <dx:ASPxListBox ID="ASPxListBox1" runat="server" Height="269px" Width="977px">
                                                    </dx:ASPxListBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
