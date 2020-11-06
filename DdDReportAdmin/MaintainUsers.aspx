<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="MaintainUsers.aspx.cs" Inherits="DdDReportAdmin.MaintainUsers" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" Runat="Server">
	<style type="text/css">
		.style4
		{
			width: 100px;
		}
		.style5
		{
			width: 126px;
		}
		.style8
		{
			width: 45px;
		}
		.style9
		{
			width: 7px;
		}
		.style10
		{
			width: 219px;
		}
		.style11
		{
			width: 110px;
		}
		.style12
		{
			height: 44px;
		}
		.style13
		{
			width: 100px;
		}
		.dataHeader
		{
			height: 20px;
			width: 99%;
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<%= DdDReportAdmin.HTMLHelpers.RoundedBoxTop("") %>
	<div class="dataHeader">Maintain users:</div>
	<div class="content">
	<table class="style1">
	<tr>
		<td>
			<table class="style1">
				<tr>
					<td class="style4">
						Cube</td>
					<td class="style5">
						<dx:ASPxComboBox ID="cb_searchCube" runat="server" 
				ValueType="System.String" DropDownHeight="500px" EnableIncrementalFiltering="True" 
							ClientInstanceName="cb_searchCube" IncrementalFilteringMode="StartsWith">
				<ClientSideEvents SelectedIndexChanged="function(s, e) {
 lastAction = &quot;&quot;;
 cb_language.SetText(&quot;&quot;);
 cb_concern.SetText(&quot;&quot;);
 cb_cube.SetText(&quot;&quot;);
 cb_currency.SetText(&quot;&quot;);
 cb_user.SetText(&quot;&quot;);
 txt_id.SetText(&quot;&quot;);
 txt_username.SetText(&quot;&quot;);
 txt_password.SetText(&quot;&quot;);
 txt_email.SetText(&quot;&quot;);
 cb_panel2.PerformCallback();
}" />
			</dx:ASPxComboBox>
					</td>
					<td>
						&nbsp;</td>
					<td>
						<asp:HiddenField ID="HiddenField1" runat="server" Value="Secret! ;)" />
					</td>
					<td>
						<asp:HiddenField ID="HiddenField2" runat="server" />
					</td>
				</tr>
			</table>
		</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="style12">
			<dx:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" 
				ClientInstanceName="cb_panel2" Width="200px" 
				oncallback="ASPxCallbackPanel2_Callback">
				<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<script src="Scripts/jquery-1.3.2.js" type="text/javascript">
</script>
	<script type="text/javascript" type="text/javascript">


	var lastAction = "";
  
	$(document).ready(function() {

		SetCombo();
		ShowClients();
	});

	function SetCombo() {
		var v = $("#ctl00_ContentPlaceHolder1_HiddenField1").val();
		//  alert(v);

		switch (v) {
			case "0":
				cb_cube.SetEnabled(true);
				cb_concern.SetEnabled(true);
				break;
			case "1":
				cb_cube.SetEnabled(false);
				cb_concern.SetEnabled(true);
				break;
			case "2":
				cb_cube.SetEnabled(false);
				cb_concern.SetEnabled(false);
		}
	}

	function Notify() {
//        alert("lastaction: " + lastAction);
		if (lastAction == "")
			return;

		var v = $("#ctl00_ContentPlaceHolder1_ASPxCallbackPanel1_Msg").text();

//        alert("v: " + v);

		if (v == "saveError" || v == "usernameTaken") {
			lastAction = v;
			$("#ctl00_ContentPlaceHolder1_ASPxCallbackPanel1_Msg").text('')
		}
		switch (lastAction) {
			case "save":
				{
					alert("Save sucess");
					lastAction = "";
					btn_show.SetEnabled(true);
					break;
				}
			case "delete":
				{
					lastAction = "";
					cb_language.SetText("");
					cb_concern.SetText("");
					cb_cube.SetText("");
					cb_search.SetText("");
					cb_currency.SetText("");
					cb_user.SetText("");
					txt_id.SetText("");
					txt_username.SetText("");
					txt_password.SetText("");
					txt_email.SetText("");
					alert("delete sucess"); 

					break;
				}
			case "new":
				{
					lastAction = "";
					btn_show.SetEnabled(false);
					break;
				}
				case "saveError": alert("Could not save. Please correct errors");break;
				case "usernameTaken": alert("Username in use. Please enter another.");break;
				
			default: lastAction = "";
		}
	  }

	  function ShowClients() {
		  $("#ctl00_ContentPlaceHolder1_ASPxCallbackPanel1_btn_show").click(function(event) {
			  var clientid = txt_id.GetValue();
			  var link = "./ClientSelector.aspx?userid=" + clientid;
			  pop.SetContentUrl(link);
			  pop.Show();
		  });
	  }
	
	
	
	
