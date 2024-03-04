<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarContenidos.aspx.cs" Inherits="POV.Web.PortalSocial.ContenidosDigitales.BuscarContenidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>Profesionalizacion.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.contenidos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.contenidos.busqueda.js"
        type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.popup.ayuda.Sistema.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        var api = new ContenidosApi();
        var controlsViewState = {
            txtContenido: "#hdnContenidoIn",
            hdnPushButton: "#hdnPushButtonAction",
            hdnCurrentPage: "#hdnCurrentPage",
            hdnFocusElement: "#hdnFocusElement"
        };
        var oAyudaApi;

        $(document).ready(initPage);

        function initPage() {
            $("#NombreContenidoIn").focus();
            oAyudaApi = new AyudaApi({
                popup_container_id: 'popupAyudaContainer',
                url_contenido_popup: '../Files/Ayuda/AyudaBusquedaProfesionalizacion.htm'
            });
            oAyudaApi.initPopup();
            var returnValue = $(controlsViewState.hdnPushButton).val();
            var lastCurrentPage = $(controlsViewState.hdnCurrentPage).val();
            
            if (returnValue == "true") {
                var txtSearch = $(controlsViewState.txtContenido).val();
                $("#NombreContenidoIn").val(txtSearch);
                buscar();
                if (lastCurrentPage > 2) {
                    $(controlsViewState.hdnFocusElement).focus();
                }
            }
        }

        function buscar() {
            currentPage = 1;
            nombreContenido = $("#NombreContenidoIn").val();
            $(controlsViewState.txtContenido).val(nombreContenido);
            $(controlsViewState.hdnPushButton).val("true");

            buscarContenidos();
        }
    </script>
    <script id="contenidoTmpl" type="text/x-jquery-tmpl">
    <li class="item_pub  prof_font_size_12">
        <div class="prof_contenido_cols_container">
            <div class="prof_contenido_cols_left">
                <div class="element_height_28">
                    <label class="prof_text_size_12_bold">
                        <a href="../ContenidosDigitales/VerContenidoDigital.aspx?src=eje&ejeid=${ejetematicoid}&contid=${contenidoid}" class="link_blue">${nombrecontenido}</a>
                    </label>
                </div>
                <div class="element_height_28">
                    <div class="prof_contenido_cols_inside_container">
                        <div class="prof_contenido_cols_inside_left"><label class="prof_bold">Tema:</label> ${nombresituacion}</div>
                        <div class="prof_contenido_cols_inside_right"><label class="prof_bold">Etiquetas:</label><i> ${etiquetas}</i></div>
                    </div>
                </div>
            </div>
            <div class="prof_contenido_cols_right">
                <a href="../ContenidosDigitales/VerContenidoDigital.aspx?src=eje&ejeid=${ejetematicoid}&contid=${contenidoid}" style="color:#333">
                <div style="height:56px">
                    <div class="icon_prof ${imagendocumento}" style="height:35px; width:35px;margin:5px auto;">
                        
                    </div>
                </div>
                <div style="height:28px;text-align:center;">${tipodocumento}</div>
                </a>
            </div>
        </div>
    </li>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div>
        <div class="div_prof_title_top">
            <h1>
                Buscar recursos didácticos</h1>
        </div>
        <div class="tmpl_prof_twocolumns_container">
            <div class="tmpl_prof_twocolumns_left">
                <div>
                    <div class="div_prof_menu_anchors">
                        <ul class="lista_horizontal">
                            <li  style="margin-bottom: -3px;">
                                <asp:HyperLink ID="lnkEjes" NavigateUrl="~/Profesionalizacion/BuscarEjes.aspx" runat="server"
                                    CssClass="link_blue">B&uacute;squeda general</asp:HyperLink>
                            </li>
                            <li style="margin-bottom: -3px;border-bottom: 3px solid #91268F;">Recursos did&aacute;cticos</li>
                        </ul>
                    </div>
                    <div class="clear">
                    </div>
                    <div>
                        <div style="padding-bottom: 5px;">
                        <table class="container_busqueda_general ui-widget-content" style="padding:25px 0 25px 0">
                            <tr>
                                <td class="label_general">
                                   Nueva b&uacute;squeda
                                </td>
                                <td  style="width: 250px">
                                    <input type="text" id="NombreContenidoIn" class="input_text_general" tabindex="0" />
                                    <input type="hidden" id="hdnContenidoIn" />
                                    <input type="hidden" id="hdnPushButtonAction" />
                                    <input type="hidden" id="hdnCurrentPage" />
                                </td>
                                <td style="text-align: left;">
                                    <input type="submit" value="Buscar" onclick="javascript:buscar();return false;" class="button_clip_39215E" />
                                </td>

                            </tr>
                        </table>
                        </div>
                        <div class="div_prof_result_container">
                            <div class="div_prof_result_title">
                                <p>
                                    Recurso didáctico</p>
                            </div>
                            <div style="margin: 3px;">
                                <ul id="resultados_contenidos">
                                </ul>
                                <div id="more_contenidos" class="more ver_mas_container ver_mas_bg">
                                    Recursos didácticos encontrados
                                </div>
                                <input id="hdnFocusElement" type="hidden"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="tmpl_prof_twocolumns_right">
                <!-- menu lateral derecho-->
                <button type="button" class="button_clip_39215E " style="width:100%;font-size:14px;text-align:left" onclick="oAyudaApi.showPopup();">
	                <i class="icon icon_ayuda" style="margin: 0 15px"></i> 
	                <span>Ayuda</span>
                </button>
                
            </div>
        </div>
    </div>
</asp:Content>
