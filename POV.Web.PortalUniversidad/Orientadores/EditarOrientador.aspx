<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarOrientador.aspx.cs" Inherits="POV.Web.PortalUniversidad.Orientadores.EditarOrientador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.usuarios.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/moment-with-locales-v2.9.0.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap-datetimepicker-v4.15.35.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $('#tabs a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            })
        });
        function sowMessage(text) {
            var api = new MessageApi();
            api.CreateMessage(text, "ERROR");
            api.Show();
            resetHiden();
        }
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
        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();                        
            validateOrientador();
        }

        function validateOrientador() {
			       	
            var rules = {
                <%=txtCurp.UniqueID %>:{required:true, maxlength:18 },        
                <%=txtNombre.UniqueID %> :{required:true, maxlength:80},
                <%=txtPrimerApellido.UniqueID %> :{required:true, maxlength:50},
                <%=txtSegundoApellido.UniqueID %>:{required:false,  maxlength:50},
                <%=txtNombreUsuario.UniqueID %>:{required:true,maxlength:50, minlength:6},
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
                    errores+="correo electrónico";
                }

                if ($("#usuarioimgError").length>0) {
                    if(errores.length>0) errores += ", nombre de usuario";
                    else errores += "nombre de usuario";  
                }
                
                if (errores.length <= 0) {
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
    <ol class="breadcrumb">
      <li>
          <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Orientadores/BuscarOrientadores.aspx" style="font-size:30px !important;" CssClass="">Volver</asp:HyperLink>
      </li>
      <li style="font-size:30px !important;">Editar orientador</li>
    </ol>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n de orientador
        </div>
        <div class="panel-body">
            <div class="form-horizontal" style="padding-top:20px;">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre*" CssClass="col-sm-2 control-label" ToolTip="Nombre orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="80" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <asp:Label runat="server" ID="lblPrimerApellido" Text="Primer apellido*" CssClass="col-sm-2 control-label" ToolTip="Primer apellido orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtPrimerApellido" TabIndex="2" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSegundoApellido" Text="Segundo apellido" CssClass="col-sm-2 control-label" ToolTip="Segundo apellido orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtSegundoApellido" TabIndex="3" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" ID="lblCurp" Text="CURP *" ToolTip="Curp orientador" CssClass="col-sm-2 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCurp" TabIndex="4" MaxLength="18" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSexo" Text="Sexo *" CssClass="col-sm-2 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="CbxSexo" runat="server" TabIndex="5" CssClass="form-control" required="" data-required-error="Dato requerido">
                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="True">HOMBRE</asp:ListItem>
                            <asp:ListItem Value="False">MUJER</asp:ListItem>
                        </asp:DropDownList>
                        <div class="help-block with-errors"></div>
                    </div>
                    <asp:Label runat="server" ID="lblFechaNacimiento" Text="Fecha nacimiento*" CssClass="col-sm-2 control-label" ToolTip="Fecha de nacimiento del orientador"></asp:Label>
                    <div class="col-sm-4">
                        <div class="input-group date" id="datetimepickerFechNacimiento">
                            <asp:TextBox runat="server" ID="txtFechaNacimiento" TabIndex="6" MaxLength="15" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
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
            <div class="form-horizontal" style="padding-top:20px;">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblCorreo" Text="Correo electrónico *"  CssClass="col-sm-2 control-label" ToolTip="Correo electrónico del orientador"></asp:Label>
                    <div class="col-sm-3 email1">
                        <asp:TextBox runat="server" ID="txtCorreo" TextMode="Email" TabIndex="7" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="col-sm-1 email"></div>
                    <asp:Label runat="server" ID="lblNombreUsuario" Text="Nombre de usuario*" CssClass="col-sm-2 control-label" ToolTip="Nombre de usuario del orientador"></asp:Label>
                    <div class="col-sm-3 usuario1">
                        <asp:TextBox runat="server" ID="txtNombreUsuario" TabIndex="8" MaxLength="100" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 usuario"></div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSkype" Text="Usuario skype*" CssClass="col-sm-2 control-label" ToolTip="Usuario skype"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtSkype" TabIndex="9" MaxLength="50" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Datos profesionales
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblCedula" Text="C&eacute;dula profesional*" CssClass="col-sm-2 control-label" ToolTip="C&eacute;dula profesional del orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCedula" TabIndex="11" MaxLength="10" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                    <asp:Label runat="server" ID="lblNivEstudio" Text="Nivel de estudio" CssClass="col-sm-2 control-label" ToolTip="Nivel estudio orientador *"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlNivelEstudio" runat="server" TabIndex="12" CssClass="form-control" required="" data-required-error="Dato requerido">
                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="Maestria">Maestr&iacute;a</asp:ListItem>
                            <asp:ListItem Value="Doctorado">Doctorado</asp:ListItem>
                        </asp:DropDownList>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblTitulo" Text="T&iacute;tulo*" CssClass="col-sm-2 control-label" ToolTip="Titulo del orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtTitulo" TabIndex="13" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-10 col-md-offset-1 col-lg-10 col-lg-offset-1">
                    <div>
                        <ul class="nav nav-tabs" role="tablist" id="tabs">
                            <li role="presentation" class="active">
                                <a href="#especialidad" aria-controls="especialidad" role="tab" data-toggle="tab">Especialidad</a>
                            </li>
                            <li role="presentation">
                                <a href="#experiencia" aria-controls="experiencia" role="tab" data-toggle="tab">Experiencia</a>
                            </li>
                            <li role="presentation">
                                <a href="#cursos" aria-controls="cursos" role="tab" data-toggle="tab">Cursos</a>
                            </li>
                        </ul>
                    </div>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="especialidad">
                            <asp:TextBox runat="server" ID="txtEspecialidades" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="14" placeholder="Especialidad *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <div class="help-block with-errors"></div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="experiencia">
                            <asp:TextBox runat="server" ID="txtExperiencia" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="15" placeholder="Experiencia *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <div class="help-block with-errors"></div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="cursos">
                            <asp:TextBox runat="server" ID="txtCursos" TextMode="MultiLine" MaxLength="200" Rows="30" TabIndex="16" placeholder="Cursos *" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <div class="help-block with-errors"></div>
                        </div>     
                    </div>
                        </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="">
        <div class="text-center">
            <div class="form-group">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-cancel btn-md" TabIndex="17" OnClick="btnGuardar_Click" Style="display: none" />
                <asp:Button ID="btnCancelarPerfil" runat="server" Text="Aceptar" TabIndex="18" class="btn btn-green btn-md" OnClick="btnCancelarPerfil_Click" />
            </div>
            <div class="form-group">
                <asp:TextBox runat="server" ID="txtCorreoEdit" Style="display: none;"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtNombreUsuarioEdit" Style="display: none;"></asp:TextBox>
            </div>
        </div>
        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(loadControls);
            prm.add_endRequest(endRequests);            
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
            function loadControls(sender, args) {
                $(".boton").button();
                validateOrientador();
                
                CoreUsuarios.ValidacionesUsuario.init({
                    //buttons
                    btnguardar: $('#<%=btnGuardar.ClientID %>'),
                    buttons: $('.boton'),
                    
                    //identificadores
                    uiusername: $('#<%=txtNombreUsuario.ClientID %>'),
                    uiemail: $('#<%=txtCorreo.ClientID %>'),
                    uitelefono: $('#<%=txtSkype.ClientID %>'),
                    uiuniversidadid: '',

                    dialog: $('#dialogquestion'),
                    textdialog: $('#dialogquestion').find('#dialogtext')
                });
            }	  

        var self = CoreUsuarios.ValidacionesUsuario; 
            
        $("#MainContent_txtSkype , #MainContent_txtCedula , #MainContent_txtTitulo").keypress(function (event) {                                
            //console.log(event.keyCode);
            if(!/[`´?!¿¡*%&¨ #~,<>;':"/[\]|{}()=+]/.test(String.fromCharCode(event.keyCode))){
            }else{                
                event.preventDefault();
            }
        });

        $('#<%=txtCorreo.ClientID %>').keyup(function () {
            emailValid();
        });

        function emailValid() {
            var email = $("#<%=txtCorreo.ClientID%>").val();
            var emailEdit = $("#<%=txtCorreoEdit.ClientID%>").val();

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

                    if(email!=emailEdit)
                        self.validateUsuario(2, true);//true = usuarios activos, false = usuarios inactivos
                } else {
                    $(".email1 input").attr('class','error form-control');                    
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
