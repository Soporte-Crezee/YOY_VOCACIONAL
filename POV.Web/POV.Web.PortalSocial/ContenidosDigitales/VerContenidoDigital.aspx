<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="VerContenidoDigital.aspx.cs" Inherits="POV.Web.PortalSocial.ContenidosDigitales.VerContenidoDigital" %>

<%@ Import Namespace="POV.Web.PortalSocial.Actividades" %>
<%-- VerEjeTematico --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>Profesionalizacion.css" rel="stylesheet"
        type="text/css" />
    <%-- Plugin videos y audios--%>
    <script type="text/javascript" src="<% =Page.ResolveClientUrl("~/Scripts/")%>jQuery.jPlayer.2.4.0/jquery.jplayer.min.js"></script>
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>pink.flag/jplayer.pink.flag.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<% =Page.ResolveClientUrl("~/Scripts/")%>pdfobject.js"></script>
    <script type="text/javascript" src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.visor.contenido.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#accordion").accordion({
                collapsible: true, active: -1
            });

            var miContenido = $("#<%=hdnUrlContenido.ClientID%>").val();
            var tipoVisor = $("#<%=hdnTipoVisor.ClientID%>").val();
            var esDescarga = $("#<%=hdnEsDescargable.ClientID%>").val() == "true";
            var esRedireccion = $("#<%=hdnEsRedireccion.ClientID%>").val() == "true";
            var reproducir = $("#<%=hdnEsReproducible.ClientID%>").val() == "true";

            //VISORPDF, VISORMP4, VISORMP3, VISORYOUTUBE,VISORIMG
            var opciones = {
                srcPlantillas: '<% =Page.ResolveClientUrl("~/Scripts/VisoresTmpl/")%>',
                srcScripts: '<% =Page.ResolveClientUrl("~/Scripts/")%>',
                elementVisorId: 'contenido_digital',
                elementOpcionesId: 'opciones_contenido',
                urlContenido: miContenido,
                visorType: tipoVisor,
                esDescargable: esDescarga,
                esRedireccion: esRedireccion,
                esReproducible: reproducir
            };

            var oVisorApi = new VisorApi(opciones);

            oVisorApi.initVisor();

        });
        $(function () {
            $('div.panel-heading.clickable').click(function () {
                var $panel = $(this).parent();
                var $panelContainerBody = $panel.find('div.panel-collapse');
                var $panelBody = $panelContainerBody.find('.panel-body');
                if ($panelContainerBody.hasClass('in')) {
                    $panelContainerBody.removeClass('in');
                    $panelBody.slideUp();
                } else {
                    $panelContainerBody.addClass('in');
                    $panelBody.slideDown();
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="row">
                <div class="col-xs-1">
                    <div id="divImagenDocumento" class="icon_prof " style="height: 35px; width: 35px;" runat="server">
                    </div>
                </div>
                <div class="col-xs-11">
                    <asp:Label Text="Recurso didáctico:" runat="server"></asp:Label>
                    <asp:Label ID="lblNombreContenido" runat="server" CssClass="tBienvenidaLabel"></asp:Label>
                    <div style="float: right; margin: 0; padding: 0; width: 190px;">
                        <asp:HyperLink ID="lnkVerEje" runat="server" CssClass="nueva_busqueda_container nueva_busqueda_bg" NavigateUrl="~/ContenidosDigitales/BuscarContenidos.aspx">
                    Nueva b&uacute;squeda</asp:HyperLink>
                    </div>
                    <%-- Boton Regresar a Realizar Actividades  AN5 --%>
                    <div style="float: right; margin: 0; padding: 0; width: 190px;">
                        <asp:Button ID="lnkVerEjeA" runat="server" CssClass="btn-cancel" Text=" Finalizar tarea" OnClick="lnkVerEjeA_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="panel-group" id="acordionHabitos" role="tablist" aria-multiselectable="false">
                    <div class="panel panel-default">
                        <div class="panel-heading clickable" role="tab" id="headingHabitos">
                            <h3 class="panel-title ">
                                <label class="label-header" runat="server" id="lbResultadoGratis1" style="cursor: pointer">
                                    Descripci&oacute;n
                                </label>
                            </h3>
                        </div>
                        <div id="collapseHabitos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingHabitos">
                            <div class="panel-body">
                                <h4>Tipo de documento:
                        <asp:Label ID="lblTipoDocumento" runat="server"></asp:Label></h4>
                                <h4>Etiquetas:
                        <asp:Label ID="lblTags" runat="server"></asp:Label></h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="content_visor_contenido" class="prof_content_box_border">
                    <div id="contenido_digital">
                    </div>
                    <div id="opciones_contenido" style="margin: 15px 0;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="margin: 25px 10px" class="bodyadaptable">
        <div id="" style="margin: 5px 10px">
            <div style="overflow: hidden; margin: 0; padding: 0;">
                <div style="float: left; margin: 0; padding: 0;" class="tBienvenida">

                    <div class="clear">
                    </div>
                </div>
            </div>


            <div style="float: right; margin: 0; padding: 0; width: 190px;">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnUrlContenido" runat="server" />
    <asp:HiddenField ID="hdnEsDescargable" runat="server" />
    <asp:HiddenField ID="hdnEsReproducible" runat="server" />
    <asp:HiddenField ID="hdnEsRedireccion" runat="server" />
    <asp:HiddenField ID="hdnTipoVisor" runat="server" />
</asp:Content>
