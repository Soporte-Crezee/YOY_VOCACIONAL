<%@ Page Title="YOY - NOTIFICACIONES" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="NotificacionesSocial.aspx.cs" Inherits="POV.Web.PortalSocial.Social.NotificacionesSocial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var current_page = 1;

        var ESTATUS_NUEVO = $("");
        var page_size = 20;
        $(document).ready(initPage);

        function initPage() {
            loadNotificaciones();

        }
    </script>

    <script id="notificacionTmpl" type="text/x-jquery-tmpl">

        <div class="notificacionSocial">
            <div id="${notificacionid}" class="notification_item ${$item.getCss(estatus)}">
                <div id="content_notification">
                    <div id="content_izq_notification">
                        <img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${emisorid}" class="user_image_top" alt="imagen" />
                        {{if renderemisorlink}}
            <a class="link_blue" href="../Social/ViewMuro.aspx?u=${emisorid}">${emisorname}</a>
                        {{else}}
            <label>${emisorname}</label>
                        {{/if}}
        <label>${textonotificacion}</label>
                        <a class="link_blue" href="${url}?n=${notificacionid}">${urllabel}</a>
                        {{if textonotificable.length > 0}}
            <label>: "${textonotificable}"</label>
                        {{/if}}
        <label class="dateformat">- ${fecharegistro}</label>

                    </div>
                    <div id="content_der_notification">
                        <button id="btn-del-${notificacionid}" style="vertical-align: middle; font-size: .6em" type="button" onclick="javascript:deleteNotificacion('${notificacionid}');">Eliminar notificaci&oacute;n</button>
                    </div>
                </div>
            </div>
        </div>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div class="bodyadaptable">
        <div class="panel panel-default">
            <div class="panel-heading">
                Tus notificaciones
            </div>
            <div class="panel-body">
                <div id="notificacion_container" class="row">
                    <div id="notificaciones_stream" class="">
                    </div>
                    <div id="more" class="col-md-12 col-xs-12">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
