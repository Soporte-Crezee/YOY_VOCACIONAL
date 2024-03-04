<%@ Page Title="" Language="C#" MasterPageFile="~/Errores.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="POV.Web.PortalTutor.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/Styles/Talentos.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="page col-xs-12">
        <div class="page">
            <div class="pageError">
                <div class="">
                    <img alt="404" src="../Images/SOV_imgError.png" class="imgError img-responsive" />
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
