<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="DistributeReports.aspx.cs" Inherits="DdDReportAdmin.DistributeReports" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="Server">
	<script type="text/javascript" language="javascript">
		var ctreeall = false;
		var rtreeall = false;
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="Server">
	<dx:ASPxRoundPanel ID="rpDistributeReports" runat="server" ShowCollapseButton="true" Width="200px" Theme="Office2010Blue">
		<HeaderTemplate>
			Distribute reports:
		</HeaderTemplate>
		<PanelCollection>
			<dx:PanelContent>
				<table class="style1">
					<tr>
						<td>
							<table class="style1">
								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Choose user:">
										</dx:ASPxLabel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" ValueType="System.String"
											DropDownRows="20">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cbpanel.PerformCallback();
}" />
										</dx:ASPxComboBox>
									</td>
								</tr>
							</table>
						</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td colspan="2">
							<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server"
								ClientInstanceName="cbpanel"
								OnCallback="ASPxCallbackPanel1_Callback" Width="547px">
								<PanelCollection>
									<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
										<table class="style1">
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Reports for user:">
													</dx:ASPxLabel>
												</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Receipents:">
													</dx:ASPxLabel>
												</td>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxTreeList ID="ASPxTreeList1" runat="server"
														AutoGenerateColumns="False" ClientInstanceName="ctree"
														OnCustomCallback="ASPxTreeList1_CustomCallback">

														<Settings ShowTreeLines="False" ShowColumnHeaders="False"></Settings>

														<SettingsSelection Enabled="True"></SettingsSelection>
														<Columns>
															<dx:TreeListTextColumn Caption="Clients" FieldName="Client"
																ShowInCustomizationForm="True" VisibleIndex="0">
																<PropertiesTextEdit EncodeHtml="False">
																</PropertiesTextEdit>
															</dx:TreeListTextColumn>
														</Columns>
														<Settings ShowColumnHeaders="False" ShowTreeLines="False" />
														<SettingsSelection Enabled="True" />

														<Styles>
															<SelectedNode BackColor="White" ForeColor="Black">
															</SelectedNode>
														</Styles>
													</dx:ASPxTreeList>
												</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td valign="top">
													<dx:ASPxTreeList ID="ASPxTreeList2" runat="server"
														AutoGenerateColumns="False" ClientInstanceName="rtree"
														OnCustomCallback="ASPxTreeList2_CustomCallback">

														<Settings ShowTreeLines="False" ShowColumnHeaders="False"></Settings>

														<SettingsSelection Enabled="True"></SettingsSelection>

														<Columns>
															<dx:TreeListTextColumn Caption="Receipents" FieldName="Receipent"
																ShowInCustomizationForm="True" VisibleIndex="0">
															</dx:TreeListTextColumn>
														</Columns>
														<Settings ShowColumnHeaders="False" ShowTreeLines="False" />
														<SettingsSelection Enabled="True" />

														<Styles>
															<SelectedNode BackColor="White" ForeColor="Black">
															</SelectedNode>
														</Styles>

													</dx:ASPxTreeList>
												</td>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td colspan="3">&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Select / Deselect all"
														Width="155px" AutoPostBack="False">
														<ClientSideEvents Click="function(s, e) {
	ctreeall = !ctreeall;
	ctree.PerformCallback(ctreeall);
}" />
													</dx:ASPxButton>
												</td>
												<td>&nbsp;</td>
												<td>&nbsp;</td>
												<td>
													<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Select / Deselect all"
														Width="155px" AutoPostBack="False">
														<ClientSideEvents Click="function(s, e) {
	rtreeall = !rtreeall;
	rtree.PerformCallback(rtreeall);
}" />
													</dx:ASPxButton>
												</td>
												<td>
													<dx:ASPxButton ID="ASPxButton3" runat="server" OnClick="ASPxButton3_Click"
														Text="Save changes">
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
