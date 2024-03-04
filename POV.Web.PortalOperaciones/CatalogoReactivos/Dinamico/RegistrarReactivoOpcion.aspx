<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarReactivoOpcion.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoReactivos.Dinamico.RegistrarReactivoOpcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <style type="text/css">
        .elemento_oculto {
            display: none;
            width: 1px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            InicializarValidadorFileUpload(null, null);

        }

        
        function ValidateFields() {


            var tipoPresentacion = $("#<%= ddlTipoPresentacionPregunta.ClientID %>").val();
            var inputImagenPregunta = $("#<%=hdnImagenPregunta.ClientID %>");
            if (tipoPresentacion != 1) {
                inputImagenPregunta.addClass("required");
            } else {
                inputImagenPregunta.removeClass("required");
            }


            var tipoPuntaje = $("#<%= ddlTipoPuntaje.ClientID %>").val();
            var txtPuntajePregunta = $("#<%=txtPuntajeUnico.ClientID %>");
            if (tipoPuntaje == 1) {
                txtPuntajePregunta.addClass("required");
            } else {
                txtPuntajePregunta.removeClass("required");
            }

            //campo puntaje
            var tipoMetodo = $("#<%= hdnTipoMetodoCalificacion.ClientID%>").val();

            $('input[name$="txtOpcionesError"]').each(function (index) {
                $(this).removeClass("required");
            });

            if (tipoMetodo == 0) { //puntos 
                // al menos una correcta
                var countCorrectas = 0;

                $('input[name$="chkEsCorrecta"]').each(function (index) {
                    var chkID = $(this).attr('id');
                    var check = !$("#" + chkID + ":checked").val() ? false : true;

                    if (check)
                        countCorrectas++;
                });
                if (countCorrectas < 1) {
                    $('input[name$="txtOpcionesError"]').each(function (index) {
                        $(this).addClass("required");
                    });
                }


                $('select[name$="ddlClasificador"]').each(function (index) {
                    $(this).removeClass("required");
                });

                // si es tipopuntaje por opcion validar que tenga puntaje requerido
                if (tipoPuntaje == 2) {
                    $('input[name$="txtPuntaje"]').each(function (index) {
                        $(this).addClass("required");
                    });
                } else {
                    $('input[name$="txtPuntaje"]').each(function (index) {
                        $(this).removeClass("required");
                    });
                }



            } else if (tipoMetodo == 1) { //aciertos
                // al menos una correcta
                var countCorrectas = 0;

                $('input[name$="chkEsCorrecta"]').each(function (index) {
                    var chkID = $(this).attr('id');
                    var check = !$("#" + chkID + ":checked").val() ? false : true;

                    if (check)
                        countCorrectas++;
                });
                if (countCorrectas < 1) {
                    $('input[name$="txtOpcionesError"]').each(function (index) {
                        $(this).addClass("required");
                    });
                }

                // puntaje no requerido
                $('select[name$="ddlClasificador"]').each(function (index) {
                    $(this).removeClass("required");
                });
                $('input[name$="txtPuntaje"]').each(function (index) {
                    $(this).removeClass("required");
                });
            } else if (tipoMetodo == 2) { //clasificacion
                //clasificador requerido
                //puntaje opcion requerido

                $('select[name$="ddlClasificador"]').each(function (index) {
                    $(this).addClass("required");
                });
                $('input[name$="txtPuntaje"]').each(function (index) {
                    $(this).addClass("required");
                });
            } else if (tipoMetodo == 3) {//seleccion
                //clasificador requerido
                $('select[name$="ddlClasificador"]').each(function (index) {
                    $(this).addClass("required");
                });
                $('input[name$="txtPuntaje"]').each(function (index) {
                    $(this).removeClass("required");
                });
            }
            //campo imagen de opcion
            var tipoPresentacionOpcion = $("#<%= ddlPresentacionOpc.ClientID%>").val();
            if (tipoPresentacionOpcion == 0) {
                $('input[name$="hdnImagen"]').each(function (index) {

                    $(this).removeClass("required");
                });
            } else if (tipoPresentacionOpcion == 1) {
                $('input[name$="hdnImagen"]').each(function (index) {

                    $(this).addClass("required");
                });
            }

            $("#frmMain").validate().settings.ignore = "";
        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Registrar
        reactivo</h3>
    <div class="ui-widget-content table-responsive" style="padding: 5px">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblClave" runat="server" Text="Clave" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClave" runat="server" CssClass="required" MaxLength="30"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="required" MaxLength="100"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="Pregunta texto abierto" CssClass="label"></asp:Label>
                    <asp:CheckBox ID="cbPreguntaAbierta" runat="server" TextAlign="Left" AutoPostBack="True" OnCheckedChanged="cbPreguntaAbierta_CheckedChanged" />
                </td>
                <td>
                </td>
            </tr>
           <tr>
                <td class="td-label">
                    <asp:Label ID="lblModelo" runat="server" Text="Modelo de prueba" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlModelo" runat="server" OnSelectedIndexChanged="ddlModelo_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="required">
                    </asp:DropDownList>
                    <asp:Label ID="lblNombreMetodo" runat="server" Text="Método de calificación: "></asp:Label>
                    <asp:Label ID="lblMetodoCalificacion" runat="server" Text="SELECCIONE UN MODELO" Font-Size="11px"></asp:Label>
                    <asp:HiddenField ID="hdnTipoMetodoCalificacion" runat="server" Value="-1" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblTipoPresentacionPregunta" runat="server" Text="Presentación pregunta" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoPresentacionPregunta" runat="server">
                        <asp:ListItem Text="Texto" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Imagen" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Texto e imagen" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="td-label-texarea">
                    <asp:Label ID="lblPregunta" runat="server" Text="Texto de pregunta" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPregunta" runat="server" TextMode="MultiLine" Columns="40" Rows="5" CssClass="required" MaxLength="1000"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr id="rowClasificadores" runat="server" visible="false">
                <td class="td-label">
                    <asp:Label ID="lblClasificadorPreguntaAbierta" runat="server" Text="Clasificador" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlClasificadorPreguntaAbierta" runat="server">
                        <asp:ListItem Text="SELECCIONE UN MODELO..." Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <div id="divReactivoOpcionMultiple" runat="server">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tr>
                            <td class="td-label">
                                <asp:Label ID="lblImagenPregunta" runat="server" Text="Imagen de pregunta" CssClass="label"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="fileUploadImage" runat="server" CssClass="fileup"></asp:FileUpload>
                                <asp:Button ID="btnUpload" runat="server" Text="Subir imagen" OnClick="btnUpload_Click"
                                    CssClass="elemento_oculto" OnClientClick="CancelValidate()" />
                                <asp:TextBox ID="hdnImagenPregunta" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                            </td>
                            <td align="center">
                                <label>Vista previa:</label><br />
                                <asp:Image ID="img" runat="server" Width="100" Height="100" ImageAlign="Middle" BackColor="#ffffff" /><br />
                                <br />
                                <asp:Button ID="btnEliminarImagen" runat="server" CssClass="btn-cancel" Text="Eliminar imagen" OnClick="btnEliminarImgPregunta_OnClick" />
                            </td>
                        </tr>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpload" />
                    </Triggers>
                </asp:UpdatePanel>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblPuntaje" runat="server" Text="Puntaje" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updPanelTipoPuntaje" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoPuntaje" runat="server" OnSelectedIndexChanged="ddlTipoPuntaje_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Sin puntaje" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Puntaje por opción" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Único" Value="1"></asp:ListItem>
                                </asp:DropDownList>
            

                                <label>Puntaje único:</label>
                                <asp:TextBox ID="txtPuntajeUnico" runat="server" Width="100" Enabled="false" CssClass="number" MaxLength="14"></asp:TextBox>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblTipoOpciones" runat="server" Text="Tipo selección" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoSeleccion" runat="server">
                            <asp:ListItem Text="Múltiple" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Única" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblPresentacionOpc" runat="server" Text="Presentación de las opciones" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPresentacionOpc" runat="server">
                            <asp:ListItem Text="Texto" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Imagen" Value="1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                <td class="td-label">
                    <asp:Label ID="lblGrupo" runat="server" Text="Grupo" CssClass="label"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGrupo" runat="server" Width="100" CssClass="number" MaxLength="3" ></asp:TextBox> 
                    <asp:Label ID="lbltext" runat="server" Text="**Solo aplica para prueba Kuder y Allport*"></asp:Label>
                </td>
            </tr>
            </div>
        </table>
        <div id="divOpciones" runat="server">
            <div class=" ui-widget-content" style="padding: 20px;">
                <h1 style="border-bottom: 1px solid #bbb;">Opciones de respuesta:</h1>

                <asp:UpdatePanel ID="updPanelDDLClasificador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <br />
                        <asp:Repeater ID="rptOpciones" runat="server" OnItemDataBound="RptOpciones_DataBound"
                            OnItemCommand="RptOpciones_ItemCommand">
                            <ItemTemplate>
                                <asp:Label ID="op" runat="server" style="visibility:hidden" >opción</asp:Label>
                                <div style="border-bottom: 1px solid #bbb;">
                                    <div style="float: right; height: 30px;">
                                        <asp:Button ID="btnDeleteOpcion" runat="server" Text="Eliminar opción" CommandArgument="deleteOpcion" CommandName="deleteOpcion" CssClass="btn-cancel" />
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTextoOpc" runat="server" Text="Texto opción" CssClass="label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTextoOpcion" runat="server" TextMode="MultiLine" Rows="5" Columns="40" CssClass="required texto-opcion" MaxLength="1000"> </asp:TextBox>
                                            </td>
                                            <td style="padding:0px 0px 0px 40px">
                                                <div class="checkbox">                                                
                                                    <asp:CheckBox ID="chkEsCorrecta" Checked="false" runat="server" Text="¿Es Correcta?" Visible="true" />                                                
                                                    <asp:TextBox ID="txtOpcionesError" Text="" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                                </div>
                                                <div class="checkbox">
                                                    <asp:CheckBox ID="chkEsInteres" Checked="false" runat="server" Text="¿Es Interés?" Visible="true" />
                                                    <asp:TextBox ID="txtInteresError" Text="" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPresentacionOpc" runat="server" Text="Clasificador" CssClass="label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlClasificador" runat="server">
                                                    <asp:ListItem Text="SELECCIONE UN MODELO..." Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPuntaje" runat="server" Text="Puntaje" CssClass="label"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPuntaje" runat="server" CssClass="number" MaxLength="14">0</asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblImagenOpc" runat="server" Text="Imagen de opción" CssClass="label"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:FileUpload ID="fileUploadImageOpc" runat="server" CssClass="fileup"></asp:FileUpload>
                                                        <asp:Button ID="btnUploadOpc" runat="server" Text="Subir imagen" CommandArgument="uploadimg"
                                                            CommandName="uploadimg" CssClass="elemento_oculto" OnClientClick="CancelValidate()" />
                                                        <asp:TextBox ID="hdnImagen" runat="server" CssClass="elemento_oculto"></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <label>Vista previa:</label><br />
                                                        <asp:Image ID="imgOpc" runat="server" Height="100" ImageAlign="Middle" BackColor="#ffffff" Width="100" /><br />
                                                        <br />
                                                        <asp:Button ID="btnEliminarImagenOpc" runat="server" CssClass="btn-cancel" Text="Eliminar imagen" CommandArgument="deleteImagen" CommandName="deleteImagen" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUploadOpc" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hdnOpcionID" runat="server" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div style="padding: 20px 50px;">
                            <h4 style="margin: 10px 0;">
                                <asp:Button ID="btnAgregarNuevaOpcion" runat="server" OnClick="BtnAgregarOpcion_OnClick"
                                    Text="Agregar opción" CssClass="btn-green" OnClientClick="" />
                            </h4>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ddlModelo" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />
        <table>
            <tr>
                <td class="td-label"></td>
                <td class="label">
                    <asp:Button ID="btnGuardar" runat="server" CssClass="btn-green" Text="Guardar" OnClick="BtnGuardar_OnClick" OnClientClick="ValidateFields()" />
                </td>
                <td class="label">
                    <asp:Button ID="btnCancelar" runat="server" CssClass="btn-cancel" Text="Cancelar" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()" />
                </td>
                <td class="td-label"></td>
            </tr>
        </table>       
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>
