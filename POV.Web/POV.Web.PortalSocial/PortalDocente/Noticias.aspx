<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/PortalDocente/PortalDocente.master" AutoEventWireup="true" CodeBehind="Noticias.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Noticias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.publicaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reporteabuso.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.publicaciones.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.compartir.contenido.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reactivos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.contenidos.js" type="text/javascript"></script>

    <style type="text/css">
        .pnl-insertar {
            margin: 8px 5px;
        }

        .pnl-contenido-agregado {
            padding: 0 6px;
            margin: 8px 5px;
            font-style: italic;
            line-height: 36px;
        }

        table.DDGridView th {
            background-color: #eee;
            border: 1px solid #ccc;
            height: 20px;
            padding: 5px;
        }

        table.DDGridView td {
            padding: 5px;
            border: 1px solid #ccc;
        }

        .dateformat, .boton_link > input[type="button"],
        .item_pub_content, .user_pub.link_blue, a.link_blue {
            font-size: 18px !important;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div class="bodyadaptable">
        <div id="info_alumno" class="titulo_marco_general">
            <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
            Muro
            
            <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
        </div>
        <div>
            <!-- publicaciones muro -->
            <div>
                <div id="PublicacionStream">
                </div>
                <div id="more">
                </div>
            </div>
            <asp:HiddenField ID="hdnSocialHubID" runat="server" />
            <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
            <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
            <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
            <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
            <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
            <asp:HiddenField ID="hdnFuente" runat="server" Value="D" />
            <script type="text/javascript">
                var hub = $("#<%=hdnSocialHubID.ClientID%>");
                var usr = $("#<%= hdnUsuarioSocialID.ClientID%>");
                var curpage = 1;

                var INICIO = 0;
                var MURO = 1;
                var VIEW_PUB = 2;
                var place = INICIO;

                var hubse = $("#<%=hdnSessionSocialHubID.ClientID%>");
            var usrse = $("#<%= hdnSessionUsuarioSocialID.ClientID%>");

                var TIPO_PUBLICACION_TEXTO = $("#<%=hdnTipoPublicacionTexto.ClientID%>");
                var TIPO_PUBLICACION_REACTIVO = $("#<%=hdnTipoPublicacionSuscripcionReactivo.ClientID%>");
                var TIPO_FUENTE_PUBLICACION = $("#<%=hdnFuente.ClientID%>");

                var myApiCompartir;
                $(function () {
                    myApiCompartir = new CompartirApi({ folderWcf: '<% =Page.ResolveClientUrl("~/wcf")%>' });
                myApiCompartir.initApi();
            });
            </script>
        </div>
        <div id="dialog-people-likes" title="Personas que le ha gustado">
            <div id="people-stream">
            </div>
        </div>

        <asp:Panel ID="pnlSeleccionContenido" runat="server">
            <input id="hdn-current-page-compartir" type="hidden" value="1" style="display: none; visibility: hidden;" />
            <!-- dialogo compartir reactivo -->
            <div id="dialog-seleccionar-reactivo" title="Compartir reactivo" class="dialog-compartir-contenido" style="display: none;">
                <div class="titulo_marco_general">
                    Nueva b&uacute;squeda
                </div>
                <div class="ui-widget-content" style="padding: 10px 40px;">
                    <label>Nombre: </label>
                    <input id="txt-nombre-reactivo" type="text" class="input_text_general" />
                    <label>Área: </label>
                    <select id="ddl-area-reactivo" class="input_text_general"></select>
                    <button id="btn-buscar-reactivo-pub" type="button" class="button_clip_39215E" onclick="myApiCompartir.consultarReactivosCompartir();">Buscar</button>
                </div>
                <br />
                <div class="titulo_marco_general">
                    Reactivos
                </div>
                <ul id="pnl-resultados-compartir-reactivos" class="ui-widget-content">
                </ul>
                <div id="pnl-more-resultados-compartir-reactivos" class="more">
                    Reactivos encontrados
                </div>
            </div>

            <!-- dialogo compartir juego -->
            <div id="dialog-seleccionar-juego" title="Compartir juego" class="dialog-compartir-contenido" style="display: none;">
                <div class="titulo_marco_general">
                    Nueva b&uacute;squeda
                </div>
                <div class="ui-widget-content" style="padding: 10px 40px;">
                    <label>Nombre: </label>
                    <input id="txt-compartir-nombre-juego" type="text" class="input_text_general" />
                    <label>Área: </label>
                    <select id="ddl-compartir-area-juego" class="input_text_general"></select>
                    <button type="button" class="button_clip_39215E" onclick="myApiCompartir.consultarJuegosCompartir();">Buscar</button>
                </div>
                <br />
                <div class="titulo_marco_general">
                    Juegos
                </div>
                <ul id="pnl-resultados-compartir-juegos" class="ui-widget-content">
                </ul>
                <div id="pnl-more-resultados-compartir-juegos" class="more">
                    Juegos encontrados
                </div>
            </div>
            <div id="dialog-seleccionar-contenido" title="Compartir contenido digital" class="dialog-compartir-contenido" style="display: none;">
                <div class="titulo_marco_general">
                    Nueva b&uacute;squeda
                </div>
                <div class="ui-widget-content" style="padding: 10px 40px;">
                    <label>Nueva b&uacute;squeda: </label>
                    <input id="txt-compartir-nombre-contenido" type="text" class="input_text_general" />
                    <button type="button" class="button_clip_39215E" onclick="myApiCompartir.consultarContenidosDigitales();">Buscar</button>
                </div>
                <br />
                <div class="titulo_marco_general">
                    Contenidos digitales
                </div>
                <ul id="pnl-resultados-compartir-contenidos" class="ui-widget-content">
                </ul>
                <div id="pnl-more-resultados-compartir-contenidos" class="more">
                    Contenidos encontrados
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
