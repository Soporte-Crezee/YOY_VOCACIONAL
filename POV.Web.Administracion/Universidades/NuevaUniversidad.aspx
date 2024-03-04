<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NuevaUniversidad.aspx.cs" Inherits="POV.Web.Administracion.Universidades.NuevaUniversidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $( "#tabs" ).tabs();
        }); 

        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();            
            validateUniversidad();

            var options = {
                'maxCharacterSize': 500,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 40,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=txtDireccion.ClientID%>').textareaCount(options);
        }

        function validateUniversidad() {
			       	
            var rules = {                     
                <%=txtNombreUniversidad.UniqueID %> :{required:true, maxlength:250},
                <%=txtDireccion.UniqueID %> :{required:true, maxlength:500},
                <%=txtTelefono.UniqueID %>:{required:true,  maxlength:20},
                <%=txtCorreo.UniqueID %>:{required:true,maxlength:100},
                <%=txtNombreUsuario.UniqueID %>:{required:true,maxlength:50, minlength:6},
                <%=txtClaveEscolar.UniqueID %>:{required:true,maxlength:50},
                <%=CbPais.UniqueID %>:{required:true,minlength:1},
                <%=CbEstado.UniqueID %>:{required:true,minlength:1},
                <%=CbMunicipio.UniqueID %>:{required:true,minlength:1},
                <%=ddlNivelEscolar.UniqueID %>:{required:true,minlength:1}
            };
                
            jQuery.extend(jQuery.validator.messages, {
                required: jQuery.validator.format("Este campo es obligatorio")
            });

            $("#frmMain").validate({
                rules: rules,
                submitHandler: function(form) {
                    var errores = '';

                    if ($("#telefonoimgError").length>0) {
                        errores+="teléfono";
                    }

                    if ($("#emailimgError").length>0) {
                        if(errores.length>0) errores+=", email";
                        else errores+="email";    
                    }

                    if ($("#usuarioimgError").length>0) {
                        if(errores.length>0) errores+=", usuario";
                        else errores+="usuario";                        
                    }
                
                    if (errores.length <= 0) {
                        DocumentBlockUI();
                        form.submit();
                    }
                    else{
                        alert("Los siguientes datos no se encuentran disponibles para su uso: "+errores+".");
                        telefonoValid();
                        emailValid();
                        userValid(); 
                    }
                }
            });

            $("#<%=txtTelefono.ClientID%>").on("keypress", function (e) {
                var keynum = window.event ? window.event.keyCode : e.which;
                if ((keynum == 8) || (keynum == 46))
                    return true;
                return /\d/.test(String.fromCharCode(keynum));
            });
        }
    </script>

    <style type="text/css">
        .text_Area {
            font-family: Helvetica;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <h1 class="tBienvenida">
            <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Universidades/BuscarUniversidad.aspx" CssClass="tBienvenidaLabel">
            Volver
            </asp:HyperLink>
            <%-- CG --%>
        / Registrar escuela
        </h1>

        <div class="col-xs-12 col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
                </div>
                <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n de escuela</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNombreUniversidad" Text="Nombre *" ToolTip="Nombre universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtNombreUniversidad" TabIndex="1" MaxLength="250" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblTelefono" Text="Tel&eacute;fono *" ToolTip="Tel&eacute;fono universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtTelefono" TabIndex="2" MaxLength="20" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtTelefonoEdit" Style="display: none;"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblClaveEscolar" Text="Clave *" ToolTip="Clave escolar" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtClaveEscolar" TabIndex="3" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNivelEscolar" Text="Nivel *" ToolTip="Nivel escolar" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ID="ddlNivelEscolar" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Seleccionar"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Básico"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Medio"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Superior"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblDireccion" Text="Dirección *" ToolTip="Dirección universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtDireccion" TextMode="MultiLine" Rows="4" Width="230px" TabIndex="3" MaxLength="500" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n de usuario</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblCorreo" Text="Correo electr&oacute;nico *" ToolTip="Correo universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-7 email1">
                                    <asp:TextBox runat="server" ID="txtCorreo" TextMode="Email" TabIndex="5" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido" autocomplete="off"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtCorreoEdit" Style="display: none;"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                                <div class="col-sm-1 email"></div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNombreUsuario" Text="Usuario" ToolTip="Nombre de usuario universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-7 usuario1">
                                    <asp:TextBox runat="server" ID="txtNombreUsuario" TabIndex="7" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 usuario"></div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblSiglas" Text="Nombre corto" ToolTip="Nombre corto universidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtSiglas" TabIndex="6" MaxLength="15" CssClass="form-control"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n de ubicaci&oacute;n</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblPais" Text="Pa&iacute;s" ToolTip="Pa&iacute;s" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:UpdatePanel ID="updPais" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbPais" AutoPostBack="True" TabInde="8" OnSelectedIndexChanged="CbPais_SelectedIndexChanged" CssClass="form-control" required="" data-required-error="Dato requerido">
                                            </asp:DropDownList>                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblMunicipio" Text="Cuidad" ToolTip="Cuidad" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:UpdatePanel ID="updMunicipio" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbMunicipio" AutoPostBack="True" TabIndex="9" CssClass="form-control" required="" data-required-error="Dato requerido">
                                            </asp:DropDownList>                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">

                            <div class="form-group">
                                <asp:Label runat="server" ID="lblEstado" Text="Estado" ToolTip="Estado" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:UpdatePanel ID="updEstado" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="CbEstado" AutoPostBack="True" TabIndex="10" OnSelectedIndexChanged="CbEstado_SelectedIndexChanged" CssClass="form-control" required="" data-required-error="Dato requerido">
                                            </asp:DropDownList>                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-8 col-sm-7 col-md-7 col-xs-offset-3 col-sm-offset-5 col-md-offset-5">
                    <div class="opciones_formulario">
                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-green btn-md" OnClick="btnGuardar_Click" />
                        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarUniversidad.aspx" CssClass="btn-cancel"></asp:HyperLink>
                    </div>
                </div>
                <asp:TextBox runat="server" ID="TextBox2" Style="display: none;"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtNombreUsuarioEdit" Style="display: none;"></asp:TextBox>
            </div>
            <div id="dialogquestion" title="YOY">
                <p id="dialogtext">
                </p>
            </div>
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_pageLoaded(loadControls);
                prm.add_endRequest(endRequests);            
            
                function loadControls(sender, args) {
                    var ddate = new Date();
                    var maxddate = new Date(ddate.getYear() - 17, -1, 1);
                    $(".boton").button();
                    validateUniversidad();
                
                    CoreUsuarios.ValidacionesUsuario.init({
                        //buttons
                        btnguardar: $('#<%=btnGuardar.ClientID %>'),
                        buttons: $('.boton'),
                    
                        //identificadores
                        uiusername: $('#<%=txtNombreUsuario.ClientID %>'),
                        uiemail: $('#<%=txtCorreo.ClientID %>'),
                        uitelefono: $('#<%=txtTelefono.ClientID %>'),

                        dialog: $('#dialogquestion'),
                        textdialog: $('#dialogquestion').find('#dialogtext')
                    });
                }	  

                var self = CoreUsuarios.ValidacionesUsuario; 

                $('#<%=txtNombreUsuario.ClientID %>').keyup(function () {
                    if(/^[a-zA-Z0-9]+([-_\.][a-zA-Z0-9]+)*[a-zA-Z0-9]*$/.test($(this).val())){
                        if($(this).val().length > 6){
                            self.validateUsuario(1, true);//true = usuarios activos, false = usuarios inactivos
                        }else{
                            $("#usuarioimg").remove();
                            $("#usuarioerror").remove();
                            var labelError =                    
                            '<label class="error" id="usuarioerror" for="MainContent_txtNombreUsuario" generated="true">' +
                            'Por favor, escribe un nombre de usuario válido' +
                            '</label>';
                    
                            $(this).addClass("novalido");                                         
                            $("#usuarioerror").remove();
                            $(this).find(".usuario1").append(labelError);
                        }
                        $(this).removeClass("novalido");	
                    }else {
                        var labelError =                    
                            '<label class="error" id="usuarioerror" for="MainContent_txtNombreUsuario" generated="true">' +
                            'Por favor, escribe un nombre de usuario válido' +
                            '</label>';
                    
                        $(this).addClass("novalido");     
                        $("#usuarioerror").remove();
                        $(this).find(".usuario1").append(labelError);
                        event.preventDefault();
                    }        
                });
            
                $('#<%=txtCorreo.ClientID %>').keyup(function () {
                    emailValid();
                });

                $('#<%=txtTelefono.ClientID %>').keyup(function () {
                    telefonoValid();
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
                    else{
                        self.validateUsuario(1, true);//true = usuarios activos, false = usuarios inactivos
                    }
                }

                function emailValid() {
                    var email = $("#<%=txtCorreo.ClientID%>").val();
                    if (email.length < 5) {
                        if ($("#emailimg").length > 0)
                            $("#emailimg").remove();
                    
                        if ($("#emailimgError").length > 0)
                            $("#emailimgError").remove();                    
                    }
                    else{

                        var labelError =
                        [
                            '<label class="error" id="emailerror" for="MainContent_txtCorreo" generated="true">',
                            'Por favor, escribe una dirección de correo válida',
                            '</label>'
                        ].join('');

                        if($("#emailerror").length>0)
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

                            self.validateUsuario(2, true);//true = usuarios activos, false = usuarios inactivos
                        } else {
                            if (!$(".email1 input").hasClass("error")) {
                                $(".email1 input").attr('class','form-control error');                    
                                $(".email1").append(labelError);
                            }
                        }
                    }
                }

                function telefonoValid() {
                    var tel = $("#<%=txtTelefono.ClientID%>").val();
                if (tel.length < 7) {
                    if ($("#telefonoimg").length > 0) {
                        $("#telefonoimg").remove();
                    }
                    if ($("#telefonoimgError").length > 0) {
                        $("#telefonoimgError").remove();
                    }
                }
                else{
                    self.validateUsuario(3, true);//true = usuarios activos, false = usuarios inactivos
                }
            }

            function endRequests(sender, args) {
                validateUniversidad();
            }
            </script>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>
    </div>
</asp:Content>