</script>
	<div>
		<table style="width: 493px">
			<tr>
				<td class="style13">
					Choose user:</td>
				<td>
					<dx:ASPxComboBox ID="cb_search" runat="server" ClientInstanceName="cb_search" 
						DropDownHeight="500px" EnableIncrementalFiltering="True" 
						IncrementalFilteringMode="StartsWith" ValueType="System.String">
						<ClientSideEvents SelectedIndexChanged="function(s, e) {
 lastAction = &quot;&quot;;
 cb_language.SetText(&quot;&quot;);
 cb_concern.SetText(&quot;&quot;);
 cb_cube.SetText(&quot;&quot;);
 cb_currency.SetText(&quot;&quot;);
 cb_user.SetText(&quot;&quot;);
 txt_id.SetText(&quot;&quot;);
 txt_username.SetText(&quot;&quot;);
 txt_password.SetText(&quot;&quot;);
 txt_email.SetText(&quot;&quot;);
 cpanel.PerformCallback();
}" />
					</dx:ASPxComboBox>
				</td>
				<td>
					<dx:ASPxButton ID="btn_new" runat="server" AutoPostBack="False" 
						Text="Create new user" Width="198px">
						<ClientSideEvents Click="function(s, e) {
							 lastAction = &quot;new&quot;;
cb_language.SetText(&quot;&quot;);
cb_concern.SetText(&quot;&quot;);
cb_cube.SetText(&quot;&quot;);
cb_search.SetText(&quot;&quot;);
cb_currency.SetText(&quot;&quot;);
cb_user.SetText(&quot;&quot;);
btn_show.SetEnabled(false);
cpanel.PerformCallback(&quot;new&quot;);
}" />
					</dx:ASPxButton>
				</td>
			</tr>
		</table>
		<br />
	</div>
					</dx:PanelContent>
</PanelCollection>
			</dx:ASPxCallbackPanel>
		</td>
		<td class="style12">
			</td>
	</tr>
	<tr>
		<td>
			<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Height="418px" 
				Width="1012px" ClientInstanceName="cpanel" 
				oncallback="ASPxCallbackPanel1_Callback1">
				<PanelCollection>
