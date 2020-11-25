<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="ResetReports.aspx.cs" Inherits="DdDReportAdmin.ResetReports" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="Server">
    <dx:ASPxRoundPanel ID="rpResetReports" runat="server" ShowCollapseButton="true" Width="200px">
        <HeaderTemplate>
            Reset reports:
        </HeaderTemplate>
        <PanelCollection>
            <dx:PanelContent>
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" ValueType="System.String"
                                Width="250px">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	cbpanel.PerformCallback();
}" />
                            </dx:ASPxComboBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server"
                                ClientInstanceName="cbpanel"
                                OnCallback="ASPxCallbackPanel1_Callback">
                                <PanelCollection>
                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server"
                                                        Text="All reports below will be deleted!">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxListBox ID="ASPxListBox1" runat="server" Rows="15" Height="400px"
                                                        Width="250px">
                                                    </dx:ASPxListBox>
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click"
                                                        Text="Delete Reports">
                                                    </dx:ASPxButton>
                                                </td>
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

