<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarPerfil.aspx.cs" Inherits="POV.Web.PortalUniversidad.Pages.EditarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(initPage);
        function messageError(error) {
            if (error == "error")
                alert("Complete los datos requeridos");
            else
                alert(error);
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
                    default:
                        break;
                }
                //Actualiza una sección de div $("#div").load(location.href + " #div" + ">*", "");
            }, 200);
        }

        function validarSelect(selector, idclass) {
            var selectRequired =
            [
                '<div class="help-block with-errors ' + idclass + '">',
                '<ul class="list-unstyled">',
                '<li>Seleccione un elemento de la lista.</li>',
                '</ul>',
                '</div>'
            ].join('');

            if (selector.val() != '' && selector.val() != undefined) {
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
                    }, 300);
                }
            }
        }

        function initPage() {
            $("#<%=txtNombre.ClientID%>").focus();
            $(".boton").button();

            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                }
            };

            $('#frmMain').validator(validations).on('submit', function (e) {
                // handle the invalid form...
                var pais = $("#<%=CbPais.ClientID%> option:selected");
                var estado = $("#<%=CbEstado.ClientID%>  option:selected");
                var municipio = $("#<%=CbMunicipio.ClientID%>  option:selected");

                var completo1 = (pais.val() != undefined && pais.val() != '')
                var completo2 = (estado.val() != undefined && estado.val() != '')
                var completo3 = (municipio.val() != undefined && municipio.val() != '');

                var complete = completo1 && completo2 && completo3;
                

                if (!complete && !web) {
                    if (!completo1)
                        reloadUbicacion(1);
                    if (!completo2)
                        reloadUbicacion(2);
                    if (!completo3)
                        reloadUbicacion(3);

                    messageError("error");
                    e.preventDefault();
                }
                else {
                    // everything looks good!
                    return;
                }
            });

            $("#<%=TxtTelefono.ClientID%>").on("keypress", function (e) {
                var keynum = window.event ? window.event.keyCode : e.which;
                if ((keynum == 8) || (keynum == 46))
                    return true;
                return /\d/.test(String.fromCharCode(keynum));
            });

            var options = {
                'maxCharacterSize': 254,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 244,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=TxtDescripcion.ClientID%>').textareaCount(options);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="LblNombreUsuario" runat="server" Text="" Style="display: none;"></asp:Label>
    <ol class="breadcrumb">
        <li>Editar perfil</li>
    </ol>
    <div class="">
        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n de universidad
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">Nombre*</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtNombre" MaxLength="250" runat="server" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 control-label">P&aacute;gina web</label>
                    <div class="col-sm-4 pag1">
                        <asp:TextBox ID="txtPaginaWeb" MaxLength="500" runat="server" CssClass="form-control" TextMode="Url" pattern="https?://.+" ></asp:TextBox>
                         <div class="help-block with-errors"></div>
                    </div>                     
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">Direcci&oacute;n*</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtDirecion" MaxLength="500" Rows="2" TextMode="MultiLine" Width="100%" CssClass="textarea_no_resize form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <label class="col-sm-2 control-label">Nombre corto</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtSiglas" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        <div class="help-block with-errors"></div>
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
                    <label class="col-sm-2 control-label">Nombre de usuario*</label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="TxtUsuario" MaxLength="50" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>                        
                    </div>
                    <div class="col-sm-1"></div>
                    <label class="col-sm-2 control-label">Correo electr&oacute;nico*</label>
                    <div class="col-sm-3 email1">
                        <asp:TextBox ID="TxtEmail" MaxLength="100" runat="server" TextMode="Email" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="col-sm-1 email"></div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label">Tel&eacute;fono*</label>
                    <div class="col-sm-3 telefono1">
                        <asp:TextBox ID="TxtTelefono" runat="server" MaxLength="20" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="col-sm-1 telefono"></div>
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
    <div class="panel panel-default">
        <div class="panel-heading">
            Otra informaci&oacute;n
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-sm-2 control-label">Descripci&oacute;n</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="TxtDescripcion" MaxLength="254" CssClass="textarea_no_resize form-control" runat="server" Rows="4" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="text-center ">
        <div class="opciones_formulario">
            <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar" OnClick="btnGuardarCambios_Click" class="btn btn-green btn-md" />
            <asp:Button ID="btnCancelarPerfil" runat="server"  UseSubmitBehavior="false"  Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-cancel btn-md"
                formnovalidate="formnovalidate" />
        </div>
    </div>
    <div id="panel-container">
        <asp:TextBox runat="server" ID="txtCorreoEdit" Style="display: none;"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtTelefonoEdit" Style="display: none;"></asp:TextBox>
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);
        prm.add_endRequest(endRequests);
        function loadControls(sender, args) {
            var ddate = new Date();
            var maxddate = new Date(ddate.getYear() - 17, -1, 1);
            $(".boton").button();

            CoreUsuarios.ValidacionesUsuario.init({
                //buttons
                btnguardar: $('#<%=btnGuardarCambios.ClientID %>'),
                buttons: $('.boton'),

                //identificadores
                uiusername: $('#<%=TxtUsuario.ClientID %>'),
                uiemail: $('#<%=TxtEmail.ClientID %>'),
                uitelefono: $('#<%=TxtTelefono.ClientID %>'),
                uiuniversidadid: '',


                dialog: $('#dialogquestion'),
                textdialog: $('#dialogquestion').find('#dialogtext')
            });
        }

        var self = CoreUsuarios.ValidacionesUsuario;

        $('#<%=TxtEmail.ClientID %>').keyup(function () {
            emailValid();
        });

        $('#<%=TxtTelefono.ClientID %>').keyup(function () {
            telefonoValid();
        });

        //$('#<=txtPaginaWeb.ClientID %>').keyup(function () {
        //    paginaWebValid();
        //});

        function emailValid() {
            var email = $("#<%=TxtEmail.ClientID%>").val();
            var emailEdit = $("#<%=txtCorreoEdit.ClientID%>").val();
            if (email.length < 5) {
                if ($("#emailimg").length > 0)
                    $("#emailimg").remove();

                if ($("#emailimgError").length > 0)
                    $("#emailimgError").remove();
            }
            else {                
                if ($("#emailerror").length > 0)
                    $("#emailerror").remove();

                if ($("#emailimg").length > 0)
                    $("#emailimg").remove();

                if ($("#emailimgError").length > 0)
                    $("#emailimgError").remove();

                // Expresion regular para validar el correo
                var regex = /[\w-\.]{1,}@([\w-]{1,}\.)*([\w-]{1,}\.)[\w-]{1,4}/;

                // Se utiliza la funcion test() nativa de JavaScript
                if (regex.test(email.trim())) {
                    $(".email1 input").removeClass('error');
                    $(".email1 input").addClass('valid');
                    if (emailEdit.trim() != email.trim())
                        self.validateUsuario(2, true); //true = usuarios activos, false = usuarios inactivos 2=email
                } else {
                    $(".email1 input").attr('class', 'form-control error');
                    $(".email1").append(labelError);
                }
            }
        }

        function telefonoValid() {
            var tel = $("#<%=TxtTelefono.ClientID%>").val();
            var telEdit = $("#<%=txtTelefonoEdit.ClientID%>").val();

            if (tel.length < 7) {
                if ($("#telefonoimg").length > 0) {
                    $("#telefonoimg").remove();
                }
                if ($("#telefonoimgError").length > 0) {
                    $("#telefonoimgError").remove();
                }
            }
            else {
                if ($("#telefonoerror").length > 0)
                    $("#telefonoerror").remove();

                if ($("#telefonoimg").length > 0)
                    $("#telefonoimg").remove();

                if ($("#telefonoimgError").length > 0)
                    $("#telefonoimgError").remove();

                if (telEdit.trim() != tel.trim())
                    self.validateUsuario(3, true);//true = usuarios activos, false = usuarios inactivos 3= telefono
            }
        }

        setTimeout(function () {
            telefonoValid();
            emailValid();
        }, 2200);

        function endRequests(sender, args) {

        }
    </script>
    </div>
</asp:Content>
