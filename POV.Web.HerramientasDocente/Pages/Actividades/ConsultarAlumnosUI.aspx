<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarAlumnosUI.aspx.cs"
    Inherits="POV.Web.Pages.Actividades.ConsultarAlumnosUI" %>

<%@ Register Src="~/Pages/Actividades/ConsultarAlumnosUC.ascx" TagName="ConsultarAlumnosUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <uc1:ConsultarAlumnosUC ID="ConsultarAlumnosUC1" runat="server" />
    </div>
</asp:Content>