<dx:PanelContent runat="server">
	<table class="style1">
		<tr>
			<td class="style8">
				ID:</td>
			<td class="style10">
				<dx:ASPxTextBox ID="txt_id" runat="server" ReadOnly="True" Width="170px" 
					OnValidation="txt_text_Validation" ClientInstanceName="txt_id">
				</dx:ASPxTextBox>
			</td>
			<td class="style11">
				Currency:</td>
			<td>
				<dx:ASPxComboBox ID="cb_currency" runat="server" ValueType="System.String" 
					ClientInstanceName="cb_currency" OnValidation="cb_combo_Validation">
					<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td class="style8">
				Username:</td>
			<td class="style10">
				<dx:ASPxTextBox ID="txt_username" runat="server" Width="170px" 
					OnValidation="txt_text_Validation" ClientInstanceName="txt_username">
					<ValidationSettings CausesValidation="True">
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
			<td class="style11">
				Password:</td>
			<td>
				<dx:ASPxTextBox ID="txt_password" runat="server" Width="170px" 
					OnValidation="txt_text_Validation" ClientInstanceName="txt_password">
					<ValidationSettings CausesValidation="True" EnableCustomValidation="True">
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td class="style8">
				Email:</td>
			<td class="style10">
				<dx:ASPxTextBox ID="txt_Email" runat="server" Width="170px" 
					OnValidation="txt_text_Validation" ClientInstanceName="txt_email">
					<ValidationSettings CausesValidation="True" EnableCustomValidation="True">
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
			<td class="style11">
				Language:</td>
			<td>
				<dx:ASPxComboBox ID="cb_language" runat="server" ValueType="System.String" 
					ClientInstanceName="cb_language" OnValidation="cb_combo_Validation">
					<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td class="style8">
				Cubename:</td>
			<td class="style10">
				<dx:ASPxComboBox ID="cb_cubename" runat="server" ClientInstanceName="cb_cube" 
					ValueType="System.String" OnValidation="cb_combo_Validation">
					<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
					</ValidationSettings>
					<ClientSideEvents TextChanged="function(s,e) {
					cb_concern.PerformCallback(cb_cube.GetText());
					
					}" />
				</dx:ASPxComboBox>
			</td>
			<td class="style11">
				Concern name</td>
			<td>
				<dx:ASPxComboBox ID="cb_concern" runat="server" 
					ClientInstanceName="cb_concern" ValueType="System.String" 
					OnCallback="cb_concern_Callback">
					<ValidationSettings CausesValidation="False">
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td class="style8">
				Userrights:</td>
			<td class="style10">
				<dx:ASPxComboBox ID="cb_user" runat="server" 
					ClientInstanceName="cb_user" ValueType="System.String" 
					OnValidation="cb_combo_Validation">
					<ValidationSettings CausesValidation="False">
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
			<td class="style11">
				User active:</td>
			<td>
				<dx:ASPxImage ID="img_status" runat="server" Height="32px" IsPng="True" 
					Width="32px">
				</dx:ASPxImage>
			</td>
		</tr>
	</table>
	<table>
	<tr>
	<td>
		<dx:ASPxButton ID="btn_show" runat="server" AutoPostBack="False" 
			Text="Show / Change shops" Width="198px" ClientInstanceName="btn_show">
			<ClientSideEvents Click="function(s, e) {
	ShowClients();
}" CheckedChanged="function(s, e) {
	pop.ShowOpup();
}" />
		</dx:ASPxButton>
		</td><td class="style9">
			<dx:ASPxButton ID="btn_delete" runat="server" AutoPostBack="False" 
				Text="Delete user" Width="198px">
				<ClientSideEvents Click="function(s, e) {
var res = confirm(&quot;Are you sure you want to delete this user?&quot;);
if(res)
	{
	lastAction = &quot;delete&quot;;
cpanel.PerformCallback(&quot;delete&quot;);
}
else
	{
	}
}" />
			</dx:ASPxButton>
		</td><td>
			<dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" 
				Text="Save changes" Width="198px">
				<ClientSideEvents Click="function(s, e) {
				lastAction = &quot;save&quot;;
	var a = cb_language.GetText();
	var b = cb_cube.GetText();
	var c = cb_concern.GetText();
	var d = cb_user.GetText();
	var e = cb_currency.GetText();
	var msg = &quot;save#&quot; + a+&quot;#&quot;+b +&quot;#&quot;+c +&quot;#&quot;+d +&quot;#&quot;+e;
	btn_show.SetEnabled(true);
	cpanel.PerformCallback(msg);
}" />
			</dx:ASPxButton>
		</td>
		<td>
			<dx:ASPxLabel ID="Msg" runat="server" ClientInstanceName="msg" 
				ForeColor="White" Text="ASPxLabel">
			</dx:ASPxLabel>
		</td>
	</tr>
	</table>
 </dx:PanelContent>
</PanelCollection>
			  
				<ClientSideEvents EndCallback="function(s, e) {
	SetCombo();
	Notify();
}" />
			  
			</dx:ASPxCallbackPanel>
		</td>
		<td>
			&nbsp;</td>
	</tr>
</table>
<table>
<tr>
<td>
  <dx:aspxpopupcontrol ID="ASPxPopupControl1" runat="server" 
					ClientInstanceName="pop" ContentUrl="~/Blank.aspx" 
					HeaderText="Active clients" PopupHorizontalAlign="WindowCenter" 
					PopupVerticalAlign="WindowCenter" Height="500px" Width="400px" 
					CloseAction="CloseButton">
					<ClientSideEvents CloseUp="function(s, e) {
	pop.SetContentUrl(&quot;./Blank.aspx&quot;);
	cpanel.PerformCallback(&quot;reload&quot;);
}" />
					<ContentCollection>
<dx:PopupControlContentControl runat="server"></dx:PopupControlContentControl>
</ContentCollection>
				</dx:aspxpopupcontrol>
</td>
</tr>
</table>

</div>
<%= DdDReportAdmin.HTMLHelpers.RoundedBoxBottom() %>
	</asp:Content>

