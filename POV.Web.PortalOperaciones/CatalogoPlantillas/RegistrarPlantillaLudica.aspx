<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrarPlantillaLudica.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoPlantillas.RegistrarPlantillaLudica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {

            $(".boton").button();
            InicializarValidadorFileUpload(null, null);

        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
    <style type="text/css">
        .elemento_oculto {
            display: none;
            width: 1px;
        }


        #pnlwork {
            width: 450px;
            height: 650px;
            margin: 0 auto;
            position: relative;
            background-image: url('../Images/transparente.png');
        }

        .icon {
            background-repeat: no-repeat;
            vertical-align: middle;
            padding-right: 3px;
            padding-left: 3px;
        }

        .icon_36 {
            background-repeat: no-repeat;
            height: 48px;
            width: 37px;
            background-color: transparent;
            /*background-image: url('Ico.reactivos.png');*/
            position: absolute;
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#<%=pnlwork.ClientID%>").click(function (e) {
                /*var p = $(this).position();
                alert(p.left);
                alert(p.top);*/
                var offsetLeft = $(this).css("left");
                var offsetTop = $(this).css("top");
                //alert( offsetLeft + " " + offsetTop);
                //alert( e.pageX + " " + e.pageY);

                var posX = $(this).offset().left,
	            posY = $(this).offset().top;
                var dato = (e.pageX - posX) + ',' + (e.pageY - posY);
                var valores = $("#txt-puntos").val();

                if (valores != "")
                    valores += "|" + dato;
                else
                    valores = dato;
                $("#txt-puntos").val(valores);
                $("#<%=PuntosA.ClientID%>").val(valores);
                pintar();
            });

        });

        function pintar() {
            var valores = $("#txt-puntos").val();
            var puntos = $("#<%=PuntosA.ClientID%>").val();
            var coords = valores.split("|");
            $("#<%=pnlwork.ClientID%>").empty();
            var cont = 400;
            for (var index = 0; index < coords.length; index++) {
                var dato = coords[index].split(",");
                if (index == 0)
                    var x = dato[0] - (dato[0] > 18 ? 18 : 0);
                else
                    var x = dato[0] - ((dato[0] > 18 ? 28 : 0)) / 2;
                var y = dato[1] - (dato[1] > 36 ? (18) : 0);

                var css = "left:" + x + "px;top:" + y + "px;z-index:" + cont + ";";
                var im = $("#<%= hiddenPosicion.ClientID %>").val();
                $("#<%=pnlwork.ClientID%>").append('<div class="icon_36" style="background-repeat: no-repeat;height: 48px;width: 37px;background-color: transparent;background-image: url(' + im + ');position: absolute;display: inline-block;' + css + '"></div>')

                cont++;
            }
        }

        function limpiar() {
            $("#txt-puntos").val("");
            $("#<%=PuntosA.ClientID%>").val("");
            $("#<%=pnlwork.ClientID%>").empty();
        }

    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InicializarValidadorFileUpload);
        function InicializarValidadorFileUpload(sender, args) {
            $(".fileup").each(function (index) {
                $(this).change(function () {

                    var val = $(this).val();

                    switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                        case 'jpg':
                        case 'gif':
                        case 'jpeg':
                        case 'png':
                        case 'bmp':
                            $(this).next().click();
                            break;
                        default:
                            $(this).val('');
                            $("#txtRedirect").val("");
                            $(hdnmessageinputid).val("El archivo seleccionado debe ser un archivo de imagen.");
                            $(hdnmessagetypeinputid).val('1');
                            mostrarError();
                            break;
                    }
                });

            });
        }
    </script>
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="BuscarPlantilla.aspx">Volver</asp:HyperLink>
        /Registrar &aacute;rea estandarizada</h3>
    <div class="main_div ui-widget-content" style="padding: 5px;">
        <h2>Informaci&oacute;n de la plantilla l&uacute;dica</h2>
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombrePlantilla" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombrePLantilla" runat="server" MaxLength="200"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblPredeterminado" runat="server" Text="¿Es predeterminado?"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEsPredeterminado" runat="server">
                        <asp:ListItem Value="" Text="Seleccionar" Selected="true"></asp:ListItem>
                        <asp:ListItem Value="true" Text="SI"></asp:ListItem>
                        <asp:ListItem Value="false" Text="NO"></asp:ListItem>
                    </asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel runat="server" ID="UpdFondo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label ID="lblImagenFondo" runat="server" Text="Imagen de fondo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fileUploadImageFondo" runat="server" CssClass="fileup"></asp:FileUpload>
                                        <asp:Button ID="btnUpload" runat="server" Text="Subir imagen" OnClick="btnUpload_Click"
                                            CssClass="elemento_oculto" OnClientClick="CancelValidate()" />
                                        <asp:TextBox ID="hdnImagenFondo" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <label>Vista previa:</label><br />
                                        <asp:Image ID="imgFondo" runat="server" Height="70" ImageAlign="Middle" BackColor="#ffffff" Width="50" /><br />
                                        <br />
                                        <asp:Button ID="btnEliminarImagenFondo" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarImagenFondo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" GroupingText=" Imágenes flechas desplazamiento ">
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdFlechaArriba" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblFlechaArriba" Text="Flecha arriba"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fileUploadFlechaArriba" runat="server" CssClass="fileup"></asp:FileUpload>
                                        <asp:Button ID="btnUploadFlechaArriba" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClick="btnUploadFlechaArriba_Click" OnClientClick="CancelValidate()" />
                                        <asp:TextBox ID="hdnFlechaArriba" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <label>Vista previa:</label><br />
                                        <asp:Image ID="imgFlechaArriba" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                        <br />
                                        <asp:Button ID="btnEliminarFlechaArriba" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarFlechaArriba_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUploadFlechaArriba" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdFlechaAbajo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblFlechaAbajo" Text="Flecha abajo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fileUploadFlechaAbajo" runat="server" CssClass="fileup"></asp:FileUpload>
                                        <asp:Button ID="btnUploadFlechaAbajo" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClick="btnUploadFlechaAbajo_Click" OnClientClick="CancelValidate()" />
                                        <asp:TextBox ID="hdnFlechaAbajo" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <label>Vista previa:</label><br />
                                        <asp:Image ID="imgFlechaAbajo" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                        <br />
                                        <asp:Button ID="btnEliminarFlechaAbajo" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarFlechaAbajo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUploadFlechaAbajo" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
       </asp:Panel>
        <asp:Panel ID="plImagenesActividades" runat="server" GroupingText=" Imágenes estado de las actividades ">
            <div style="float: left; width: 49%">
                <asp:UpdatePanel runat="server" ID="UpdActividades" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblPendiente" Text="Actividad Pendiente"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadActPendiente" runat="server" CssClass="fileup"></asp:FileUpload>
                                    <asp:Button ID="btnUploadPendiente" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClick="btnUploadPendiente_Click" OnClientClick="CancelValidate()" />
                                    <asp:TextBox ID="hdnActPendiente" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <label>Vista previa:</label><br />
                                    <asp:Image ID="imgPendiente" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                    <br />
                                    <asp:Button ID="btnEliminarPendiente" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarImagenPendiente_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadPendiente" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="UpdActividadNoDisponible" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblDisponible" Text="Actividad no disponible"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadActNoDisponible" runat="server" CssClass="fileup"></asp:FileUpload>
                                    <asp:Button ID="btnUploadActNoDisponible" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClientClick="CancelValidate()" OnClick="btnUploadActNoDisponible_Click" />
                                    <asp:TextBox ID="hdnNoDisponible" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <label>Vista previa:</label><br />
                                    <asp:Image ID="imgNoDisponible" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                    <br />
                                    <asp:Button ID="btnEliminarNoDisponible" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarImagenNoDisponible_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadActNoDisponible" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div style="float: right; width: 50%">
                <asp:UpdatePanel runat="server" ID="UpdActividadInic" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblIniciada" Text="Actividad iniciada"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadActIniciada" runat="server" CssClass="fileup"></asp:FileUpload>
                                    <asp:Button ID="btnUploadActIniciada" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClientClick="CancelValidate()" OnClick="btnUploadActIniciada_Click" />
                                    <asp:TextBox ID="hdnActIniciada" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <label>Vista previa:</label><br />
                                    <asp:Image ID="imgIniciada" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                    <br />
                                    <asp:Button ID="btnEliminarIniciada" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarImagenIniciada_Click" />
                                </td>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadActIniciada" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="UpdActividadFin" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblFinalizada" Text="Actividad finalizada"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadActFinalizada" runat="server" CssClass="fileup"></asp:FileUpload>
                                    <asp:Button ID="btnUploadActFinalizada" runat="server" Text="Subir imagen" CssClass="elemento_oculto" OnClientClick="CancelValidate()" OnClick="btnUploadActFinalizada_Click" />
                                    <asp:TextBox ID="hdnFinalizada" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <label>Vista previa:</label><br />
                                    <asp:Image ID="imgFinalizada" runat="server" ImageAlign="Middle" BackColor="#ffffff" Width="36px" Height="36px" /><br />
                                    <br />
                                    <asp:Button ID="btnEliminarFinalizada" runat="server" CssClass="boton" Text="Eliminar imagen" OnClick="btnEliminarImagenFinalizada_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUploadActFinalizada" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>

        <div>
            <asp:Panel ID="plPosicionActividad" runat="server" GroupingText="Posiciones de las actividades">
                <input id="txt-puntos" type="text" style="visibility: hidden; display: none" /><asp:TextBox ID="hiddenPosicion" runat="server" Style="visibility: hidden; display: none"></asp:TextBox>
                <asp:TextBox ID="PuntosA" runat="server" Style="visibility: hidden; display: none"></asp:TextBox>
                <div style="float: left; width: 49%">
                    <div id="pnlwork" runat="server"></div>
                </div>
                <div style="float: right; width: 49%">
                    <table>
                        <tr>
                            <td><b>Instrucciones:</b><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul type="disc">
                                    <li>Se requiere que previamente selecciones la imagen de fondo.</li>
                                    <li>Posteriormente selecciones la imagen de actividad pendiente.</li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>**Esto es con la finalidad que visualmente, tengas una idea de como se presentarán las actividades en la plantilla.</td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <button type="button" id="btn-pintar" onclick="pintar();" class="boton">Pintar</button>
                    <button type="button" id="btn-limpiar" onclick="limpiar();" class="boton">Limpiar</button>
                </div>

            </asp:Panel>
        </div>
        <div class="line"></div>
        <table>
            <tr>
                <td>
                    <div>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                        <asp:HyperLink ID="btnCancelar" CssClass="boton" runat="server" NavigateUrl="~/CatalogoPlantillas/BuscarPlantilla.aspx">Cancelar</asp:HyperLink>

                    </div>
                </td>
            </tr>
        </table>
    </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
        style="display: none;" />
</asp:Content>
