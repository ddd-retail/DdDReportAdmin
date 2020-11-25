<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MaintainUsers.aspx.cs" Inherits="DdDReportAdmin.MaintainUsers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="server">
    <script src="Scripts/jquery-1.3.2.js" type="text/javascript">
	</script>
	<script type="text/javascript">


		var lastAction = "";

		$(document).ready(function () {

			SetCombo();
			ShowClients();
		});

		function SetCombo() {
		    debugger;
			var v = $("#ctl00_ContentPlaceHolder1_HiddenField1").val();
			//  alert(v);

			switch (v) {
				case "0":
				    cbCubename.SetEnabled(true);
					cbConcern.SetEnabled(true);
					break;
				case "1":
				    cbCubename.SetEnabled(false);
					cbConcern.SetEnabled(true);
					break;
				case "2":
				    cbCubename.SetEnabled(false);
					cbConcern.SetEnabled(false);
			}
		}

		function Notify() {
		    //        alert("lastaction: " + lastAction);
		    debugger;
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
						btnShow.SetEnabled(true);
						break;
					}
				case "delete":
					{
						lastAction = "";
						cbLanguage.SetText("");
						cbConcern.SetText("");
						cbCubename.SetText("");
						cbSearch.SetText("");
						cbCurrency.SetText("");
						cbUser.SetText("");
						txtUserId.SetText("");
						txtUsername.SetText("");
						txtPassword.SetText("");
						txtEmail.SetText("");
						alert("delete sucess");

						break;
					}
				case "new":
					{
						lastAction = "";
						btnShow.SetEnabled(false);
						break;
					}
				case "saveError": alert("Could not save. Please correct errors"); break;
				case "usernameTaken": alert("Username in use. Please enter another."); break;

				default: lastAction = "";
			}
		}

		function ShowClients() {
			$("#btnShow").click(function (event) {
			    var clientid = txtUserId.GetValue();
				var link = "./ClientSelector.aspx?userid=" + clientid;
				pcPop.SetContentUrl(link);
				pcPop.Show();
			});
		}
	</script>
	<dx:ASPxRoundPanel ID="rpMaintainUsers" runat="server" ShowCollapseButton="true" Width="200px">
		<HeaderTemplate>
			Maintain users:
		</HeaderTemplate>
		<PanelCollection>
			<dx:PanelContent>
				<table>
					<tr>
						<td>
							<table>
								<tr>
									<td>
										<dx:ASPxComboBox ID="cbSearchCube" runat="server" ValueType="System.String" DropDownHeight="500px" IncrementalFilteringMode="StartsWith" ClientInstanceName="cbSearchCube">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {
												 lastAction = &quot;&quot;;
												 cbLanguage.SetText(&quot;&quot;);
												 cbConcern.SetText(&quot;&quot;);
												 cbCubename.SetText(&quot;&quot;);
												 cbCurrency.SetText(&quot;&quot;);
												 cbUser.SetText(&quot;&quot;);
												 txtUserId.SetText(&quot;&quot;);
												 txtUsername.SetText(&quot;&quot;);
												 txtPassword.SetText(&quot;&quot;);
												 txtEmail.SetText(&quot;&quot;);
												 cpControlPanel.PerformCallback();
												}" />
										</dx:ASPxComboBox>
									</td>
									<td>&nbsp;</td>
									<td>
										<asp:HiddenField ID="HiddenField1" runat="server" Value="Secret! ;)" />
									</td>
									<td>
										<asp:HiddenField ID="HiddenField2" runat="server" />
									</td>
								</tr>
							</table>
						</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxCallbackPanel ID="cpControlPanel" runat="server" ClientInstanceName="cpControlPanel" Width="200px" OnCallback="cpControlPanel_Callback">
								<PanelCollection>
									<dx:PanelContent runat="server">
										<table style="width: 493px">
											<tr>
												<td class="style13">Choose user:</td>
												<td>
													<dx:ASPxComboBox ID="cbSearch" runat="server" ClientInstanceName="cbSearch" ClientIDMode="Static"
														DropDownHeight="500px" EnableIncrementalFiltering="True"
														IncrementalFilteringMode="StartsWith" ValueType="System.String">
														<ClientSideEvents SelectedIndexChanged="function(s, e) {
 lastAction = &quot;&quot;;
 cbLanguage.SetText(&quot;&quot;);
 cbConcern.SetText(&quot;&quot;);
 cbCubename.SetText(&quot;&quot;);
 cbCurrency.SetText(&quot;&quot;);
 cbUser.SetText(&quot;&quot;);
 txtUserId.SetText(&quot;&quot;);
 txtUsername.SetText(&quot;&quot;);
 txtPassword.SetText(&quot;&quot;);
 txtEmail.SetText(&quot;&quot;);
 cpUser.PerformCallback();
}" />
													</dx:ASPxComboBox>
												</td>
												<td>
													<dx:ASPxButton ID="btnNew" runat="server" AutoPostBack="False"
														Text="Create new user" Width="198px" ClientInstanceName="btnNew" ClientIDMode="Static">
														<ClientSideEvents Click="function(s, e) {
							 lastAction = &quot;new&quot;;
