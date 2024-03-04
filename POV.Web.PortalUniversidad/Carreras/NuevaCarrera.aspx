<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NuevaCarrera.aspx.cs" Inherits="POV.Web.PortalUniversidad.Carreras.NuevaCarrera" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();
 
            validateCarrera();

            var options = {
                'maxCharacterSize': 254,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 244,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=txtDescripcion.ClientID%>').textareaCount(options);
        }

        function validateCarrera() {
            var rules = {
                <%=txtNombre.UniqueID %>:{required:true, maxlength:100 },        
                <%=txtDescripcion.UniqueID %> :{required:true, maxlength:254},
                <%=ddlAreaConocimiento.UniqueID %>:{required:true,minlength:1}

            };
                
            jQuery.extend(jQuery.validator.messages, {
                minlength: jQuery.validator.format("Este campo es obligatorio")
            });

            validations = {
                feedback: {
                    success: 'glyphicon-ok',
                    error: 'glyphicon-remove'
                },
                rules: rules
            };

            $('#frmMain').validator(validations).on('submit', function (e) {
                //submit...
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="tBienvenida">
        <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Carreras/VincularCarreras.aspx" CssClass="tBienvenidaLabel">
                Volver
        </asp:HyperLink>
        ► <asp:Label ID="lblAccion" runat="server" Text="Registrar" CssClass="tBienvenidaLabel" ></asp:Label> carrera
    </h1>
    <div class="col-xs-12 col-md-12">
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
            </div>
            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n de la carrera</div>
                <div class="col-xs-12 container_busqueda_general ui-widget-content">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblNombre" Text="Nombre *" CssClass="col-sm-4 control-label" ToolTip="Nombre de la carrera"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="100" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblAreaConocimiento" Text="Área de conocimiento *" CssClass="col-sm-4 control-label" ToolTip="Área de conocimiento"></asp:Label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlAreaConocimiento" runat="server" CssClass="form-control" required="" data-required-error="Dato requerido">
                                    <asp:ListItem Value="">Seleccionar área de conocimiento</asp:ListItem>
                                </asp:DropDownList>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblDescripcion" Text="Descripción *" CssClass="col-sm-2 control-label" ToolTip="Descripción del evento"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" MaxLength="1000" Rows="15" TabIndex="4" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-8 col-sm-7 col-md-7 col-xs-offset-3 col-sm-offset-5 col-md-offset-5">
                <div class="opciones_formulario">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-green btn-md" TabIndex="5" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="6" class="btn btn-cancel btn-md" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-1"></div>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(loadControls);
            prm.add_endRequest(endRequests);

            function loadControls(sender, args) {
                var ddate = new Date();
                var maxddate = new Date(ddate.getYear() - 17, -1, 1);
                $(".boton").button();
                validateCarrera();
            }

            function endRequests(sender, args) {
            }
        </script>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>
