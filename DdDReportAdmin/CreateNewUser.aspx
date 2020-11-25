<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="CreateNewUser.aspx.cs" Inherits="DdDReportAdmin.CreateNewUser" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" Runat="Server">

    <script language="javascript" type="text/javascript" src="jquery-1.3.2.js"></script>
    <style type="text/css">
    .style3
    {
            width: 866px;
        }
        .style4
        {
            width: 91px;
        }
        </style>
    <script type="text/javascript" language="javascript">
        function ConnectCellclick() {
            $(".cellimg").click(function(event) {
                var clientid = $(this).attr("alt");
                var link = "./ClientSelector.aspx?userid=" + clientid;
                pop.SetContentUrl(link);
                pop.Show();
           });
        }

        $(document).ready(function() {
            ConnectCellclick();
        });
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" Runat="Server">
    <%= DdDReportAdmin.HTMLHelpers.RoundedBoxTop("") %>
    <div class="dataHeader">Maintain users:</div>
    <div class="content">
    <table class="style1">
        <tr>
            <td class="style3">
                <dxe:ASPxButton ID="btn_newUser" runat="server" AutoPostBack="False" 
                    Text="New User">
                    <ClientSideEvents Click="function(s, e) {
	grid.AddNewRow();
}" />
                </dxe:ASPxButton>
            </td>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style4">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td align="left" valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
    <dx:ASPxGridView ID="Grid_Clients" runat="server" 
    AutoGenerateColumns="False" DataSourceID="DatasourceUserObj" KeyFieldName="Id" 
                    ClientInstanceName="grid" 
                    oncelleditorinitialize="ASPxGridView1_CellEditorInitialize" 
                    oninitnewrow="ASPxGridView1_InitNewRow" 
                    onrowvalidating="Grid_Clients_RowValidating" Width="850px" 
                    onrowcommand="Grid_Clients_RowCommand" 
                    onstartrowediting="Grid_Clients_StartRowEditing" 
                    ondatabound="Grid_Clients_DataBound1" onrowinserted="Grid_Clients_RowInserted">
    <ClientSideEvents RowDblClick="function(s, e) {
grid.StartEditRow(e.visibleIndex);

}"
EndCallback="function(s, e) {
	ConnectCellclick();
}"
/>

        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" 
            ProcessFocusedRowChangedOnServer="False"/>
    <SettingsEditing Mode="EditForm" PopupEditFormModal="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this user?" />
<ClientSideEvents RowDblClick="function(s, e) {
grid.StartEditRow(e.visibleIndex);

}"></ClientSideEvents>
    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0">
            <EditButton Visible="True">
            </EditButton>
            <DeleteButton Visible="True">
            </DeleteButton>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="1" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Username" VisibleIndex="2">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Password" VisibleIndex="3">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="4">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataComboBoxColumn FieldName="Currency" VisibleIndex="5">
            <PropertiesComboBox TextField="Currency" 
                ValueType="System.String">
                <Items>
                    <dx:ListEditItem Text="DKK" Value="dk" />
                    <dx:ListEditItem Text="SEK" Value="se" />
                    <dx:ListEditItem Text="EUR" Value="eu" />
                    <dx:ListEditItem Text="NRK" Value="no" />
                </Items>
            </PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="Language" VisibleIndex="6">
            <PropertiesComboBox TextField="Language" ValueType="System.String">
                <Items>
                    <dx:ListEditItem Text="Danish" Value="da-DK" />
                    <dx:ListEditItem Text="English" Value="en-GB" />
                    <dx:ListEditItem Text="Swedish" Value="sv-SE" />
                    <dx:ListEditItem Text="German" Value="de-DE" />
                </Items>
            </PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="Cubename" VisibleIndex="7">
            <PropertiesComboBox ValueType="System.String">
                
            </PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="UserType" VisibleIndex="8">
            <PropertiesComboBox ValueType="System.String">
                <Items>
                    <dx:ListEditItem Text="Normal User" Value="-1" />
                    <dx:ListEditItem Text="DdDSupport" Value="0" />
                    <dx:ListEditItem Text="Cube administrator" Value="1" />
                    <dx:ListEditItem Text="Concern administrator" Value="2" />
                </Items>
            </PropertiesComboBox>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataColumn Caption="Active" VisibleIndex="9" Name="img_col" 
            ReadOnly="True">
                    <DataItemTemplate>
                        <img id="Img1" class="cellimg" runat="server" src='<%# GetImageName(Eval("Id")) %>' alt='<%# GetUserId(Eval("Id")) %>' title='Click to view clients' height="16" width="16"/>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>

    </Columns>
        <Settings GridLines="Horizontal" ShowFilterRow="True" />
        
<SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" 
            ProcessFocusedRowChangedOnServer="False"></SettingsBehavior>

<SettingsEditing Mode="EditForm" PopupEditFormModal="True"></SettingsEditing>

<Settings GridLines="Horizontal" ShowFilterRow="True"></Settings>

<SettingsText ConfirmDelete="Are you sure you want to delete this user?"></SettingsText>

        <StylesEditors>
            <ReadOnlyStyle BackColor="#CCCCCC">
            </ReadOnlyStyle>
            <ReadOnly BackColor="#CCCCCC">
            </ReadOnly>
        </StylesEditors>
        
</dx:ASPxGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
            </td>
            <td align="left" valign="top">

                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                <dxpc:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
                    ClientInstanceName="pop" ContentUrl="~/Blank.aspx" 
                    HeaderText="Active clients" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Height="500px" Width="400px" 
                    CloseAction="CloseButton">
                    <ClientSideEvents CloseUp="function(s, e) {
	pop.SetContentUrl(&quot;./Blank.aspx&quot;);
	grid.Refresh();
}" />
                    <ContentCollection>
<dx:PopupControlContentControl runat="server"></dx:PopupControlContentControl>
</ContentCollection>
                </dxpc:ASPxPopupControl>
            </td>
            <td align="left" valign="top">
                &nbsp;</td>
        </tr>
    </table>
    </div>
<%= DdDReportAdmin.HTMLHelpers.RoundedBoxBottom() %>
<asp:ObjectDataSource ID="DatasourceUserObj" runat="server" 
    DataObjectTypeName="DdDReportUser" 
    TypeName="DdDReportUser" UpdateMethod="UpdateUser" DeleteMethod="DeleteUser" 
        InsertMethod="InsertUser" EnableCaching="True" 
        EnablePaging="True" SelectMethod="GetUsers">
    <SelectParameters>
        <asp:SessionParameter Name="usertype" SessionField="userType" Type="String" />
        <asp:SessionParameter DefaultValue="" Name="control" SessionField="control" 
            Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
    </asp:Content>