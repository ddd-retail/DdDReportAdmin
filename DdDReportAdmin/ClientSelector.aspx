<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientSelector.aspx.cs" Inherits="DdDReportAdmin.ClientSelector" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.9.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
	</style>
	<script language="javascript">
		var all = false;
	</script>
	<script language="javascript" type="text/javascript">
	function doCloseButtonClick(){
		window.parent.ASPxPopupControl1.Hide();
	}
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<p>
		<table class="style1">
			<tr>
				<td>
					<table class="style1">
						<tr>
							<td width="300">
					<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
						Text="Select / deselect all" Width="142px">
						<ClientSideEvents Click="function(s, e) {
						all = !all;
	tree.PerformCallback(all);
}" />
					</dx:ASPxButton>
							</td>
							<td>
								<dxe:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" 
									onclick="btn_save_Click" Text="Save changes">
								</dxe:ASPxButton>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<dx:ASPxTreeList ID="ASPxTreeList1" runat="server" 
						AutoGenerateColumns="False" ClientInstanceName="tree" 
						oncustomcallback="ASPxTreeList1_CustomCallback">
						<Styles>
							<SelectedNode BackColor="White" ForeColor="Black">
							</SelectedNode>
							<FocusedNode BackColor="White" ForeColor="Black">
							</FocusedNode>
						</Styles>
						<SettingsSelection Enabled="True" />
						<Settings ShowColumnHeaders="False" ShowTreeLines="False" />
						<Columns>
							<dxwtl:TreeListTextColumn FieldName="Client" VisibleIndex="0">
							</dxwtl:TreeListTextColumn>
						</Columns>
						<Paddings PaddingLeft="0px" />
					</dx:ASPxTreeList>
				</td>
			</tr>
		</table>
	</p>
	<div>
	
	</div>
	</form>
</body>
</html>

