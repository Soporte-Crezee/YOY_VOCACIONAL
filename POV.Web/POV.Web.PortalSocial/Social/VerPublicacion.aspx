<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="VerPublicacion.aspx.cs" Inherits="POV.Web.PortalSocial.Social.VerPublicacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.publicaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.publicaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reporteabuso.js" type="text/javascript"></script>

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

        .comments ul {
            margin-left: 65px;
        }

        @media screen and (max-width:768px) {
            .comments ul {
                margin-left: 0px;
            }
            /*.tBienvenidaLabel {
                font-size:20px!important
            }*/
            .tBienvenida {
                padding:8px!important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div>
        <div class="panel panel-dafault">
            <div class="panel-heading">
                    <asp:Label ID="lblTipo" CssClass="tBienvenidaLabel" runat="server"></asp:Label>
                    <asp:Label ID="lblNombreMuroPublicacion" CssClass="tBienvenidaLabel" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblNombreGrupo" CssClass="tBienvenidaLabel" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="pane-body">
                <div id="PublicacionStream">
        </div>
            </div>
        </div>


        
    </div>
    <div id="dialog-people-likes" title="Personas que le ha gustado">
        <div id="people-stream">
        </div>
    </div>
    <asp:Panel ID="pnlSeleccionContenido" runat="server" Visible="false">
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

    <asp:HiddenField ID="hdnPublicacionID" runat="server" />
    <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
    <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
    <asp:HiddenField ID="hdnDefaultUrl" runat="server" />
    <asp:HiddenField ID="hdnFuente" runat="server" Value="A" />
    <script type="text/javascript">
        var publicacionID = $("#<%=hdnPublicacionID.ClientID%>");
        var pubs_current = 1;

        var MURO = 1;
        var INICIO = 0;
        var VIEW_PUB = 2;
        var place = VIEW_PUB;
        var usrse = $("#<%= hdnSessionUsuarioSocialID.ClientID%>");
        var TIPO_PUBLICACION_TEXTO = $("#<%=hdnTipoPublicacionTexto.ClientID%>");
        var TIPO_PUBLICACION_REACTIVO = $("#<%=hdnTipoPublicacionSuscripcionReactivo.ClientID%>");
        var defaulturl = $("#<%=hdnDefaultUrl.ClientID%>");
        var TIPO_FUENTE_PUBLICACION = $("#<%=hdnFuente.ClientID%>");
        var myApiCompartir;
        $(function () {

            if (TIPO_FUENTE_PUBLICACION.val() == "D") {
                myApiCompartir = new CompartirApi({ folderWcf: '<% =Page.ResolveClientUrl("~/wcf")%>' });
                myApiCompartir.initApi();
            }

        });
    </script>
</asp:Content>
