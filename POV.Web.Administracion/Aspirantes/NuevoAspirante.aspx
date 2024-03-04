<%@ Page Title="" Language="C#" MasterPageFile="Aspirante.Master" AutoEventWireup="true"
    CodeBehind="NuevoAspirante.aspx.cs" Inherits="POV.Web.Administracion.Aspirantes.NuevoAspirante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>jquery-ui-v1.12.0.css" rel="stylesheet" type="text/css" />
        
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.alumnos.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.alumnos.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.tutores.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.tutores.js")%>" type="text/javascript"></script>       
    <script src="<% =Page.ResolveClientUrl("~/Scripts/moment-with-locales-v2.9.0.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap-datetimepicker-v4.15.35.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        //Variables para validar que los datos hayan sido ingresados
        var completo1 = '';
        var completo2 = '';
        var completo3 = '';
        var completo4 = '';
        var completo5 = '';
        var completo6 = '';
        var completo7 = '';
        var completo8 = '';
        var completo9 = '';
        var completo10 = '';
        var completo11 = '';
        var completo12 = '';
        var completo13 = '';
        var completo14 = '';
        var completo15 = '';
        var completo16 = '';
        var completo17 = '';
        var completo18 = '';
        var completo19 = '';
        var completo20 = '';
        var completo21 = '';
        var completo22 = ''; //Escuela
        var completo23 = ''; //Confirmar correo alumno
        var completo24 = ''; //Confirmar correo tutor
        var complete = '';
        var completoFormTutor = '';
        var completeTutor = '';

        $(document).ready(initPage);

        function sowMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");

            api.Show();
            resetHiden();
        }

        function messageError(error) {
            if (error == "error")
                sowMessage("Complete los datos requeridos");
            else
                sowMessage(error);
        }

        function reloadUbicacion(item) {
            setTimeout(function () {
                switch (item) {
                    case 1:
                        validarSelect($("#<%=ddlPais.ClientID%>"), 'pais');
                        break;
                    case 2:
                        validarSelect($("#<%=ddlEstado.ClientID%>"), 'estado');
                        break;
                    case 3:
                        validarSelect($("#<%=ddlMunicipio.ClientID%>"), 'municipio');
                        break;
                    case 4:
                        validarSelect($("#<%=txtNombre.ClientID%>"), 'nameAlumno');
                        break;
                    case 5:
                        validarSelect($("#<%=txtPrimerApellido.ClientID%>"), 'firstNameAlumno');
                        break;
                    case 6:
                        validarSelect($("#<%=CbSexo.ClientID%>"), 'sexAlumno');
                        break;
                    case 7:
                        validarSelect($("#<%=txtCurp.ClientID%>"), 'curpAlumno');
                        break;
                    case 8:
                        validarSelect($("#<%=txtFechaNacimiento.ClientID%>"), 'bornAlumno');
                        break;
                    case 9:
                        validarSelect($("#<%=txtCorreoElectronico.ClientID%>"), 'emailAlumno');
                        break;
                    case 10:
                        validarSelect($("#<%=txtNombreUsuario.ClientID%>"), 'userAlumno');
                        break;
                    case 11:
                        validarSelect($("#<%=txtPassword.ClientID%>"), 'passAlumno');
                        break;
                    case 12:
                        validarSelect($("#<%=txtConfirmarPassword.ClientID%>"), 'confirmPassAlumno');
                        break;
                    case 13:
                        validarSelect($("#<%=txtNombreTutor.ClientID%>"), 'nameTutor');
                        break;
                    case 14:
                        validarSelect($("#<%=txtPrimerApellidoTutor.ClientID%>"), 'firstNameTutor');
                        break;
                    case 15:
                        validarSelect($("#<%=ddlSexoTutor.ClientID%>"), 'sexoTutor');
                        break;
                    case 16:
                        validarSelect($("#<%=txtFechaNacimientoTutor.ClientID%>"), 'bornTutor');
                        break;
                    case 17:
                        validarSelect($("#<%=txtCurpTutor.ClientID%>"), 'curpTutor');
                        break;
                    case 18:
                        validarSelect($("#<%=txtEmailTutor.ClientID%>"), 'emailTutor');
                        break;
                    case 19:
                        validarSelect($("#<%=txtNombreUsuarioTutor.ClientID%>"), 'userNameTutor');
                        break;
                    case 20:
                        validarSelect($("#<%=txtPasswordTutor.ClientID%>"), 'passTutor');
                        break;
                    case 21:
                        validarSelect($("#<%=txtConfirmarPasswordTutor.ClientID%>"), 'confirmPassTutor');
                        break;
                    case 22:
                        validarSelect($("#<%=ddlEscuela.ClientID%>"), 'escuela');
                        break;
                    case 23:
                        validarSelect($("#<%=txtConfirmarCorreoElectronico.ClientID%>"), 'confirmEmail');
                        break;
                    case 24:
                        validarSelect($("#<%=txtConfirmarEmailTutor.ClientID%>"), 'confirmEmailTutor');
                        break;
                    default:
                        break;
                }
            }, 500);
        }

        function validarSelect(selector, idclass) {

            
            var message = '';
            var formato = false;
            if (idclass == 'emailAlumno' || idclass == 'emailTutor' || idclass == 'confirmEmail' || idclass == 'confirmEmailTutor') {
                var el_correo = selector.val(); //validar correo
                var er_email = /^(.+\@.+\..+)$/; // expresion regular para validar formato de correo
                if (selector.val() != 'undefined' && selector.val() != '') {
                    if (!er_email.test(el_correo)) {
                        message = 'Formato incorrecto';
                        formato = true;
                        $('#' + idclass).find('ul.list-unstyled>li').text(message);
                    }
                }
                else {
                    message = 'Data requerido';
                    formato = false;
                }
            } else {
                message = 'Data requerido';
                formato = false;
            }
            var selectRequired =
            [
                '<div class="help-block with-errors ' + idclass + '">',
                '<ul class="list-unstyled">',
                '<li>' + message + '.</li>',
                '</ul>',
                '</div>'
            ].join('');

            if (selector.val() != '' && selector.val() != undefined && selector.val() != 0 && !formato) {
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
            $('.button').button();

            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                }
            };

            $('#frmMain').submit(function (e) {
                // handle the invalid form...
                var pais = $("#<%=ddlPais.ClientID%> option:selected");
                var estado = $("#<%=ddlEstado.ClientID%>  option:selected");
                var municipio = $("#<%=ddlMunicipio.ClientID%>  option:selected");
                var escuela = $("#<%=ddlEscuela.ClientID%>  option:selected");
                var nombreAlumno = $("#<%=txtNombre.ClientID%>");
                var primerApellidoAlumno = $("#<%=txtPrimerApellido.ClientID%>");
                var sexo = $("#<%=CbSexo.ClientID%>  option:selected");
                var curpAlumno = $("#<%=txtCurp.ClientID%>");
                var fechaNacimientoAlumno = $("#<%=txtFechaNacimiento.ClientID%>");
                var correoAlumno = $("#<%=txtCorreoElectronico.ClientID%>");
                var usuarioAlumno = $("#<%=txtNombreUsuario.ClientID%>");
                var passAlumno = $("#<%=txtPassword.ClientID%>");
                var confirmPassAlumno = $("#<%=txtConfirmarPassword.ClientID%>");
                var confirmEmailAlumno = $("#<%=txtConfirmarCorreoElectronico.ClientID%>");


                completo1 = (pais.val() != undefined && pais.val() != '' && pais.val() != 0)
                completo2 = (estado.val() != undefined && estado.val() != '' && estado.val() != 0)
                completo3 = (municipio.val() != undefined && municipio.val() != '' && municipio.val() != 0);
                completo4 = (nombreAlumno.val() != undefined && nombreAlumno.val() != '' && nombreAlumno.val().length > 0);
                completo5 = (primerApellidoAlumno.val() != undefined && primerApellidoAlumno.val() != '' && primerApellidoAlumno.val().length > 0);
                completo6 = (sexo.val() != undefined && sexo.val() != '' && sexo.val() != 0);
                completo7 = (curpAlumno.val() != undefined && curpAlumno.val() != '' && curpAlumno.val().length > 0);
                completo8 = (fechaNacimientoAlumno.val() != undefined && fechaNacimientoAlumno.val() != '' && fechaNacimientoAlumno.val().length > 0);
                completo9 = (correoAlumno.val() != undefined && correoAlumno.val() != '' && correoAlumno.val().length > 0);
                completo10 = (usuarioAlumno.val() != undefined && usuarioAlumno.val() != '' && usuarioAlumno.val().length > 0);
                completo11 = (passAlumno.val() != undefined && passAlumno.val() != '' && passAlumno.val().length > 0);
                completo12 = (confirmPassAlumno.val() != undefined && confirmPassAlumno.val() != '' && confirmPassAlumno.val().length > 0);
                completo22 = (escuela.val() != undefined && escuela.val() != '' && escuela.val() != 0);
                completo23 = (confirmEmailAlumno.val() != undefined && confirmEmailAlumno.val() != '' && confirmEmailAlumno.val() != 0);

                complete = completo1 && completo2 && completo3 && completo4 && completo5 && completo6 && completo7 && completo8 && completo9 && completo10 && completo11 && completo12 && completo22 && completo23;

                completo13 = '';
                completo14 = '';
                completo15 = '';
                completo16 = '';
                completo17 = '';
                completo18 = '';
                completo19 = '';
                completo20 = '';
                completo21 = '';
                completoFormTutor = false;

                if (typeof $("#<%=txtNombreTutor.ClientID%>").val() !== 'undefined' && typeof $("#<%=txtPrimerApellidoTutor.ClientID%>").val() !== 'undefined'
                && typeof $("#<%=txtFechaNacimientoTutor.ClientID%>").val() !== 'undefined' && typeof $("#<%=txtCurpTutor.ClientID%>").val() !== 'undefined'
                && typeof $("#<%=txtEmailTutor.ClientID%>").val() !== 'undefined' && typeof $("#<%=txtNombreUsuarioTutor.ClientID%>").val() !== 'undefined'
                && typeof $("#<%=txtPasswordTutor.ClientID%>").val() !== 'undefined' && typeof $("#<%=txtConfirmarPasswordTutor.ClientID%>").val() !== 'undefined'
                    && typeof $("#<%=ddlSexoTutor.ClientID%>  option:selected").val() !== 'undefined' && typeof $("#<%=txtConfirmarEmailTutor.ClientID%>").val() !== 'undefined') {
                    completoFormTutor = true;
                    var nombreTutor = $("#<%=txtNombreTutor.ClientID%>");
                    var primerApellidoTutor = $("#<%=txtPrimerApellidoTutor.ClientID%>");
                    var sexoTutor = $("#<%=ddlSexoTutor.ClientID%>  option:selected");
                    var fechaNacimientoTutor = $("#<%=txtFechaNacimientoTutor.ClientID%>");
                    var curpTutor = $("#<%=txtCurpTutor.ClientID%>");
                    var correoTutor = $("#<%=txtEmailTutor.ClientID%>");
                    var usuarioTutor = $("#<%=txtNombreUsuarioTutor.ClientID%>");
                    var passTutor = $("#<%=txtPasswordTutor.ClientID%>");
                    var confirmPassTutor = $("#<%=txtConfirmarPasswordTutor.ClientID%>");
                    var confirmEmailTutor = $("#<%=txtConfirmarEmailTutor.ClientID%>");

                    completo13 = (nombreTutor.val() != undefined && nombreTutor.val() != '' && nombreTutor.val().length > 0);
                    completo14 = (primerApellidoTutor.val() != undefined && primerApellidoTutor.val() != '' && primerApellidoTutor.val().length > 0);
                    completo15 = (sexoTutor.val() != undefined && sexoTutor.val() != '' && sexoTutor.val() != 0 && sexoTutor.val().length > 0);
                    completo16 = (fechaNacimientoTutor.val() != undefined && fechaNacimientoTutor.val() != '' && fechaNacimientoTutor.val().length > 0);
                    completo17 = (curpTutor.val() != undefined && curpTutor.val() != '' && curpTutor.val().length > 0);
                    completo18 = (correoTutor.val() != undefined && correoTutor.val() != '' && correoTutor.val().length > 0);
                    completo19 = (usuarioTutor.val() != undefined && usuarioTutor.val() != '' && usuarioTutor.val().length > 0);
                    completo20 = (passTutor.val() != undefined && passTutor.val() != '' && passTutor.val().length > 0);
                    completo21 = (confirmPassTutor.val() != undefined && confirmPassTutor.val() != '' && confirmPassTutor.val().length > 0);
                    completo24 = (confirmEmailTutor.val() != undefined && confirmEmailTutor.val() != '' && confirmEmailTutor.val().length > 0);
                } else {
                    completo13 = true;
                    completo14 = true;
                    completo15 = true;
                    completo16 = true;
                    completo17 = true;
                    completo18 = true;
                    completo19 = true;
                    completo20 = true;
                    completo21 = true;
                    completo24 = true;
                }

                completeTutor = completo13 && completo14 && completo15 && completo16 && completo17 && completo18 && completo19 && completo20 && completo21 && completo24;

                if (!complete || (completoFormTutor && !completeTutor)) {
                    // Pais
                    if (!completo1)
                        reloadUbicacion(1);
                    // Estado
                    if (!completo2)
                        reloadUbicacion(2);
                    // Cuidad
                    if (!completo3)
                        reloadUbicacion(3);
                    // NombreAlumno
                    if (!completo4)
                        reloadUbicacion(4);
                    // PrimerApellidoAlumno
                    if (!completo5)
                        reloadUbicacion(5);
                    // SexoAlumno
                    if (!completo6)
                        reloadUbicacion(6);
                    // CurpAlumno
                    if (!completo7)
                        reloadUbicacion(7);
                    // FechaNacimientoAlumno
                    if (!completo8)
                        reloadUbicacion(8);
                    // CorreoAlumno
                    if (!completo9)
                        reloadUbicacion(9);
                    // UsuarioAlumno
                    if (!completo10)
                        reloadUbicacion(10);
                    // PasswordAlummno
                    if (!completo11)
                        reloadUbicacion(11);
                    // ConfirmPassAlumno
                    if (!completo12)
                        reloadUbicacion(12);

                    // NombreTutor
                    if (!completo13)
                        reloadUbicacion(13);
                    // PrimerApellidoTutor
                    if (!completo14)
                        reloadUbicacion(14);
                    // SexoTutor
                    if (!completo15)
                        reloadUbicacion(15);
                    // FechaNacimientoTutor
                    if (!completo16)
                        reloadUbicacion(16);
                    // CurpTutor
                    if (!completo17)
                        reloadUbicacion(17);
                    // CorreoTutor
                    if (!completo18)
                        reloadUbicacion(18);
                    // UsuarioTutor
                    if (!completo19)
                        reloadUbicacion(19);
                    // PasswordTutor
                    if (!completo20)
                        reloadUbicacion(20);
                    // ConfirmPassTutor
                    if (!completo21)
                        reloadUbicacion(21);
                    // Escuela
                    if (!completo22)
                        reloadUbicacion(22);
                    // ConfirmEmailAlumno
                    if (!completo23)
                        reloadUbicacion(23);
                    // ConfirmEmailTutor
                    if (!completo24)
                        reloadUbicacion(24);

                    messageError("error");
                    e.preventDefault();
                    setInterval(function () {
                        if (!complete || (completoFormTutor && !completeTutor)) {
                            // Pais
                            if (!completo1)
                                reloadUbicacion(1);
                            // Estado
                            if (!completo2)
                                reloadUbicacion(2);
                            // Cuidad
                            if (!completo3)
                                reloadUbicacion(3);
                            // NombreAlumno
                            if (!completo4)
                                reloadUbicacion(4);
                            // PrimerApellidoAlumno
                            if (!completo5)
                                reloadUbicacion(5);
                            // SexoAlumno
                            if (!completo6)
                                reloadUbicacion(6);
                            // CurpAlumno
                            if (!completo7)
                                reloadUbicacion(7);
                            // FechaNacimientoAlumno
                            if (!completo8)
                                reloadUbicacion(8);
                            // CorreoAlumno
                            if (!completo9)
                                reloadUbicacion(9);
                            // UsuarioAlumno
                            if (!completo10)
                                reloadUbicacion(10);
                            // PasswordAlummno
                            if (!completo11)
                                reloadUbicacion(11);
                            // ConfirmPassAlumno
                            if (!completo12)
                                reloadUbicacion(12);

                            // NombreTutor
                            if (!completo13)
                                reloadUbicacion(13);
                            // PrimerApellidoTutor
                            if (!completo14)
                                reloadUbicacion(14);
                            // SexoTutor
                            if (!completo15)
                                reloadUbicacion(15);
                            // FechaNacimientoTutor
                            if (!completo16)
                                reloadUbicacion(16);
                            // CurpTutor
                            if (!completo17)
                                reloadUbicacion(17);
                            // CorreoTutor
                            if (!completo18)
                                reloadUbicacion(18);
                            // UsuarioTutor
                            if (!completo19)
                                reloadUbicacion(19);
                            // PasswordTutor
                            if (!completo20)
                                reloadUbicacion(20);
                            // ConfirmPassTutor
                            if (!completo21)
                                reloadUbicacion(21);
                            // Escuela
                            if (!completo22)
                                reloadUbicacion(22);
                            // ConfirmEmailAlumno
                            if (!completo23)
                                reloadUbicacion(23);
                            // ConfirmEmailTutor
                            if (!completo24)
                                reloadUbicacion(24);
                        }
                    }, 500);
                }
                else {
                    // everything looks good!
                    return;
                }
            });
        }

        $(function () {
            loadCalendar();
        });
    </script>

    <style type="text/css">
        .panel {
            border-top-left-radius: 35px !important;
            border-top-right-radius: 35px !important;
        }

        .link {
            color: #fff;
            font-size: 16pt;
            font-family: Helvetica;
            color: #2e6da4;
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fondo_gris" style="padding: 20px">
        <div class="container">
            <div class="col-sm-offset-1">
            </div>
            <div class="row" id="formEstudiante" runat="server">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Informaci&oacute;n de la escuela
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div id="pais">
                                    <div class="col-sm-6" id="paisAdd">
                                        <asp:UpdatePanel runat="server" ID="updPais">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlPais" runat="server" TabIndex="1" CssClass="form-control" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="help-block with-errors pais" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div id="estado">
                                    <div class="col-sm-6" id="estadoAdd">
                                        <asp:UpdatePanel ID="updEstado" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlEstado" runat="server" TabIndex="2" CssClass="form-control" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="help-block with-errors estado" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div id="municipio">
                                    <div class="col-sm-6" id="municipioAdd">
                                        <asp:UpdatePanel ID="updCiudad" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlMunicipio" runat="server" TabIndex="3" CssClass="form-control" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="help-block with-errors municipio" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div id="escuela">
                                    <div class="col-sm-6" id="escuelaAdd">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlEscuela" runat="server" TabIndex="4" CssClass="form-control" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlEscuela_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="help-block with-errors escuela" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel-footer"></div>
                    <div class="panel-heading">Informaci&oacute;n del estudiante</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <!--Nombre-->
                                <div id="nameAlumno">
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" Font-Size="Large" MaxLength="80" TabIndex="5" placeholder="Nombre*"    ></asp:TextBox>
                                        <div class="help-block with-errors nameAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <!--Primer apellido-->
                                <div id="firstNameAlumno">
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtPrimerApellido" CssClass="form-control" Font-Size="Large" MaxLength="50" TabIndex="6" placeholder="Primer apellido*"    ></asp:TextBox>
                                        <div class="help-block with-errors firstNameAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <!--Segundo apellido-->
                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtSegundoApellido" CssClass="form-control" Font-Size="Large" MaxLength="50" TabIndex="7" placeholder="Segundo apellido"></asp:TextBox>
                                </div>
                                <!--Sexo-->
                                <div id="sexAlumno">
                                    <div class="col-sm-6" id="sexoAdd">
                                        <asp:UpdatePanel ID="updSexo" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="CbSexo" runat="server" TabIndex="8" CssClass="form-control" Font-Size="Large" OnSelectedIndexChanged="CbSexo_SelectedIndexChanged">
                                                    <asp:ListItem Value="">Sexo*</asp:ListItem>
                                                    <asp:ListItem Value="True">Hombre</asp:ListItem>
                                                    <asp:ListItem Value="False">Mujer</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="help-block with-errors sexAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <!--Curp-->
                                <div id="curpAlumno">
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtCurp" CssClass="form-control" Font-Size="Large" MaxLength="20" TabIndex="9" placeholder="Curp *"    ></asp:TextBox>
                                        <div class="help-block with-errors curpAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <!--Fecha de nacimiento-->
                                <div id="bornAlumno">
                                    <div class="col-sm-6">
                                        <div class="input-group date" id="datetimepickerFechNacimiento">
                                            <asp:TextBox runat="server" ID="txtFechaNacimiento" CssClass="form-control" TabIndex="10" MaxLength="15"  ></asp:TextBox>
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                        <div class="help-block with-errors bornAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <!--Correo electrónico-->
                                <div id="emailAlumno">
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtCorreoElectronico" Font-Size="Large" MaxLength="100" TabIndex="11" TextMode="Email" CssClass="form-control" placeholder="Correo electrónico*"    ></asp:TextBox>
                                        <div class="help-block with-errors emailAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div id="confirmEmail">
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtConfirmarCorreoElectronico" Font-Size="Large" MaxLength="100" TabIndex="11" TextMode="Email" CssClass="form-control" placeholder="Confirmar correo electrónico*"  ></asp:TextBox>
                                        <div class="help-block with-errors confirmEmail" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer"></div>
                    <div class="panel-heading">Informaci&oacute;n de usuario estudiante</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <!--Nombre de usuario-->
                            <div id="userAlumno">
                                <div class="form-group username">
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtNombreUsuario" Font-Size="Large" MaxLength="50" TabIndex="12" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Usuario*"    ></asp:TextBox>
                                        <div class="help-block with-errors userAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--Password-->
                            <div id="passAlumno">
                                <div class="form-group">
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtPassword" Font-Size="Large" MaxLength="50" TabIndex="13" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Contraseña*"    ></asp:TextBox>
                                        <div class="help-block with-errors passAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--Confirmar Password-->
                            <div class="form-group">
                                <div id="confirmPassAlumno">
                                    <div class="col-sm-8">
                                        <asp:TextBox runat="server" ID="txtConfirmarPassword" Font-Size="Large" MaxLength="50" TabIndex="14" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Confirmar contraseña*"    ></asp:TextBox>
                                        <div class="help-block with-errors confirmPassAlumno" style="display: none">
                                            <ul class="list-unstyled">
                                                <li>Dato requerido.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer"></div>
                </div>
            </div>

            <asp:UpdatePanel ID="upd" runat="server">
                <ContentTemplate>


                    <div id="formTutor" class="row" runat="server" visible="false">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Informaci&oacute;n del padre/madre
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <!--Nombre-->
                                        <div id="nameTutor">
                                            <div class="col-sm-6">

                                                <asp:TextBox runat="server" ID="txtNombreTutor" CssClass="form-control" Font-Size="Large" MaxLength="80" TabIndex="15" placeholder="Nombre*"    ></asp:TextBox>
                                                <div class="help-block with-errors nameTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <!--Primer apellido-->
                                        <div id="firstNameTutor">
                                            <div class="col-sm-6">
                                                <asp:TextBox runat="server" ID="txtPrimerApellidoTutor" CssClass="form-control" Font-Size="Large" MaxLength="50" TabIndex="16" placeholder="Primer apellido *"    ></asp:TextBox>
                                                <div class="help-block with-errors firstNameTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <!--Segundo apellido-->
                                        <div class="col-sm-6">
                                            <asp:TextBox runat="server" ID="txtSegundoApellidoTutor" CssClass="form-control" Font-Size="Large" MaxLength="50" TabIndex="17" placeholder="Segundo apellido"></asp:TextBox>
                                        </div>
                                        <!--Sexo-->
                                        <div id="sexoTutor">
                                            <div class="col-sm-6" id="sexoTutorAdd">
                                                <asp:UpdatePanel ID="formUpd" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlSexoTutor" runat="server" TabIndex="18" CssClass="form-control" Font-Size="Large" OnSelectedIndexChanged="ddlSexoTutor_SelectedIndexChanged">
                                                            <asp:ListItem Value="">Sexo*</asp:ListItem>
                                                            <asp:ListItem Value="True">Hombre</asp:ListItem>
                                                            <asp:ListItem Value="False">Mujer</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="help-block with-errors sexoTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <!-- Fecha de nacimiento-->
                                        <div id="bornTutor">
                                            <div class="col-sm-6">
                                                <div class="input-group date" id="datetimepickerFechNacimientoTutor">
                                                    <asp:TextBox runat="server" ID="txtFechaNacimientoTutor" CssClass="form-control" Font-Size="Large" TabIndex="19"    ></asp:TextBox>
                                                    <span class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                                <div class="help-block with-errors bornTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <!--Curp-->
                                        <div id="curpTutor">
                                            <div class="col-sm-6">
                                                <asp:TextBox runat="server" ID="txtCurpTutor" CssClass="form-control" Font-Size="Large" MaxLength="20" TabIndex="20" placeholder="Curp *"    ></asp:TextBox>
                                                <div class="help-block with-errors curpTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <!--Correo electrónico-->
                                        <div id="emailTutor">
                                            <div class="col-sm-6">
                                                <asp:TextBox runat="server" ID="txtEmailTutor" Font-Size="Large" MaxLength="100" TextMode="Email" TabIndex="21" CssClass="form-control" placeholder="Correo electrónico*"    ></asp:TextBox>
                                                <div class="help-block with-errors emailTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="confirmEmailTutor">
                                            <div class="col-sm-6">
                                                <asp:TextBox runat="server" ID="txtConfirmarEmailTutor" Font-Size="Large" MaxLength="100" TextMode="Email" TabIndex="21" CssClass="form-control" placeholder="Confirmar correo electrónico*"  ></asp:TextBox>
                                                <div class="help-block with-errors confirmEmailTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer"></div>
                            <div class="panel-heading">
                                Informacion de usuario padre/madre
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <!--Nombre de usuario-->
                                    <div id="userNameTutor">
                                        <div class="form-group usernametutor">
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtNombreUsuarioTutor" Font-Size="Large" MaxLength="50" TabIndex="22" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Usuario*"    ></asp:TextBox>
                                                <div class="help-block with-errors userNameTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--Password-->
                                    <div class="form-group">
                                        <div id="passTutor">
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtPasswordTutor" Font-Size="Large" MaxLength="50" TabIndex="23" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Contraseña*"    ></asp:TextBox>
                                                <div class="help-block with-errors passTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Confirmar Password-->
                                    <div class="form-group">
                                        <div id="confirmPassTutor">
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtConfirmarPasswordTutor" Font-Size="Large" MaxLength="50" TabIndex="24" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Confirmar contraseña*"    ></asp:TextBox>
                                                <div class="help-block with-errors confirmPassTutor" style="display: none">
                                                    <ul class="list-unstyled">
                                                        <li>Dato requerido.</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer"></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <!-- botones guardar y cancelar -->
            <div class="col-xs-12 text-center" style="height: 100px;">
                <div class="col-xs-6 col-md-6">
                    <asp:HyperLink ID="HyperLink1" Text="Volver" TabIndex="25" runat="server" Width="80px" NavigateUrl="http://testpov.grupoplenum.com/PaginaAcceso.aspx" Style="text-decoration: none; color: #fff;" CssClass="left txt-helvetica-label btn-entrar"></asp:HyperLink>
                </div>
                <div class="col-xs-6 col-md-6">
                    <asp:Button runat="server" ID="btnGuardar" TabIndex="26" Text="Guardar" Width="80px" CssClass="btn-entrar btn-md btn-block btnAcces" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>

        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
        <asp:HiddenField runat="server" ID="hdnalumn" />
        <div id="dialogquestion" title="YOY">
            <p id="dialogtext">
            </p>
        </div>
    </div>


    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);
        prm.add_endRequest(endRequests);

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnEndRequestValidateTutor);

        function OnEndRequestValidateTutor(sender, args) {
            var mymoment = moment(new Date());
            var mind = moment().date(1).month(0).year(mymoment.year() - 100);
            var maxd = moment().date(31).month(11).year(mymoment.year() - 17);
            $('#<%=txtFechaNacimientoTutor.ClientID%>').datetimepicker({
                format: 'DD/MM/YYYY',
                locale: 'es',
                maxDate: maxd,
                minDate: mind
            });


            $("#datetimepickerFechNacimientoTutor span").click(function (e) {
                $('#<%=txtFechaNacimientoTutor.ClientID %>').data("DateTimePicker").show()
            });

            $('#<%=txtNombreUsuarioTutor.ClientID %>').blur(function () {
                if (/^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val())) {
                    $("#userimgtutor").remove();
                    verificarUsuarioTutor();
                } else {
                    $("#userimgtutor").remove();
                    $(".usernametutor").append('<img src="../images/hr.gif" id="userimgtutor" title="nombre de usuario no correcto" >');
                    alertify.set('notifier', 'position', 'bottom-left');
                    alertify.set('notifier', 'delay', 5);
                    alertify.error("El nombre de usuario está incorrecto");
                    $(".btnAcces").prop('disabled', true);
                }
            });

            setTimeout(function () {
                if (typeof $('#<%=txtNombreUsuarioTutor.ClientID %>').val() !== 'undefined' && $('#<%=txtNombreUsuarioTutor.ClientID %>').val().length >= 6) {
                    if (typeof /^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val()) !== 'undefined') {
                        $("#userimgtutor").remove();
                        verificarUsuarioTutor();
                    } else {
                        $("#userimgtutor").remove();
                        $(".usernametutor").append('<img src="../images/hr.gif" id="userimgtutor" title="nombre de usuario no correcto" >');
                        alertify.set('notifier', 'position', 'bottom-left');
                        alertify.set('notifier', 'delay', 5);
                        alertify.error("El nombre de usuario está incorrecto");
                        $(".btnAcces").prop('disabled', true);
                    }
                }
            }, 1000);
            }

            var loadCalendar = function () {
                var mymoment = moment(new Date());
                var mind = moment().date(1).month(0).year(mymoment.year() - 100);
                var maxd = moment().date(31).month(11).year(mymoment.year() - 17);
                $('#<%=txtFechaNacimiento.ClientID%>').datetimepicker({
                    format: 'DD/MM/YYYY',
                    locale: 'es',
                    maxDate: maxd,
                    minDate: mind
                });

                $('#<%=txtFechaNacimientoTutor.ClientID%>').datetimepicker({
                    format: 'DD/MM/YYYY',
                    locale: 'es',
                    maxDate: maxd,
                    minDate: mind
                });

                $("#datetimepickerFechNacimiento span").click(function (e) {
                    $('#<%=txtFechaNacimiento.ClientID %>').data("DateTimePicker").show()
                });

                $("#datetimepickerFechNacimientoTutor span").click(function (e) {
                    $('#<%=txtFechaNacimientoTutor.ClientID %>').data("DateTimePicker").show()
            });
            }
            function loadControls(sender, args) {
                loadCalendar();
                CoreAlumnos.ValidacionesAlumno.init({
                    btnguardar: $('#<%=btnGuardar.ClientID %>'),
                    buttons: $('.boton'),
                    txtalumnombre: $('#<%=txtNombre.ClientID %>'),
                    txtusername: $('#<%=txtNombreUsuario.ClientID %>'),
                    dialog: $('#dialogquestion'),
                    textdialog: $('#dialogquestion').find('#dialogtext')
                });

                CoreTutores.ValidacionesTutor.init({
                    btnguardar: $('#<%=btnGuardar.ClientID %>'),
                    buttons: $('.boton'),
                    txttutornombre: $('#<%=txtNombreTutor.ClientID %>'),
                    txtusername: $('#<%=txtNombreUsuarioTutor.ClientID %>'),
                    dialog: $('#dialogquestion'),
                    textdialog: $('#dialogquestion').find('#dialogtext')
                });
            }

            //estudiante
            setTimeout(function () {
                var valid = $('#<%=txtNombre.ClientID %>').val().length > 0 &&
                    $('#<%=txtPrimerApellido.ClientID %>').val().length > 0 &&
	                $('#<%=CbSexo.ClientID %>').val().length > 0 &&
	                $('#<%=txtCorreoElectronico.ClientID %>').val().length > 0;

                if (!valid) $('#<%=txtNombreUsuario.ClientID %>').val(null);

                $('#<%=txtPassword.ClientID %>').val(null);
                $('#<%=txtConfirmarPassword.ClientID %>').val(null);
            }, 600);

            //padre/madre
            setTimeout(function () {
                var valid = typeof $('#<%=txtNombreTutor.ClientID %>').val() !== 'undefined' && $('#<%=txtNombreTutor.ClientID %>').val().length > 0 &&
                    $('#<%=txtPrimerApellidoTutor.ClientID %>').val().length > 0 &&
	                $('#<%=ddlSexoTutor.ClientID %>').val().length > 0 &&
	                $('#<%=txtEmailTutor.ClientID %>').val().length > 0;

                if (!valid) $('#<%=txtNombreUsuarioTutor.ClientID %>').val(null);

                $('#<%=txtPasswordTutor.ClientID %>').val(null);
                $('#<%=txtConfirmarPasswordTutor.ClientID %>').val(null);
            }, 600);

            //estudiante
            $('#<%=txtNombreUsuario.ClientID %>').blur(function (event) {
            if (/^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val())) {
                $("#userimg").remove();
                verificarUsuario();
            } else {
                $("#userimg").remove();
                $(".username").append('<img src="../images/hr.gif" id="userimg" title="nombre de usuario no correcto" >');
                alertify.set('notifier', 'position', 'bottom-left');
                alertify.set('notifier', 'delay', 5);
                alertify.error("El nombre de usuario está incorrecto");
                $(".btnAcces").prop('disabled', true);
            }
        });

        //padre/madre
        $('#<%=txtNombreUsuarioTutor.ClientID %>').blur(function () {
            if (/^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val())) {
                $("#userimgtutor").remove();
                verificarUsuarioTutor();
            } else {
                $("#userimgtutor").remove();
                $(".usernametutor").append('<img src="../images/hr.gif" id="userimgtutor" title="nombre de usuario no correcto" >');
                alertify.set('notifier', 'position', 'bottom-left');
                alertify.set('notifier', 'delay', 5);
                alertify.error("El nombre de usuario está incorrecto");
                $(".btnAcces").prop('disabled', true);
            }
        });

        //estudiante
        function verificarUsuario() {
            var self = CoreAlumnos.ValidacionesAlumno;
            var user = $('#<%=txtNombreUsuario.ClientID %>').val();
            if (user != "" && user.length >= 6) {

                self.validateUsuario();

            }
            else {
                var id = $("#userimg").length;
                if (id > 0) {
                    $("#userimg").remove();
                }
            }
        }

        //padre/madre
        function verificarUsuarioTutor() {
            var self = CoreTutores.ValidacionesTutor;
            var user = $('#<%=txtNombreUsuarioTutor.ClientID %>').val();
            if (user != "" && user.length >= 6)
                self.validateUsuario();
            else {
                var id = $("#userimgtutor").length;
                if (id > 0) {
                    $("#userimgtutor").remove();
                }
            }
        }

        //estudiante
        $('#<%=txtNombreUsuario.ClientID %>').keyup(function () {
            var user = $(this).val();
            if (user.length < 6) {
                var id = $("#userimg").length;
                if (id > 0) {
                    $("#userimg").remove();
                }
            }
        });

        //padre/madre
        $('#<%=txtNombreUsuarioTutor.ClientID %>').keyup(function () {
            var user = $(this).val();
            if (user.length < 6) {
                var id = $("#userimgtutor").length;
                if (id > 0) {
                    $("#userimgtutor").remove();
                }
            }
        });

        function endRequests(sender, args) {

        }
    </script>
</asp:Content>
