<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PruebaDiagnostica.aspx.cs" Inherits="POV.Web.PortalSocial.Auth.PruebaDiagnostica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY</title>
    <link rel="shortcut icon" runat="server" href="~/Images/Yoy_Favicon20px.png" />
    <link href="~/Styles/Default.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery-1.7.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
            redireccionar();
        };
        function redireccionar() {
            var dir = $("#url").val();
            location.href = dir;
        }
    </script>
</head>
<body style="background-color:#fff;">
     <form id="form1" runat="server">
     <asp:HiddenField ID="url" runat="server" />
    
    
    </form>
</body>
</html>
