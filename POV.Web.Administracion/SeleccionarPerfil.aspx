<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultSite.Master" AutoEventWireup="true"
    CodeBehind="SeleccionarPerfil.aspx.cs" Inherits="POV.Web.Administracion.SeleccionarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>Icons.css" rel="stylesheet" type="text/css" />
    <style>
        body {
            list-style-type: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="seleccion-escuela ui-widget-content" style="padding: 10px; min-height: 300px;">
        <h1>
            Selecciona un perfil</h1>
        <h2>
            <asp:Label ID="LblErrorPerfil" runat="server" CssClass="error"></asp:Label></h2>
        <ul>
            <li class="item_pub">
                <asp:LinkButton ID="LnkBtnSeleccionarAutoridad" runat="server" Font-Size="20"
                    OnClick="Btn_Autoridad_OnClick">
                    <i class="icon_48 icon_user_profile48"></i>
                    <span>Autoridad estatal</span>
                </asp:LinkButton>
            </li>
            <li class="item_pub">
                <asp:LinkButton ID="LnkBtnSeleccionarDirector"  runat="server" Font-Size="20"
                    OnClick="Btn_Director_OnClick">
                    <i class="icon_48 icon_escuela"></i>
                    <span>Director</span>
                </asp:LinkButton>
            </li>
        </ul>
    </div>
</asp:Content>