cbLanguage.SetText(&quot;&quot;);
cbConcern.SetText(&quot;&quot;);
cbCubename.SetText(&quot;&quot;);
cbSearch.SetText(&quot;&quot;);
cbCurrency.SetText(&quot;&quot;);
cbUser.SetText(&quot;&quot;);
btnShow.SetEnabled(false);
cpUser.PerformCallback(&quot;new&quot;);
}" />
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
									</dx:PanelContent>
								</PanelCollection>
							</dx:ASPxCallbackPanel>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxCallbackPanel ID="cpUser" runat="server" ClientInstanceName="cpUser" ClientIDMode="Static"
								OnCallback="cpUser_Callback">
								<PanelCollection>
									<dx:PanelContent runat="server">
										<table class="style1">
											<tr>
												<td class="style8">ID:</td>
												<td class="style10">
													<dx:ASPxTextBox ID="txtUserId" runat="server" ReadOnly="True" Width="170px"
														OnValidation="txtText_Validation" ClientInstanceName="txtUserId" ClientIDMode="Static">
													</dx:ASPxTextBox>
												</td>
												<td class="style11">Currency:</td>
												<td>
													<dx:ASPxComboBox ID="cbCurrency" runat="server" ValueType="System.String"
														ClientInstanceName="cbCurrency" ClientIDMode="Static" OnValidation="cbCombo_Validation">
														<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td class="style8">Username:</td>
												<td class="style10">
													<dx:ASPxTextBox ID="txtUsername" runat="server" Width="170px"
														OnValidation="txtText_Validation" ClientInstanceName="txtUsername" ClientIDMode="Static">
														<ValidationSettings CausesValidation="True">
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style11">Password:</td>
												<td>
													<dx:ASPxTextBox ID="txtPassword" runat="server" Width="170px"
														OnValidation="txtText_Validation" ClientInstanceName="txtPassword" ClientIDMode="Static">
														<ValidationSettings CausesValidation="True" EnableCustomValidation="True">
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td class="style8">Email:</td>
												<td class="style10">
													<dx:ASPxTextBox ID="txtEmail" runat="server" Width="170px"
														OnValidation="txtText_Validation" ClientInstanceName="txtEmail" ClientIDMode="Static">
														<ValidationSettings CausesValidation="True" EnableCustomValidation="True">
														</ValidationSettings>
													</dx:ASPxTextBox>
												</td>
												<td class="style11">Language:</td>
												<td>
													<dx:ASPxComboBox ID="cbLanguage" runat="server" ValueType="System.String"
														ClientInstanceName="cbLanguage" ClientIDMode="Static" OnValidation="cbCombo_Validation">
														<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td class="style8">Cubename:</td>
												<td class="style10">
													<dx:ASPxComboBox ID="cbCubename" runat="server" ClientInstanceName="cbCubename" ClientIDMode="Static"
														ValueType="System.String" OnValidation="cbCombo_Validation">
														<ValidationSettings CausesValidation="False" EnableCustomValidation="True">
														</ValidationSettings>
														<ClientSideEvents TextChanged="function(s,e) {
					cbConcern.PerformCallback(cbCubename.GetText());
					
					}" />
													</dx:ASPxComboBox>
												</td>
												<td class="style11">Concern name</td>
												<td>
													<dx:ASPxComboBox ID="cbConcern" runat="server"
														ClientInstanceName="cbConcern" ValueType="System.String"
														OnCallback="cbConcern_Callback">
														<ValidationSettings CausesValidation="False">
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td class="style8">Userrights:</td>
												<td class="style10">
													<dx:ASPxComboBox ID="cbUser" runat="server"
														ClientInstanceName="cbUser" ClientIDMode="Static" ValueType="System.String"
														OnValidation="cbCombo_Validation">
														<ValidationSettings CausesValidation="False">
														</ValidationSettings>
													</dx:ASPxComboBox>
												</td>
												<td class="style11">User active:</td>
												<td>
													<dx:ASPxImage ID="img_status" runat="server" Height="32px"
														Width="32px">
													</dx:ASPxImage>
												</td>
											</tr>
										</table>
										<table>
											<tr>
												<td>
													<dx:ASPxButton ID="btnShow" runat="server" AutoPostBack="False"
														Text="Show / Change shops" Width="198px" ClientInstanceName="btnShow" ClientIDMode="Static">
														<ClientSideEvents Click="function(s, e) {
	ShowClients();
}"
															CheckedChanged="function(s, e) {
	pcPop.ShowOpup();
}" />
													</dx:ASPxButton>
												</td>
												<td class="style9">
													<dx:ASPxButton ID="btnDelete" runat="server" AutoPostBack="False"
														Text="Delete user" Width="198px">
														<ClientSideEvents Click="function(s, e) {
