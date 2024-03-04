<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegresarPortalSocial.aspx.cs" Inherits="POV.Web.HerramientasDocente.Auth.RegresarPortalSocial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="<% =Page.ResolveClientUrl("~/Content/Scripts/")%>jquery-1.7.2.min.js" type="text/javascript"></script>
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
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="url" runat="server" />
    </div>
    </form>
</body>
</html>
