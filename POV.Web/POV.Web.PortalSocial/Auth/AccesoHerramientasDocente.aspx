<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoHerramientasDocente.aspx.cs" Inherits="POV.Web.PortalSocial.Auth.AccesoHerramientasDocente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY - ORIENTADOR</title>
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