var res = confirm(&quot;Are you sure you want to delete this user?&quot;);
if(res)
	{
	lastAction = &quot;delete&quot;;
cpUser.PerformCallback(&quot;delete&quot;);
}
else
	{
	}
}" />
													</dx:ASPxButton>
												</td>
												<td>
													<dx:ASPxButton ID="btnSave" runat="server" AutoPostBack="False"
														Text="Save changes" Width="198px">
														<ClientSideEvents Click="function(s, e) {
				lastAction = &quot;save&quot;;
	var a = cbLanguage.GetText();
	var b = cbCubename.GetText();
	var c = cbConcern.GetText();
	var d = cbUser.GetText();
	var e = cbCurrency.GetText();
	var msg = &quot;save#&quot; + a+&quot;#&quot;+b +&quot;#&quot;+c +&quot;#&quot;+d +&quot;#&quot;+e;
	btnShow.SetEnabled(true);
	cpUser.PerformCallback(msg);
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
						<td>&nbsp;</td>
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
	 <dx:aspxpopupcontrol ID="pcPop" runat="server" 
					ClientInstanceName="pcPop" ContentUrl="~/Blank.aspx" ClientIDMode="Static"
					HeaderText="Active clients" PopupHorizontalAlign="WindowCenter" 
					PopupVerticalAlign="WindowCenter" Height="500px" Width="400px" 
					CloseAction="CloseButton">
					<ClientSideEvents CloseUp="function(s, e) {
	pcPop.SetContentUrl(&quot;./Blank.aspx&quot;);
	cpUser.PerformCallback(&quot;reload&quot;);
}" />
					<ContentCollection>
<dx:PopupControlContentControl runat="server"></dx:PopupControlContentControl>
</ContentCollection>
				</dx:aspxpopupcontrol>
</asp:Content>
