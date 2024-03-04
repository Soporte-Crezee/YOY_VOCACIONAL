<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarPerfil.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.EditarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .input_file_hidden {
            background: transparent;
            width: 400px;
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
            padding: 0px 0px 3px;
            width: 100px;
            vertical-align: middle;
            text-align: center;
        }

        #label_photo {
            font-size: 12px;
            display: block;
            width: 280px;
        }

        .container_photo_select {
            width: 400px;
            position: relative;
            padding: 3px;
            height: 32px;
        }

        .ui-datepicker-year {
            color: #502e85;
        }

        .ui-datepicker-month {
            color: #502e85;
        }
    </style>


    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/moment-with-locales-v2.9.0.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap-datetimepicker-v4.15.35.js")%>" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/css/intlTelInput.css" rel="stylesheet" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/intlTelInput.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(initPage);

        function showMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            resetHiden();
            api.Show();
        }

        function messageError(error) {
            if (error == "error")
                showMessage("Complete los datos requeridos");
                //alert("Complete los datos requeridos");
            else
                showMessage(error);
            //alert(error);
        }

        function justNumbers(e) {
            var keynum = window.event ? window.event.keyCode : e.which;
            if ((keynum == 8) || (keynum == 46))
                return true;
            return /\d/.test(String.fromCharCode(keynum));
        }

        function reloadUbicacion(item) {
            setTimeout(function () {
                switch (item) {
                    case 1:
                        validarSelect($("#<%=CbPais.ClientID%>"), 'pais');
                        break;
                    case 2:
                        validarSelect($("#<%=CbEstado.ClientID%>"), 'estado');
                        break;
                    case 3:
                        validarSelect($("#<%=CbMunicipio.ClientID%>"), 'municipio');
                        break;
                    case 4:
                        validarSelect($("#telefonocelular"), 'celphone');
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
            $("#<%=txtSegundoApellido.ClientID%>").focus();
            $(".boton").button();

            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                }
            };
            setTimeout(function () {
                var telefono1 = $("#<%=TxtTelefono.ClientID%>").val();

                if (telefono1 != undefined || telefono1.length > 0) {
                    $("#telefonocelular").intlTelInput("setNumber", telefono1);
                }

                var telefono2 = $("#<%=TxtTelefonoCasa.ClientID%>").val();

                if (telefono2 != undefined || telefono2.length > 0) {
                    $("#telefonocasa").intlTelInput("setNumber", telefono2);
                }
            }, 500)

            $(".telefono").intlTelInput({
                nationalMode: false,
                formatOnDisplay: false,
                separateDialCode: true,
                utilsScript: "<% =Page.ResolveClientUrl("~/Scripts/")%>telefonoinput/js/utils.js",
                preferredCountries: ["mx", "AR", "co", "ve", "cl"]
            });

            $(".telefono").keydown(function (e) {
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

            $('#form1').validator(validations).on('submit', function (e) {
                // handle the invalid form...
                var pais = $("#<%=CbPais.ClientID%> option:selected");
                var estado = $("#<%=CbEstado.ClientID%>  option:selected");
                var municipio = $("#<%=CbMunicipio.ClientID%>  option:selected");
                var celular = $("#telefonocelular");
                var telefono = $("#telefonocasa");

                var completo1 = (pais.val() != undefined && pais.val() != '')
                var completo2 = (estado.val() != undefined && estado.val() != '')
                var completo3 = (municipio.val() != undefined && municipio.val() != '');
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
                    complete = completo1 && completo2 && completo3 && completo4 && completo5;
                else
                    complete = completo1 && completo2 && completo3 && completo4;

                if (!complete) {
                    if (!completo1)
                        reloadUbicacion(1);
                    if (!completo2)
                        reloadUbicacion(2);
                    if (!completo3)
                        reloadUbicacion(3);
                    if (!completo4)
                        reloadUbicacion(4);
                    if (telefono.val().length > 0) {
                        if (!completo5)
                            reloadUbicacion(5);
                    }

                    messageError("error");
                    e.preventDefault();
                }
                else {
                    // everything looks good!
                    return;
                }
            });

            $("#form1").submit(function () {
                $("#<%=TxtTelefono.ClientID%>").val($("#telefonocelular").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                $("#<%=TxtTelefonoCasa.ClientID%>").val($("#telefonocasa").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));

                var error = $("#telefonocelular").intlTelInput("getValidationError");

                if (error === intlTelInputUtils.validationError.IS_POSSIBLE) {
                    $("#<%=TxtTelefono.ClientID%>").val($("#telefonocelular").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                    } else {
                        if ($("#telefonocelular").val().trim() == "")
                            $("#<%=TxtTelefono.ClientID%>").val('');
                }

                var errorCasa = $("#telefonocasa").intlTelInput("getValidationError");

                if (errorCasa === intlTelInputUtils.validationError.IS_POSSIBLE) {
                    $("#<%=TxtTelefonoCasa.ClientID%>").val($("#telefonocasa").intlTelInput("getNumber", intlTelInputUtils.numberFormat.E164));
                    } else {
                        if ($("#telefonocasa").val().trim() == "")
                            $("#<%=TxtTelefonoCasa.ClientID%>").val('');
                }
            });

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

            $("#<%=TxtTelefonoCasa.ClientID%>").on("keypress", function (e) {
                return justNumbers(e);
            });

            $("#<%=TxtTelefono.ClientID%>").on("keypress", function (e) {
                return justNumbers(e);
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <asp:Label ID="LblNombreUsuario" runat="server" Text="" Style="display: none;"></asp:Label>
    <ul class="breadcrumb">
        <li>Editar perfil
        </li>
    </ul>
    <div class="col-xs-12">
        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
        <div class="panel panel-default">
            <div class="panel-heading">
                Informaci&oacute;n de padre/madre
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
                            <div class="input-group date" id="datetimepickerFechNacimiento">
                                <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                <span class="input-group-addon">
                                    <span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </div>
                        <label class="col-sm-2 control-label">Direcci&oacute;n</label>
                        <div class="col-sm-4">
                            <asp:TextBox runat="server" ID="txtDirecion" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Informaci&oacute;n complementaria
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Nombre de usuario</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="TxtUsuario" MaxLength="50" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 control-label">Correo electr&oacute;nico*</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="TxtEmail" TextMode="Email" MaxLength="100" runat="server" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label">Tel&eacute;fono celular*</label>
                        <div class="col-sm-4" id="celphone">
                            <input type="tel" id="telefonocelular" style="width: 100%" maxlength="10" class="form-control telefono" required="" data-required-error="Dato requerido" />
                            <asp:HiddenField ID="TxtTelefono" runat="server"></asp:HiddenField>
                            <div class="help-block with-errors celphone" style="display: none;">
                                <ul class="list-unstyled">
                                    <li>Dato requerido.</li>
                                </ul>
                            </div>
                        </div>
                        <label class="col-sm-2 control-label">Tel&eacute;fono casa</label>
                        <div class="col-sm-4" id="homephone">
                            <input type="tel" id="telefonocasa" style="width: 100%" maxlength="10" class="form-control telefono" />
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
                Datos de procedencia
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div id="pais">
                            <label class="col-sm-2 control-label">Pa&iacute;s*</label>
                            <div class="col-sm-4" id="paisAdd">
                                <asp:UpdatePanel runat="server" ID="updPais">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="CbPais" AutoPostBack="True" OnSelectedIndexChanged="CbPais_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="help-block with-errors pais" style="display: none">
                                    <ul class="list-unstyled">
                                        <li>Seleccione un elemento de la lista.</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div id="estado">
                            <label class="col-sm-2 control-label">Estado*</label>
                            <div class="col-sm-4" id="estadoAdd">
                                <asp:UpdatePanel ID="updEstado" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="CbEstado" AutoPostBack="True" OnSelectedIndexChanged="CbEstado_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="help-block with-errors estado" style="display: none">
                                    <ul class="list-unstyled">
                                        <li>Seleccione un elemento de la lista.</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="municipio">
                            <label class="col-sm-2 control-label">Municipio*</label>
                            <div class="col-sm-4" id="municipioAdd">
                                <asp:UpdatePanel ID="updCiudad" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="CbMunicipio" AutoPostBack="True" OnSelectedIndexChanged="CbMunicipio_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="help-block with-errors municipio" style="display: none">
                                    <ul class="list-unstyled">
                                        <li>Seleccione un elemento de la lista.</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="text-center" style="padding: 5px 0 0 0">
        <div class="opciones_formulario">
            <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar" OnClick="btnGuardarCambios_Click" class="btn btn-green btn-md" />
            <asp:Button ID="btnCancelarPerfil" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-cancel btn-md" UseSubmitBehavior="false" />
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
        }


    </script>
</asp:Content>
