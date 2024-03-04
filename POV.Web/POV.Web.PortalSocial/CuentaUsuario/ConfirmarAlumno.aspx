<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultSite.Master" AutoEventWireup="true"
    CodeBehind="ConfirmarAlumno.aspx.cs" Inherits="POV.Web.PortalSocial.CuentaUsuario.ConfirmarAlumno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.dialogs.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.usuarios.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.usuarios.js" type="text/javascript"></script>

    <script type="text/jscript">

        $(function () {
            $('#<%=txtDatosIncorrectos.ClientID %>').val('');
            $('#<%=txtDatosIncorrectos.ClientID %>').removeAttr('required');

            $("#btnClose").on("click", function () {
                $('#<%=txtDatosIncorrectos.ClientID %>').val('');
                $('#<%=txtDatosIncorrectos.ClientID %>').removeAttr('required');
            });

            $("#btnCancelar").button().on("click", function () {
                $('#<%=txtDatosIncorrectos.ClientID %>').val('');
                $('#<%=txtDatosIncorrectos.ClientID %>').attr('required', '');
            });
        });
    </script>

    <style type="text/css">
        .label_general {
            border-right: .25px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div class="modal fade dialog-md" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background: #33acfd">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-hidden="true" style="color: white"><span class="glyphicon glyphicon-remove"></span></button>
                    <h1 style="color: white">Mi informaci&oacute;n no es correcta</h1>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content">
                    <div class="">
                        <div class="row bs-wizard" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 5px 0px 0px 0px">
                                <div class="col-xs-12">
                                    <div class="col-xs-12 form-group">
                                        <div class="col-sm-12">
                                            <label style="padding: 0px 0px 10px 0px">¡Parece que alguno de tus datos está incorrecto! Por favor indícanos cuál y una vez realizado el cambio nosotros te notificaremos por correo</label>
                                            <asp:TextBox ID="txtDatosIncorrectos" runat="server" data-required-error="Dato requerido" placeholder="Ejemplo: Mi apellido paterno debe ser Garc&iacute;a" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                            <div class="help-block with-errors"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="col-md-6 pull-right">
                            <input type="button" value="Enviar" id="btnEnviarInfo" class="btn btn-green btn-md" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- <div>--%>
    <asp:TextBox ID="txtAntNombreUsuario" runat="server" Text="" Style="display: none"></asp:TextBox>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n Personal
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">CURP:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblCurp" Text=""></asp:Label>
                    </div>
                    <label class="col-sm-2 control-label">Nombre:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblNombre" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">Apellidos:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblApellidos" Text=""></asp:Label>
                    </div>
                    <label class="col-sm-2 control-label">Fecha de nacimiento:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblFechaNacimiento" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">Sexo:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblGenero" Text=""></asp:Label>
                    </div>
                    <label class="col-sm-2 control-label">Nombre de usuario:</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtNombreUsuario" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        <span class="usuario"></span><span class="usuario1"></span>
                        <asp:Label runat="server" ID="lblMensajeError" Text="" CssClass="rojo"></asp:Label>
                        <span style="display: block" class="nota_pequena">Solo se puede cambiar una vez.</span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">Correo electr&oacute;nico:</label>
                    <div class="col-sm-4">
                        <asp:Label runat="server" ID="lblCorreo" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default" id="SeccionPadres" runat="server">
        <div class="panel-heading">
            Padre(s)
        </div>
        <div class="panel-body">
            <div class="table table-responsive">
                <asp:GridView ID="grdTutores" AutoGenerateColumns="False" runat="server"
                    RowStyle-CssClass="td" HeaderStyle-CssClass="th" CssClass="table table-bordered table-striped"
                    OnRowDataBound="grdTutores_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Tutor.Nombre" HeaderText="Nombre(s)" />
                        <asp:BoundField DataField="Tutor.PrimerApellido" HeaderText="Apellido materno" />
                        <asp:BoundField DataField="Tutor.SegundoApellido" HeaderText="Apellido paterno" />
                        <asp:BoundField DataField="DescripcionParentesco" HeaderText="Parentesco" />
                        <asp:BoundField DataField="Tutor.Telefono" HeaderText="Telefono" />
                        <asp:BoundField DataField="Tutor.CorreoElectronico" HeaderText="Correo" />
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" />
                    <SelectedRowStyle Font-Bold="True" />

                </asp:GridView>
            </div>
        </div>
    </div>

    <div>
        <div style="display: none" class="titulo_marco_general">
            Informaci&oacute;n de tus escuelas y grupos
        </div>

        <br />
        <div>
            <asp:Button ID="btnAceptar" runat="server" Text="SÍ, mi información es correcta"
                OnClick="btnAceptar_Click" CssClass="btn btn-sm" />
            <button type="button" id="btnCancelar" data-toggle="modal" data-target=".dialog-md"
                class="btn-cancel">
                NO, mi información no es correcta</button>
        </div>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);
        prm.add_endRequest(endRequests);

        function loadControls(sender, args) {

            CoreUsuarios.ValidacionesUsuario.init({
                //buttons
                btnguardar: $('#<%=btnAceptar.ClientID %>'),
                buttons: $('.boton'),

                //identificadores
                uiusername: $('#<%=txtNombreUsuario.ClientID %>'),
                uiantusername: $('#<%=txtAntNombreUsuario.ClientID %>'),

                dialog: $('#dialogquestion'),
                textdialog: $('#dialogquestion').find('#dialogtext')
            });
        }

        var self = CoreUsuarios.ValidacionesUsuario;

        $('#<%=txtNombreUsuario.ClientID %>').keyup(function (event) {
            if (/^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val())) {
                if ($(this).val().length > 6) {
                    self.validateUsuario(1, true);//true = usuarios activos, false = usuarios inactivos
                } else {
                    $("#usuarioimg").remove();
                    $("#usuarioerror").remove();
                    var labelError =
                    '<label class="error" style="font-size:14px;padding-bottom:0px;margin-bottom:0px;display:block" id="usuarioerror" for="MainContent_txtNombreUsuario" generated="true">' +
                    'Por favor, escribe un nombre de usuario válido' +
                    '</label>';

                    $(this).addClass("novalido");
                    $("#usuarioerror").remove();
                    $(".usuario1").append(labelError);

                    $("#page_content_btnAceptar").attr("disable", true)
                }
                $(this).removeClass("novalido");
            } else {
                $("#usuarioimg").remove();

                var labelError =
                    '<label class="error" style="font-size:14px;padding-bottom:0px;margin-bottom:0px;display:block" id="usuarioerror" for="MainContent_txtNombreUsuario" generated="true">' +
                    'Por favor, escribe un nombre de usuario válido' +
                    '</label>';

                $(this).addClass("novalido");
                $("#usuarioerror").remove();
                $(".usuario1").append(labelError);

                $("#page_content_btnAceptar").attr("disable", true)

                event.preventDefault();
            }
        });

        function userValid() {
            var user = $("#<%=txtNombreUsuario.ClientID%>").val();
            if (user.length < 6) {
                if ($("#usuarioimg").length > 0) {
                    $("#usuarioimg").remove();
                }
                if ($("#usuarioimgError").length > 0) {
                    $("#usuarioimgError").remove();
                }
            }
            else {
                self.validateUsuario(1, true);//true = usuarios activos, false = usuarios inactivos
            }
        }

        var rules = {
            '<%=txtNombreUsuario.UniqueID %>': { required: true, maxlength: 50, minlength: 6 }
        };

            jQuery.extend(jQuery.validator.messages, {
                required: jQuery.validator.format("Este campo es obligatorio")
            });

            $("#btnEnviarInfo").click(function () {
                var txtDatos = $("#<%=txtDatosIncorrectos.ClientID%>");
                if (txtDatos.val().trim().length > 0){
                    __doPostBack('BtnEnviar_Click', '');
                }                    
                else {
                    txtDatos.val('');
                    txtDatos.focus();
                }
            });

            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                },
                rules: rules
            };

            $('#form1').validator(validations).on('submit', function (e) {

                var errores = '';

                if ($("#<%=txtNombreUsuario.ClientID %>").hasClass("novalido")) {
                    errores += "nombre de usuario no válido";
                }

                if (errores.length <= 0) {
                    return true;
                }
                else {
                    setTimeout(function () {
                        showMessage("Los siguientes datos no se encuentran disponibles para su uso: " + errores + ".");
                    }, 500);



                    return false;
                }
            });

            function endRequests(sender, args) {

            }
    </script>
</asp:Content>
