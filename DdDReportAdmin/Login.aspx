<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DdDReportAdmin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="styles/login.css" type="text/css" />
    <link rel="stylesheet" href="styles/layout.css" type="text/css" />
<!--[if lt IE 7]>
<link rel="stylesheet" href="styles/ie-fixes.css" type="text/css" />
<![endif]-->
<!--[if lt IE 8]>
<style>
   .box {
      zoom: 1; /* to turn on hasLayout in IE */
   }
</style>
<![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <div class="centerbox">
        <%= DdDReportAdmin.HTMLHelpers.RoundedBoxTop("") %>
        <div class="dataHeader">Log In</div>    
        <div class="loginContent">
            <asp:Login ID="LoginControl" runat="server" onauthenticate="Login1_Authenticate" 
                       TitleText=""></asp:Login>
        </div>
        <%= DdDReportAdmin.HTMLHelpers.RoundedBoxBottom() %>
        </div>
    </form>
</body>
</html>
