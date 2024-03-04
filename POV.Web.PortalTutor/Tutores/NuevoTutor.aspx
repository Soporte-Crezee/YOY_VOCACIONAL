<%@ Page Title="" Language="C#" MasterPageFile="TutoresPage.Master" AutoEventWireup="true"
    CodeBehind="NuevoTutor.aspx.cs" Inherits="POV.Web.PortalTutor.Tutores.NuevoTutor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.tutores.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.tutores.js")%>" type="text/javascript"></script>
 
    <script type="text/javascript">

        $(document).ready(initPage);

        function initPage() {
            $('#frmMain').validator();
            $('.button').button();
            rules = {
                '<%=txtNombre.UniqueID %>': { required: true, maxlength: 30 },
			    '<%=txtPrimerApellido.UniqueID %>': { required: true, maxlength: 20 },
			    '<%=txtSegundoApellido.UniqueID %>': { required: false, maxlength: 20 },
			    '<%=CbSexo.UniqueID %>': { required: true, minlength: 1 },
			    '<%=txtCorreoElectronico.UniqueID %>': { required: true, maxlength: 50 },
			    '<%=txtNombreUsuario.UniqueID %>': { required: true, minlength: 6, maxlength: 50 },
			    '<%=txtPassword.UniqueID %>': { required: true, minlength: 6, maxlength: 50 },
			    '<%=txtConfirmarPassword.UniqueID %>': { required: true, minlength: 6, maxlength: 50 },
			};

            $('#frmMain').validate(
			    {
			        rules: rules,
			        submitHandler: function (form) {
			            $('.main').block();
			            form.submit();
			        }
			    });
        }

        $(function () {
            //alertify.alert().set('message', 'This is a new message!').show();
            //alertify.error("Proceso de registro.");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fondo_gris" style="min-width: 20px; padding: 20px;background:#77aea9;">
         <div class="col-xs-offset-1"><h1 class="title_bienvenido txt-helvetica">REGISTRO</h1></div>		
		<fieldset>
			<!--Nombre-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" data-required-error="Dato requerido" Font-Size="Large"  MaxLength="30" TabIndex="1" placeholder="Nombre*" required=""></asp:TextBox>
                </div>
			</div>

			<!--Primer apellido-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtPrimerApellido" CssClass="form-control" Font-Size="Large"  MaxLength="20" TabIndex="2" placeholder="Primer apellido*" required=""></asp:TextBox>
				</div>
			</div>

			<!--Segundo apellido-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtSegundoApellido" CssClass="form-control" Font-Size="Large" MaxLength="20" TabIndex="3" placeholder="Segundo apellido"></asp:TextBox>
				</div>
			</div>

			<!--Sexo-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:DropDownList ID="CbSexo" runat="server" TabIndex="4" CssClass="form-control" Font-Size="Large" >
						<asp:ListItem Value="">Sexo*</asp:ListItem>
						<asp:ListItem Value="True">Hombre</asp:ListItem>
						<asp:ListItem Value="False">Mujer</asp:ListItem>
					</asp:DropDownList>
				</div>
			</div>

			<!--Correo electrónico-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtCorreoElectronico" Font-Size="Large"  MaxLength="50" TabIndex="5" CssClass="form-control" placeholder="Correo electrónico*" required=""></asp:TextBox>
				</div>
			</div>
		</fieldset>
		<%--<hr class="hrule" />--%>
		<fieldset>
			<!--Nombre de usuario-->
			<div class="col-xs-12 form-group username">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtNombreUsuario" Font-Size="Large"  MaxLength="50" TabIndex="6" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Usuario*" required=""></asp:TextBox>
				</div>
			</div>

			<!--Password-->
			<div class="col-xs-12 form-group">
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtPassword" Font-Size="Large"  MaxLength="50" TabIndex="7" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Contraseña*" required=""></asp:TextBox>
				</div>
			</div>

			<!--Confirmar Password-->
			<div class="col-xs-12 form-group">				
				<div class="col-sm-12">
					<asp:TextBox runat="server" ID="txtConfirmarPassword"  Font-Size="Large"  MaxLength="50" TabIndex="8" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" placeholder="Confirmar contraseña*" required=""></asp:TextBox>
				</div>
			</div>
			<%--<div class="line"></div>--%>
		</fieldset>

        <!-- botones guardar y cancelar -->
        <div class="col-xs-12" style="background:#77aea9;height:80px;">
            <div class="col-xs-6 col-md-6">
                <h4>
                    <asp:HyperLink ID="HyperLink1" Text="Volver" TabIndex="10" runat="server" Width="150px" style="color:#f1f1f1;" NavigateUrl="../Auth/Login.aspx" CssClass="left"></asp:HyperLink>
                </h4>
            </div>
            <div class="col-xs-6 col-md-6">
                <asp:Button runat="server" ID="btnGuardar" TabIndex="9" Text="Guardar" Width="150px" CssClass="btn-entrar btn-md btn-block" OnClick="btnGuardar_Click" />
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

        function loadControls(sender, args) {

            CoreTutores.ValidacionesTutor.init({
                btnguardar: $('#<%=btnGuardar.ClientID %>'),
			    buttons: $('.boton'),
			    txttutornombre: $('#<%=txtNombre.ClientID %>'),
				txtusername: $('#<%=txtNombreUsuario.ClientID %>'),
			    dialog: $('#dialogquestion'),
			    textdialog: $('#dialogquestion').find('#dialogtext')
			});
        }

        setTimeout(function () {
            var valid = $('#<%=txtNombre.ClientID %>').val().length > 0 &&
                    $('#<%=txtPrimerApellido.ClientID %>').val().length > 0 &&
	                $('#<%=CbSexo.ClientID %>').val().length > 0 &&
	                $('#<%=txtCorreoElectronico.ClientID %>').val().length > 0;

	        if (!valid) $('#<%=txtNombreUsuario.ClientID %>').val(null);

	        $('#<%=txtPassword.ClientID %>').val(null);
	        $('#<%=txtConfirmarPassword.ClientID %>').val(null);
	    }, 600);

	    setTimeout(function () {
	        if ($('#<%=txtNombreUsuario.ClientID %>').val().length >= 6) {
	            verificarUsuario();
	        }
	    }, 1000);

	    $('#<%=txtNombreUsuario.ClientID %>').blur(function () {
            verificarUsuario();
        });

        function verificarUsuario() {
            var self = CoreTutores.ValidacionesTutor;
            var user = $('#<%=txtNombreUsuario.ClientID %>').val();
	        if (user != "" && user.length >= 6)
	            self.validateUsuario();
	        else {
	            var id = $("#userimg").length;
	            if (id > 0) {
	                $("#userimg").remove();
	            }
	        }
        }

        $('#<%=txtNombreUsuario.ClientID %>').keyup(function () {
            var user = $(this).val();
            if (user.length < 6) {
                var id = $("#userimg").length;
                if (id > 0) {
                    $("#userimg").remove();
                }
            }
        });

        function endRequests(sender, args) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent2" runat="server">
    <div class="col-xs-3"></div>
    <div class="col-xs-5"><asp:Image ID="Image1" runat="server" ImageAlign="Left" style="max-height:550PX;" CssClass="" ImageUrl="~/Images/tutoreslogin.png" Height="100%" /></div>
    <div class="col-xs-4"></div>
</asp:Content>
