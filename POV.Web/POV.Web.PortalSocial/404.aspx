<%@ Page Title="" Language="C#" MasterPageFile="~/Errores.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="POV.Web.PortalSocial._404" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="page_content" runat="server">
    
    <div class="pageError">
    	<div class="imgError">
            <asp:Image ID="ImgError" runat="server" CssClass="imgError" ImageUrl="Images/imgError.jpg" />
    	</div>
        
        <div class="contenidoError">
             <p class="contenidoError">Recurso no encontrado !
                <asp:HiddenField runat="server" ID="hdnErrorType"/>
            </p>
            <p class="contenidoError">
              <a href="~/Default.aspx" runat="server" ID="hrefInicio">Inicio</a>
            </p>

        </div>
    </div>

    
</asp:Content>
