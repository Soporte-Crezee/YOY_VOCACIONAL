<%@ Page Title="Página no encontrada" Language="C#" MasterPageFile="~/Errores.Master"
    AutoEventWireup="true" CodeBehind="PageNotFound.aspx.cs" Inherits="POV.Web.Administracion.Errors.PageNotFound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 700px; margin-top: 100px;">
    </div>
    <div class="ui-state-error ui-corner-all" style="padding: 0 .7em; margin: 0 auto 0 auto;
        width: 500px;font-size: 23px;">
        <p>
            <strong>Error:</strong> Página no encontrada.</p>
    </div>
</asp:Content>
