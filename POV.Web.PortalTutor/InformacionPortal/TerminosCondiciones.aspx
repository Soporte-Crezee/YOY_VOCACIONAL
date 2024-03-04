<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TerminosCondiciones.aspx.cs" Inherits="POV.Web.PortalSocial.InformacionPortal.TerminosCondiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       .parrafo{font-size: 1.25em; line-height: 1.25em; margin: 0; text-align: justify}
       .multiline {
           resize: none;
       }              
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div style="margin: 5px 10px">
        <h1 class="title_2 line_title">
         T&eacute;rminos y condiciones
        </h1>
        <div class="subline_title">
            <br/>
    <div class="ui-corner-all ui-widget-content" style="padding:10px;">
        <asp:TextBox TextMode="MultiLine" CssClass="multiline" ID="TxtContenidoTermino" Rows="20" runat="server" Width="100%"  ReadOnly="true"></asp:TextBox>
    </div>
        </div>
    </div>
</asp:Content>