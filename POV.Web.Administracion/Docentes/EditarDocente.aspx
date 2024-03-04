<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarDocente.aspx.cs" Inherits="POV.Web.Administracion.Docentes.EditarDocente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $( "#tabs" ).tabs();
        }); 

        function sowMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            api.Show();
            resetHiden();
        }

        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();
            $("#<%=txtFechaNacimiento.ClientID %>").datepicker({ yearRange: '-100:+0', changeMonth:true, changeYear: true, dateFormat: "dd/mm/yy" });
            
            validateDocente();
        }

        function validateDocente() {
			       	
            var rules = {
                <%=txtCurp.UniqueID %>:{required:true, maxlength:18 },        
                <%=txtNombre.UniqueID %> :{required:true, maxlength:80},
                <%=txtPrimerApellido.UniqueID %> :{required:true, maxlength:50},
                <%=txtSegundoApellido.UniqueID %>:{required:false,  maxlength:50},
                <%=txtCorreo.UniqueID %>:{required:true,maxlength:100},
                <%=txtFechaNacimiento.UniqueID %>:{required:true,maxlength:10},
                <%=CbxSexo.UniqueID %>:{required:true,minlength:1},
                <%=txtSkype.UniqueID %>:{required:true,maxlength:100},
                <%=txtCedula.UniqueID %>:{required:true,maxlength:10},
                <%=ddlNivelEstudio.UniqueID %>:{required:true,minlength:1},
                <%=txtTitulo.UniqueID %>:{required:true,maxlength:100}				  
            };

                
            jQuery.extend(jQuery.validator.messages, {
                required: jQuery.validator.format("Este campo es obligatorio")
            });


            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                },
                rules:rules
            };

            $('#frmMain').validator(validations).on('submit', function (e) {
                // handle the invalid form...
                var tab1=$("#tabs-1");
                var tab2=$("#tabs-2");
                var tab3=$("#tabs-3");

                var errores = '';

                if ($("#emailimgError").length>0) {
                    errores+="email";
                }

                if ($("#usuarioimgError").length>0) {
                    if(errores.length>0) errores += ", usuario";
                    else errores += "usuario";  
                }
                
                if (errores.length <= 0) {
                    //DocumentBlockUI();
                    //e.submit();
                    if (tab1.hasClass("has-error has-danger")) 
                        $("#ahrefTab1").click();
                    else
                        if (tab2.hasClass("has-error has-danger")) 
                            $("#ahrefTab2").click();
                        else
                            if (tab3.hasClass("has-error has-danger")) 
                                $("#ahrefTab3").click();
                    
                    return;
                }
                else{
                    setTimeout(function () {
                        sowMessage("Los siguientes datos no se encuentran disponibles para su uso: "+errores+".");
                    },1000);
                    
                    e.preventDefault();
                    emailValid();
                }
            });         
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <h1 class="tBienvenida">
            <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Docentes/BuscarDocentes.aspx" CssClass="tBienvenidaLabel"> Volver </asp:HyperLink>
            / Editar orientador
        </h1>
        <h2 style="display: none">

            <asp:Label runat="server" ID="lblSubTitulo" Text="Información de la escuela"></asp:Label>
        </h2>
        <div class="col-xs-12 col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
                </div>
                <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n de orientador</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNombre" Text="Nombre *" ToolTip="Nombre orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="80" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblPrimerApellido" Text="Primer apellido *" CssClass="col-sm-4 control-label" ToolTip="Primer apellido orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtPrimerApellido" TabIndex="2" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblSegundoApellido" Text="Segundo apellido" CssClass="col-sm-4 control-label" ToolTip="Segundo apellido orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtSegundoApellido" TabIndex="3" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblCurp" Text="CURP *" ToolTip="Curp orientador" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCurp" TabIndex="4" MaxLength="18" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblSexo" Text="Sexo *" CssClass="col-sm-4 control-label"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="CbxSexo" runat="server" TabIndex="5" CssClass="form-control" required="" data-required-error="Dato requerido">
                                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="True">HOMBRE</asp:ListItem>
                                        <asp:ListItem Value="False">MUJER</asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblFechaNacimiento" Text="Fecha nacimiento *" CssClass="col-sm-4 control-label" ToolTip="Fecha de nacimiento del orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtFechaNacimiento" TabIndex="6" MaxLength="15" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
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
                                <asp:Label runat="server" ID="lblCorreo" Text="Correo electrónico *" CssClass="col-sm-4 control-label" ToolTip="Correo electrónico del orientador"></asp:Label>
                                <div class="col-sm-7 email1">
                                    <asp:TextBox runat="server" ID="txtCorreo" TextMode="Email" TabIndex="7" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido" autocomplete="off"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtCorreoEditar" Style="display: none"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                                <div class="col-sm-1 email"></div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNombreUsuario" Text="Nombre de usuario" CssClass="col-sm-4 control-label" ToolTip="Nombre de usuario del orientador"></asp:Label>
                                <div class="col-sm-7 usuario1">
                                    <asp:TextBox runat="server" ID="txtNombreUsuario" TabIndex="8" MaxLength="100" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 usuario"></div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblSkype" Text="Usuario skype *" CssClass="col-sm-4 control-label" ToolTip="Usuario skype"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtSkype" TabIndex="9" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="Es premium" TextAlign="Right" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Datos profesionales</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblCedula" Text="C&eacute;dula profesional *" CssClass="col-sm-4 control-label" ToolTip="C&eacute;dula profesional del orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtCedula" TabIndex="11" MaxLength="10" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNivEstudio" Text="Nivel de estudio *" CssClass="col-sm-4 control-label" ToolTip="Nivel estudio orientador *"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlNivelEstudio" runat="server" TabIndex="12" CssClass="form-control" required="" data-required-error="Dato requerido">
                                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                                        <asp:ListItem Value="Maestria">Maestr&iacute;a</asp:ListItem>
                                        <asp:ListItem Value="Doctorado">Doctorado</asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblTitulo" Text="T&iacute;tulo *" CssClass="col-sm-4 control-label" ToolTip="Titulo del orientador"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox runat="server" ID="txtTitulo" TabIndex="13" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12">
                            <div id="tabs">
                                <ul>
                                    <li class="col-xs-4 col-md-4"><a id="ahrefTab1" href="#tabs-1" style="width: 100%;">Especialidad</a></li>
                                    <li class="col-xs-4 col-md-4"><a id="ahrefTab2" href="#tabs-2" style="width: 100%;">Experiencia</a></li>
                                    <li class="col-xs-4 col-md-4"><a id="ahrefTab3" href="#tabs-3" style="width: 100%;">Cursos</a></li>
                                </ul>

                                <div id="tabs-1" class="form-group">
                                    <asp:TextBox runat="server" ID="txtEspecialidades" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="14" placeholder="Especialidad *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>

                                <div id="tabs-2" class="form-group">
                                    <asp:TextBox runat="server" ID="txtExperiencia" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="15" placeholder="Experiencia *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>

                                <div id="tabs-3" class="form-group">
                                    <asp:TextBox runat="server" ID="txtCursos" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="16" placeholder="Cursos *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                    <div class="help-block with-errors"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xs-8 col-sm-7 col-md-7 col-xs-offset-3 col-sm-offset-5 col-md-offset-5">
                    <div class="opciones_formulario">
                        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-green btn-md" OnClick="btnGuardar_Click" />
                        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarDocentes.aspx" CssClass="btn-cancel"></asp:HyperLink>
                    </div>
                </div>
                <asp:TextBox runat="server" ID="txtCorreoEdit" Style="display: none;"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtNombreUsuarioEdit" Style="display: none;"></asp:TextBox>
            </div>
            <div id="dialogquestion" title="YOY">
                <p id="dialogtext">
                </p>
            </div>
        </div>
        <script type="text/javascript">

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(loadControls);
            prm.add_endRequest(endRequests);            
            
            function loadControls(sender, args) {
                var ddate = new Date();
                var maxddate = new Date(ddate.getYear() - 17, -1, 1);
                $(".boton").button();
                $("#<%=txtFechaNacimiento.ClientID %>").datepicker({ maxDate: maxddate, changeYear: true });
                validateDocente();
                
                CoreUsuarios.ValidacionesUsuario.init({
                    //buttons
                    btnguardar: $('#<%=btnGuardar.ClientID %>'),
                    buttons: $('.boton'),
                    
                    //identificadores
                    uiusername: $('#<%=txtNombre.ClientID %>'),
                    uiemail: $('#<%=txtCorreo.ClientID %>'),
                    uitelefono: $('#<%=txtSkype.ClientID %>'),
                    
                    dialog: $('#dialogquestion'),
                    textdialog: $('#dialogquestion').find('#dialogtext')
                });
            }	  

            var self = CoreUsuarios.ValidacionesUsuario; 
            
            $('#<%=txtCorreo.ClientID %>').keyup(function () {
                emailValid();
            }).blur(function () {
                emailValid();
            });

            function emailValid() {
                var email = $("#<%=txtCorreo.ClientID%>").val();
                var emailEditar = $("#<%=txtCorreoEditar.ClientID%>").val();

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
                        
                        if(emailEditar.trim() != email.trim())
                            self.validateUsuario(2, true);
                    } else {
                        $(".email1 input").attr('class','form-control error');                        
                        $(".email1").append(labelError);
                    }
                }
            }

            setTimeout(function () {
                emailValid();              
            },2200);

            function endRequests(sender, args) {

            }
        </script>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>
