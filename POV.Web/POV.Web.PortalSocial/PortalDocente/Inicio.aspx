<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/PortalDocente/PortalDocente.master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script type="text/javascript">
        var current_page = 1;

        var ESTATUS_NUEVO = $("");
        var page_size = 20;
        $(document).ready(initPage);

        function initPage() {
            loadNotificaciones();
        }
    </script>
    <style type="text/css">
        .dateformat, .boton_link > input[type="button"],
        .item_pub_content, .user_pub.link_blue, a.link_blue {
            font-size: 18px !important;
        }
    </style>
    <script id="notificacionTmpl" type="text/x-jquery-tmpl">
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-12">
                    <div class="notificacionSocial">
                        <li id="${notificacionid}" class="notification_item ${$item.getCss(estatus)}">
                            <div id="content_notification">
                                <div id="content_izq_notification">
                                    <img src="../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=${emisorid}" class="user_image_top" alt="imagen" />
                                    {{if renderemisorlink}}
                        <a class="link_blue" href="../Social/ViewMuro.aspx?u=${emisorid}">${emisorname}</a>
                                    {{else}}
                        <label>${emisorname}</label>
                                    {{/if}}
                    <label>${textonotificacion}</label>
                                    <a class="link_blue" href="../Social/${url}?n=${notificacionid}">${urllabel}</a>
                                    {{if textonotificable.length > 0}}
                        <label>: "${textonotificable}"</label>
                                    {{/if}}
                    <label class="dateformat">- ${fecharegistro}</label>

                                </div>
                                <div id="content_der_notification">
                                    <button id="btn-del-${notificacionid}" style="vertical-align: middle; font-size: .6em" type="button" onclick="javascript:deleteNotificacion('${notificacionid}');">Eliminar notificaci&oacute;n</button>
                                </div>
                            </div>
                        </li>
                    </div>
                </div>
            </div>
        </div>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">    
        <div id="info_perfil">
        </div>
        <div class="titulo_marco_general">
            <label class="titulo_label_general">Tus notificaciones</label>
        </div>
        <div class="subline_title"></div>

        <div id="notificacion_container">
            <ul id="notificaciones_stream" style="padding:0px">
            </ul>
            <div id="more">
            </div>
        </div>    
</asp:Content>
