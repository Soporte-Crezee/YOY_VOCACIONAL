<%@ Page MasterPageFile="~/Errores.Master" Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="POV.Web.PortalOperaciones.Error" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="page col-xs-12">
        <div class="page">
            <div class="pageError">
                <div class="imgError">
                    <asp:Image ID="ImgError" runat="server" CssClass="imgError" ImageUrl="~/images/SOV_imgError.png" />
                </div>
                <div class="contenidoError">
                    <p class="contenidoError">
                        ¡Ups! Ha ocurrido un error. Int&eacute;ntalo m&aacute;s tarde.
                        <asp:HiddenField runat="server" ID="hdnErrorType" />
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>