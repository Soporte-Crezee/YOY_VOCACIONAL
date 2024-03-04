<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeleccionarEscuela.aspx.cs"
    Inherits="POV.Web.PortalSocial.SeleccionarEscuela" %>

<%@ Import Namespace="POV.Web.PortalSocial.AppCode" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon"  href="~/Images/Yoy_Favicon20px.png"/>
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>Default.css" rel="stylesheet"
        type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>Icons.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="container" class="page_container">
        <%-- header --%>
        <div id="header">
            <div id="logo-banner">
            </div>
            <div id="menu-top" class="menu_container menu_top_container" >
                <ul class=" menu_horizontal" >
                    <li>&nbsp;</li>
                    <li id="opcSalir">
                        <asp:HyperLink ID="HplLogout" Text="" runat="server" CssClass="iconSalir">&nbsp;
                        </asp:HyperLink>
                    </li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
        </div>
        <div id="content">
            <h1 class="title_6">
                Selecciona una escuela</h1>
            <h2>
                <asp:Label ID="LblErrorEscuela" runat="server" CssClass="title_5 error"></asp:Label></h2>
            <ul>
                <asp:Repeater ID="RptEscuelas" runat="server" OnItemCommand="Escuelas_ItemCommand"
                    OnItemDataBound="Escuelas_DataBound">
                    <ItemTemplate>
                        <li class="item_pub">
                            <asp:LinkButton ID="LnkBtnSeleccionar" runat="server" Font-Size="14" CssClass="link_blue"
                                CommandArgument="<%#((EscuelaListItem)Container.DataItem).LicenciaID%>" CommandName="SELECT_LICENCIA">
                                <i class="icon_48 icon_escuela"></i>
                                <span><%#((EscuelaListItem)Container.DataItem).Escuela%> , Turno <%#((EscuelaListItem)Container.DataItem).Turno%></span>
                            </asp:LinkButton>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="LblSinEscuelas" runat="server" Visible="false">
                        <div id="more">
                            <span>No hay escuelas asignadas.</span>
                        </div>
                        </asp:Label>
                    </FooterTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <%-- footer --%>
    <div id="footer" class="page_container">
    </div>
    </form>
</body>
</html>
