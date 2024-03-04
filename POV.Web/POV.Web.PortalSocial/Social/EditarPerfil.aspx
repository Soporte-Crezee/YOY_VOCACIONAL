<%@ Page Title="YOY - ESTUDIANTE" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarPerfil.aspx.cs" Inherits="POV.Web.PortalSocial.Social.EditarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>muro.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .input_file_hidden {
            background: transparent;
            width: 100%;
            height: 29px;
            border: 0;
            opacity: 0;
            position: absolute;
            z-index: 5;
            top: 0;
            left: 2px;
            -moz-opacity: 0;
            filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);
            text-align: right;
            cursor: pointer;
        }

        .boton_seleccionar {
            float: right;
            vertical-align: middle;
            text-align: center;
        }

        #label_photo {
            font-size: 14pt;
            display: block;
            width: 280px;
        }

        .container_photo_select {
            width: 100%;
            position: relative;
            padding: 3px;
            height: 32px;
        }

        .ui-datepicker-year {
            color: #502e85;
        }

        .ui-datepicker-month {
            color: white;
        }

        .ui-datepicker .ui-datepicker-header {
            background-color: #05AED9;
            border-color: gray;
        }
    </style>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.notificaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.notificaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.dialogs.js" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/css/intlTelInput.css" rel="stylesheet" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/intlTelInput.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/moment-with-locales-v2.9.0.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap-datetimepicker-v4.15.35.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(initPage);

        $(document).ready(function () {
            $("#telefono").intlTelInput({
                nationalMode: false,
                formatOnDisplay: false,
                separateDialCode: true,
                utilsScript: "<%=Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/utils.js",
                preferredCountries: ["mx", "AR", "co", "ve", "cl"]
            });

            $("#telefonocasa").intlTelInput({
                nationalMode: false,
                formatOnDisplay: false,
                separateDialCode: true,
                utilsScript: "<%=Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/utils.js",
                preferredCountries: ["mx", "AR", "co", "ve", "cl"]
            });

            $("#telefonotutor").intlTelInput({
                nationalMode: false,
                formatOnDisplay: false,
                separateDialCode: true,
                utilsScript: "<%=Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/utils.js",
                preferredCountries: ["mx", "AR", "co", "ve", "cl"]
            });
        });

        function reloadUbicacion(item) {
            setTimeout(function () {
                switch (item) {
                    case 4:
                        validarSelect($("#telefono"), 'celphone');
                        break;
                    case 5:
                        validarSelect($("#telefonocasa"), 'homephone');
                        break;
                    default:
                        break;
                }
            }, 500);
        }

        function validarSelect(selector, idclass) {
            var message = '';
            var formato = false;
            if (idclass == 'celphone' || idclass == 'homephone') {
                if (selector.val().length < 10) {
                    message = "El número no debe ser menor a 10 dígitos";
                    formato = true;
                    $('#' + idclass).find('ul.list-unstyled>li').text(message);
                }
            } else {
                message = "Seleccione un elemento de la lista";
                formato = true;
            }


            var selectRequired =
            [
                '<div class="help-block with-errors ' + idclass + '">',
                '<ul class="list-unstyled">',
                '<li>' + message + '</li>',
                '</ul>',
                '</div>'
            ].join('');

            if (selector.val() != '' && selector.val() != undefined && !formato) {
                $('div.' + idclass).hide();
                if ($('#' + idclass).hasClass("has-error")) {
                    $('#' + idclass).removeClass("has-error has-danger");
                    $('div.' + idclass).hide();
                }
            }
            else {
                if (!$('#' + idclass).hasClass("has-error")) {
                    $('#' + idclass).addClass("has-error has-danger");
                    setTimeout(function () {
                        $('div.' + idclass).show();
                    }, 200);
                }
            }
        }

        function initPage() {

            var options = {
                'maxCharacterSize': 200,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 190,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=TxtFirma.ClientID%>').textareaCount(options);

            $("#btnCancelar").click(function () {
                window.location = "../PortalAlumno/Noticias.aspx";
            });

            $("#<%=txtCodigoTutor.ClientID %>").focus();
            setTimeout(function () {
                $("#<%=btnAceptarCodigo.ClientID%>").attr("disabled", true);
                if ($("#<%=txtCodigoTutor.ClientID%>").val() != '') {
                    validarDisponibilidad();
                }

                if ($("#<%=txtTutorTelefono.ClientID%>").length > 0) {
                    var telefono = $("#<%=txtTutorTelefono.ClientID%>").val();

                    if (telefono != undefined || telefono.length > 0) {
                        $("#telefonotutor").intlTelInput("setNumber", telefono);
                    }
                }

                if ($("#<%=TxtTelefono.ClientID%>").length > 0) {
                    var telefono = $("#<%=TxtTelefono.ClientID%>").val();

                    if (telefono != undefined || telefono.length > 0) {
                        $("#telefono").intlTelInput("setNumber", telefono);
                    }
                }

                if ($("#<%=TxtTelefonoCasa.ClientID%>").length > 0) {
                    var telefono = $("#<%=TxtTelefonoCasa.ClientID%>").val();

                    if (telefono != undefined || telefono.length > 0) {
                        $("#telefonocasa").intlTelInput("setNumber", telefono);
                    }
                }

            }, 500);



            validarDisponibilidad = function () {
                var disponible = $("#<%=txtCodigoTutor.ClientID%>").val().trim();

                if (disponible != '' || disponible.length > 0) {
                    $("#<%=btnAceptarCodigo.ClientID%>").removeAttr('disabled');
                }
                else {
                    $("#<%=btnAceptarCodigo.ClientID%>").attr("disabled", true);
                }
            }


            $("#<%=txtCodigoTutor.ClientID%>").change(function (e) {
                validarDisponibilidad();
            });

            $("#<%=txtCodigoTutor.ClientID%>").on('change keydown paste input', function () {
                validarDisponibilidad();
            });

            $('#target').click(function (evento) {
                $('#divTutor').fadeIn(2000);
            });


            $("#<%=txtSegundoApellido.ClientID%>").focus();
            $(".boton").button();

            $("#<%=filMyFile.ClientID%>").change(function (event) {

                var val = $(this).val();
                var tmppath = URL.createObjectURL(event.target.files[0]);

                switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                    case 'gif': case 'jpg': case 'bmp': case 'png': case 'jpeg':
                        $("#label_photo").text(val.substring(val.lastIndexOf('\\') + 1));
                        $("#txtRedirect").val("");
                        $(hdnmessageinputid).val("La imagen debe ser tuya y no debe ser ofensiva para los usuarios.");
                        $(hdnmessagetypeinputid).val('3');
                        mostrarInfo();
                        $("#<%=ImgUser.ClientID%>").fadeIn("fast").attr('src', tmppath);
                        $("#<%=ImgUser.ClientID%>").fadeIn("fast").attr('alt', val.substring(val.lastIndexOf('\\') + 1));
                        break;
                    default:
                        initMasterClient();
                        $(this).val('');
                        $("#txtRedirect").val("");
                        $(hdnmessageinputid).val("El archivo seleccionado debe ser una imagen.");
                        $(hdnmessagetypeinputid).val('1');
                        mostrarError();
                        $("#label_photo").text('Archivo incorrecto, selecciona otro');
                        $("#<%=ImgUser.ClientID%>").fadeIn("fast").attr('src', '');
                        $("#<%=ImgUser.ClientID%>").fadeIn("fast").attr('alt', 'El archivo seleccionado debe ser una imagen.');
                        break;
                }
            });

            $("#form1").submit(function (e) {
                $("#<%=TxtTelefono.ClientID%>").val($("#telefono").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                $("#<%=TxtTelefonoCasa.ClientID%>").val($("#telefonocasa").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));

                var errorTelefono = $("#telefono").intlTelInput("getValidationError");

                if (errorTelefono === intlTelInputUtils.validationError.IS_POSSIBLE) {
                    $("#<%=TxtTelefono.ClientID%>").val($("#telefono").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                } else {
                    if ($("#telefono").val().trim() == "")
                        $("#<%=TxtTelefono.ClientID%>").val('');
                }

                var error = $("#telefonotutor").intlTelInput("getValidationError");

                if (error === intlTelInputUtils.validationError.IS_POSSIBLE) {
                    $("#<%=txtTutorTelefono.ClientID%>").val($("#telefonotutor").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                } else {
                    $("#<%=txtTutorTelefono.ClientID%>").val('');
                }

                var errorCasa = $("#telefonocasa").intlTelInput("getValidationError");

                if (errorCasa === intlTelInputUtils.validationError.IS_POSSIBLE) {
                    $("#<%=TxtTelefonoCasa.ClientID%>").val($("#telefonocasa").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                } else {
                    if ($("#telefonocasa").val().trim() == "")
                    $("#<%=TxtTelefonoCasa.ClientID%>").val('');
                }


                var celular = $("#telefono");
                var telefono = $("#telefonocasa");
                var completo4 = (celular.val() != undefined && celular.val() != '');
                if (celular.val().length < 10)
                    completo4 = false;

                var completo5;
                if (telefono.val().length > 0) {
                    completo5 = (telefono.val() != undefined && telefono.val() != '');

                    if (telefono.val().length < 10)
                        completo5 = false;
                }

                var complete;
                if (telefono.val().length > 0)
                    complete = completo4 && completo5;
                else
                    complete = completo4;

                if (!complete) {
                    if (!completo4)
                        reloadUbicacion(4);
                    if (telefono.val().length > 0) {
                        if (!completo5)
                            reloadUbicacion(5);
                    }

                    //messageError("error");
                    e.preventDefault();
                }
                else {
                    // everything looks good!
                    return;
                }
            });

            $("#btnClose").on("click", function () {
                $('#<%=txtNombreTutor.ClientID %>').val('');
                $('#<%=txtNombreTutor.ClientID %>').removeAttr('required');
            });

            $("#btnValidarCodigo").on("click", function () {

            });

            $("#target").on("click", function () {

            });

        }


        function viewModal() {
            window.location = "#divDatosEscolaridad";
            setTimeout(function () {
                $("#btnValidarCodigo").click();
            }, 500);
        }

        function viewTutorDatos() {
            window.location = "#divTutor";
            setTimeout(function () {
                $("#target").click();
            }, 500);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div id="panel-container" class="">
        <div class="modal fade dialog-md" tabindex="-1"
            data-keyboard="false" data-backdrop="static"
            role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal">
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-xs-12 modal_titulo_marco_general center-block">Informaci&oacute;n del tutor</div>
                    </div>
                    <div class="modal-body container_busqueda_general ui-widget-content">
                        <div class="">
                            <div class="row bs-wizard" style="border-bottom: 0;">
                                <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                                    <div class="col-xs-12">
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Nombre tutor</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtTutorID" runat="server" Enabled="false" Visible="false"></asp:TextBox>
                                                <asp:TextBox ID="txtNombreTutor" runat="server" Enabled="false" MaxLength="180" CssClass="form-control" placeholder="No proporcionado"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-4 control-label">Dirección</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtDireccionTutor" runat="server" Enabled="false" MaxLength="50" CssClass="form-control" placeholder="No proporcionado"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-4 control-label">Correo electr&oacute;nico</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtCorreoTutor" runat="server" Enabled="false" MaxLength="100" CssClass="form-control" placeholder="No proporcionado"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-4 control-label">Parentesco</label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlParentescoTutor" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                                    <asp:ListItem Value="1">PADRE</asp:ListItem>
                                                    <asp:ListItem Value="2">MADRE</asp:ListItem>
                                                    <asp:ListItem Value="3">Familiar</asp:ListItem>
                                                </asp:DropDownList>
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
                                <asp:Button runat="server" ID="btnAceptar" CssClass="button_clip_39215E" Text="Aceptar" OnClick="btnAceptar_Click" />
                                <button type="button" class="btn-cancel" id="btnClose" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="">
            <ul class="breadcrumb">
                <li>Editar perfil</li>
            </ul>
            <asp:Label ID="LblNombreUsuario" runat="server" CssClass="tBienvenidaLabel" Text="" Style="display: none;"></asp:Label>
            <div class="col-xs-12 col-md-3">
                <div id="user_img" style="margin-top: 10px;" class="profile_img_marco_aspirante">
                    <asp:Image runat="server" ID="ImgUser" CssClass="profile_img_aspirante" AlternateText="Image" /><br />
                </div>
            </div>
            <div class="col-xs-12 col-md-9">
                <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Foto de perfil
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Foto de perfil (Max. 2 MB)*</label>
                                <div class="col-sm-6">
                                    <div id="container_photo" class="form-control input-md" style="padding: 2px 2px 1px 1px; min-height: 45px;">
                                        <span id="seleccionar" class="boton_seleccionar btn btn-green btn-md" style="height: 35px;">Seleccionar</span>
                                        <label id="label_photo" class="label_photo">Selecciona una imagen</label>
                                        <input id="filMyFile" type="file" runat="server" class="input_file_hidden btn btn-green btn-md" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Informaci&oacute;n de estudiante
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Nombre</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtNombre" MaxLength="80" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Primer apellido</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtPrimerApellido" MaxLength="50" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Segundo apellido</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtSegundoApellido" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Sexo</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="CbSexo" runat="server" CssClass="form-control" readonly="true" Enabled="false">
                                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="True">HOMBRE</asp:ListItem>
                                        <asp:ListItem Value="False">MUJER</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Fecha de nacimiento*</label>
                                <div class="col-sm-4">
                                    <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
                        Informaci&oacute;n de usuario
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Nombre de usuario</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtUsuario" MaxLength="50" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Correo electr&oacute;nico</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtEmail" MaxLength="100" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Tel&eacute;fono celular*</label>
                                <div class="col-sm-4" id="celphone">
                                    <input type="tel" name="TelefonoFull" id="telefono" maxlength="10" class="form-control" required="" />
                                    <asp:HiddenField runat="server" ID="TxtTelefono"></asp:HiddenField>
                                    <div class="help-block with-errors celphone" style="display: none;">
                                        <ul class="list-unstyled">
                                            <li>Dato requerido.</li>
                                        </ul>
                                    </div>
                                </div>
                                <label class="col-sm-2 control-label">Tel&eacute;fono casa</label>
                                <div class="col-sm-4" id="homephone">
                                    <input type="tel" name="TelefonoFull" id="telefonocasa" maxlength="10" class="form-control" />
                                    <asp:HiddenField ID="TxtTelefonoCasa" runat="server"></asp:HiddenField>
                                    <div class="help-block with-errors homephone" style="display: none;">
                                        <ul class="list-unstyled">
                                            <li>Dato requerido.</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Datos de escolaridad
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Escuela*</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtEscuela" MaxLength="250" runat="server" CssClass="form-control" required=""></asp:TextBox>
                                </div>
                                <label class="col-sm-2 control-label">Grado*</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList runat="server" ID="ddlNivelEstudio" CssClass="form-control" required="">
                                        <asp:ListItem Value="" Text="Seleccionar"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Semestre 1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Semestre 2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Semestre 3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Semestre 4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Semestre 5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Semestre 6"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Datos de orientaci&oacute;n educativa
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:TextBox ID="txtOrientador" runat="server" CssClass="text-center" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <div class="table-responsive" id="orientadorAsignado" runat="server">
                                    <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdOrientadores" CssClass="table table-bordered table-striped"
                                        RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                        <Columns>
                                            <asp:BoundField HeaderText="Orientador(es) asignado(s)" DataField="NombreCompletoOrientador">
                                                <HeaderStyle Width="260px" HorizontalAlign="Left" />
                                                <ItemStyle Width="260px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Tipo de orientador" DataField="NombreUniversidad">
                                                <HeaderStyle Width="260px" HorizontalAlign="Left" />
                                                <ItemStyle Width="260px" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Tutor(es)
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Código tutor:</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtCodigoTutor" MaxLength="15" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnAceptarCodigo" Text="Validar c&oacute;digo" class="btn btn-green btn-md btn-codigo" Style="margin-top: .75em" OnClick="btnAceptarCodigo_Click" />
                                    <button type="button" id="btnValidarCodigo" class="button_clip_39215E" data-toggle="modal" style="display: none" data-target=".dialog-md">Invitar tutorado</button>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="table-responsive">
                                    <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdTutores"
                                        RowStyle-CssClass="td" HeaderStyle-CssClass="th"
                                        CssClass="table table-bordered table-striped"
                                        EmptyDataText="No tiene tutores vinculados" OnRowCommand="grdTutores_RowCommand">
                                        <Columns>
                                            <asp:BoundField HeaderText="Nombre(s)" DataField="Tutor.Nombre">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Apellido Materno" DataField="Tutor.PrimerApellido">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Apellido Paterno" DataField="Tutor.SegundoApellido">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ID="btnViewDatosTutor" CommandName="ver" ImageUrl="../images/btn_search.png"
                                                        ToolTip="Editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Tutor.TutorID")+";"+DataBinder.Eval(Container.DataItem,"Tutor.CorreoElectronico") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="th"></HeaderStyle>
                                        <RowStyle CssClass="td"></RowStyle>
                                    </asp:GridView>
                                </div>
                                <asp:Button runat="server" ID="btnAñadirTutor" Text="Nuevo tutor" class="btn btn-green btn-md" Style="margin-top: .75em" OnClick="btnAñadirTutor_Click" />
                                <button type="button" id="target" class="btn-green btn-md" style="margin-top: .75em; display: none">
                                    Nuevo Tutor</button>
                            </div>
                            <div class="form-group">
                                <div class="pane panel-default" style="display: none" id="divTutor">
                                    <div class="panel-heading">
                                        Datos del Tutor
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">Nombre*</label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtTutorNombre" MaxLength="80" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <label class="col-sm-2 control-label">Primer apellido*</label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtTutorApellido1" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">Segundo apellido</label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtTutorApellido2" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <label class="col-sm-2 control-label">Sexo*</label>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList ID="ddlTutorSexo" runat="server" CssClass="form-control" readonly="true">
                                                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                                        <asp:ListItem Value="True">Hombre</asp:ListItem>
                                                        <asp:ListItem Value="False">Mujer</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">Fecha de nacimiento*</label>
                                                <div class="col-sm-4">
                                                    <div class="input-group date" id="datetimepickerFechNacimiento">
                                                        <asp:TextBox runat="server" ID="txtTutorFechaNaciemiento" CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar" style="font-size: 18px!important;"></span>
                                                        </span>
                                                    </div>
                                                </div>
                                                <label class="col-sm-2 control-label">Tel&eacute;fono*</label>
                                                <div class="col-sm-4">
                                                    <input type="tel" name="TelefonoFull" id="telefonotutor" maxlength="10" class="form-control" />
                                                    <asp:HiddenField runat="server" ID="txtTutorTelefono"></asp:HiddenField>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">Direcci&oacute;n*</label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txtTutorDireccion" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <label class="col-sm-2 control-label">Correo electr&oacute;nico*</label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox runat="server" ID="txtTutorEMail" MaxLength="100" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label">Parentesco*</label>
                                                <div class="col-sm-4">
                                                    <asp:DropDownList runat="server" ID="ddlTutorParentesco" CssClass="form-control">
                                                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                                        <asp:ListItem Value="1">Padre</asp:ListItem>
                                                        <asp:ListItem Value="2">Madre</asp:ListItem>
                                                        <asp:ListItem Value="3">Familiar</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4" style="text-align: right">
                                                    <asp:Button runat="server" ID="btnAgregarTutor" Text="Guardar tutor" class="btn btn-green btn-md" Style="margin-top: .75em" OnClick="btnAgregarTutor_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <a>
                                            <br />
                                            <br />
                                            NOTA: Los cambios en la lista de tutores se verán reflejados hasta dar clic en el botón "Guardar cambios"</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Datos de procedencia
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Pa&iacute;s*</label>
                                <div class="col-sm-4">
                                    <asp:UpdatePanel runat="server" ID="updPais">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbPais" AutoPostBack="True" OnSelectedIndexChanged="CbPais_SelectedIndexChanged" CssClass="form-control" required>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <label class="col-sm-2 control-label">Estado*</label>
                                <div class="col-sm-4">
                                    <asp:UpdatePanel ID="updEstado" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbEstado" AutoPostBack="True" OnSelectedIndexChanged="CbEstado_SelectedIndexChanged" CssClass="form-control" required>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Municipio*</label>
                                <div class="col-sm-4">
                                    <asp:UpdatePanel ID="updCiudad" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbMunicipio" AutoPostBack="True" CssClass="form-control" required>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Otra informaci&oacute;n
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Estado</label>
                                    <asp:TextBox ID="TxtFirma" MaxLength="200" CssClass="textarea_no_resize form-control" runat="server" Rows="4" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="col-xs-12">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="ChbInfo" runat="server"></asp:CheckBox>
                            Deseo recibir informaci&oacute;n acerca de escuelas y carreras.
                        </label>
                    </div>
                    <div class="col-xs-12 border_bottom"></div>
                    <br />
                </div>
                <!--</div>-->
            </div>
            <div class="text-center">
                <!--<div class="opciones_formulario">-->
                <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar cambios" OnClick="btnGuardarCambios_Click" class="btn btn-green btn-md" />
                <input type="button" id="btnCancelar" value="Cancelar" class="btn btn-cancel btn-md" />
                <!--</div>-->
            </div>
        </div>
    </div>

    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        var mymoment = moment(new Date());
        var mind = moment().date(1).month(0).year(mymoment.year() - 100);
        var maxd = moment().date(31).month(11).year(mymoment.year() - 17);
        $(function () {
            $('#datetimepickerFechNacimiento').datetimepicker({
                format: 'DD/MM/YYYY',
                locale: 'es',
                maxDate: maxd,
                minDate: mind
            });
        });

        function loadControls() {
            $('#<%=txtFechaNacimiento.ClientID %>').datepicker({ changeYear: true });
        }


        $("#telefono").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl/cmd+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+C
                (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+X
                (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything

                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        $("#telefonocasa").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl/cmd+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+C
                (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+X
                (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything

                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        $("#telefonotutor").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl/cmd+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+C
                (e.keyCode == 67 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: Ctrl/cmd+X
                (e.keyCode == 88 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything

                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });


    </script>
</asp:Content>
