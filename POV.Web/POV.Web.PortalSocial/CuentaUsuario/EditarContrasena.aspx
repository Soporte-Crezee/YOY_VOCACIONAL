﻿<%@ Page Title="YOY - EDITAR CONTRASEÑA" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarContrasena.aspx.cs" Inherits="POV.Web.PortalSocial.CuentaUsuario.EditarContrasena" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript" language="javascript"></script>
    <script src="../Scripts/messages_es.js" language="javascript" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        span {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            width: 100%;
        }

    </style>
    <script type="text/javascript">
        $(function () {
            $("#<%=ActualPassTxt.ClientID%>").focus();
            $("#form1").validate({
                rules: {

                    '<%=ActualPassTxt.UniqueID%>': {
                        required: true,
                        minlength: 6,
                        maxlength: 15
                    }, '<%=NuevaPassTxt.UniqueID%>': {
                        required: true,
                        minlength: 6,
                        maxlength: 15
                    },
                    '<%=RepNuevaPassTxt.UniqueID%>': {
                        required: true,
                        minlength: 6,
                        maxlength: 15,
                        equalTo: "#<%=NuevaPassTxt.ClientID%>"
                    }
                },
                messages: {

                    '<%=ActualPassTxt.UniqueID%>': {
                        required: "Contrase&ntilde;a requerida",
                        minlength: jQuery.format("M&iacute;nimo {0} carácteres"),
                        maxlength: jQuery.format("Máximo {0} carácteres")
                    }, '<%=NuevaPassTxt.UniqueID%>': {
                        required: "Contrase&ntilde;a requerida",
                        minlength: jQuery.format("M&iacute;nimo {0} carácteres"),
                        maxlength: jQuery.format("Máximo {0} carácteres")
                    },
                    '<%=RepNuevaPassTxt.UniqueID%>': {
                        required: "Repetir contrase&ntilde;a requerida",
                        minlength: jQuery.format("M&iacute;nimo {0} carácteres"),
                        maxlength: jQuery.format("Máximo {0} carácteres"),
                        equalTo: "La contrase&ntilde;a no coincide"
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <asp:Label ID="LblNombreUsuario" runat="server" Text="" CssClass="tBienvenidaLabel" Style="display:none;"></asp:Label>
    <ol class="breadcrumb">
        <li>
            Cambio de contrase&ntilde;a
        </li>
    </ol>
    <div class="">
        <asp:Label ID="ActualPassErrorTxt" runat="server" Text="" CssClass="error_label"></asp:Label>
        <asp:Label ID="RepNuevaPassErrorTxt" runat="server" Text="" CssClass="error_label"></asp:Label>
        <asp:Label ID="NuevaPassErrorTxt" runat="server" Text="" CssClass="error_label"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n de usuario
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label ID="lblPassActual" runat="server" Text="Contrase&ntilde;a actual *" CssClass="col-sm-5 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="ActualPassTxt" runat="server" TextMode="Password" CssClass="form-control" MaxLength="15" required=""></asp:TextBox>
                    </div>
                    <asp:Label ID="lblPassNueva" runat="server" Text="Contrase&ntilde;a nueva *" CssClass="col-sm-5 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="NuevaPassTxt" runat="server" TextMode="Password" CssClass="form-control" MaxLength="15" required=""></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPassNuevaRepetir" runat="server" Text="Repetir contrase&ntilde;a nueva *" CssClass="col-sm-5 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="RepNuevaPassTxt" runat="server" TextMode="Password" CssClass="form-control" MaxLength="15" required=""></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="text-center">
        <asp:Button ID="GuardarBtn" runat="server" Text="Guardar" OnClick="GuardarBtn_OnClick" CssClass="btn btn-green btn-md" />   
        <input id="CancelBtn" type="button" class="btn btn-cancel btn-md" value="Cancelar" onclick="window.location = '<%=Page.ResolveClientUrl("~/Default.aspx")%>    ';" />
    </div>
</asp:Content>
