<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarEvento.aspx.cs" Inherits="POV.Web.PortalUniversidad.Eventos.EditarEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/json.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.shared.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>validator.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/moment-with-locales-v2.9.0.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap-datetimepicker-v4.15.35.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();
            validateEventoUniversidad();
            var options = {
                'maxCharacterSize': 2000,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 1950,
                'displayFormat': '#left caracteres restantes de #max max.'
            };
            $('#<%=txtDescripcion.ClientID%>').textareaCount(options);
        }

        function validateEventoUniversidad() {			       	
            var rules = {
                <%=txtNombre.UniqueID %>:{required:true, maxlength:500 },        
                <%=txtDescripcion.UniqueID %> :{required:true, maxlength:2000},
                <%=txtFechaInicio.UniqueID %> :{required:true},
                <%=txtFechaFin.UniqueID %>:{required:false}
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

            setTimeout(function () {
                loadEventsCalendars();
            },800);  
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ol class="breadcrumb">
      <li>
          <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Eventos/BuscarEventos.aspx" style="font-size:30px !important;" CssClass="">Volver</asp:HyperLink>
      </li>
      <li style="font-size:30px !important;">Editar evento</li>
    </ol>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n del evento
        </div>
        <div class="panel-body">
            <asp:TextBox runat="server" ID="txtFechaInicioTmp" hidden="hidden"></asp:TextBox>
            <asp:TextBox runat="server" ID="txtFechaFinTmp" hidden="hidden"></asp:TextBox>

            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre*" CssClass="col-sm-2 control-label" ToolTip="Nombre del evento"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="500" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblFechaInicio" Text="Fecha inicio*" CssClass="col-sm-2 control-label" ToolTip="Fecha de inicio del evento"></asp:Label>
                    <div class="col-sm-4">
                        <div class="input-group date" id="datetimepickerFechaInicio">
                            <asp:TextBox runat="server" ID="txtFechaInicio" TabIndex="2" MaxLength="15" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                        <div class="help-block with-errors"></div>
                    </div> 
                    <asp:Label runat="server" ID="lblFechaFin" Text="Fecha fin*" CssClass="col-sm-2 control-label" ToolTip="Fecha de fin del evento"></asp:Label>
                    <div class="col-sm-4">
                        <div class="input-group date" id="datetimepickerFechaFin">
                            <asp:TextBox runat="server" ID="txtFechaFin" TabIndex="3" MaxLength="15" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblDescripcion" Text="Descripción *" CssClass="col-sm-2 control-label" ToolTip="Descripción del evento"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" MaxLength="2000" Rows="15" TabIndex="4" CssClass="form-control" required="" data-required-error="Dato requerido"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="text-center">
        <div class="opciones_formulario">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-green btn-md" TabIndex="5" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="6" class="btn btn-cancel btn-md" OnClick="btnCancelar_Click" />
        </div>
    </div>
    <div class="">
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_pageLoaded(loadControls);
                prm.add_endRequest(endRequests);

                function loadControls(sender, args) {
                    $(".boton").button();
                    setTimeout(function () {
                        loadEventsCalendars();
                    },800);                    
                    validateEventoUniversidad();
                }
                
                var loadEventsCalendars=function () {
                    var fechaInicioTmp = $('#<%=txtFechaInicioTmp.ClientID %>').val();
                    var fechaFinTmp = $('#<%=txtFechaFinTmp.ClientID %>').val();
                   
                    var mymoment = moment(new Date());
                    var minDate =  moment().date(mymoment.date()).month(mymoment.month()).year(mymoment.year());

                    $('#<%=txtFechaInicio.ClientID %>').datetimepicker({
                        format: 'DD/MM/YYYY',
                        locale: 'es',
                        defaultDate: $.datepicker.parseDate('dd/mm/yy', fechaInicioTmp)
                    }).on("dp.change", function (e) {
                        $('#<%=txtFechaFin.ClientID %>').data("DateTimePicker").minDate(e.date);

                        if ($.datepicker.parseDate('dd/mm/yy', e.date.date()+"/"+e.date.month()+"/"+e.date.year()) < $.datepicker.parseDate('dd/mm/yy', mymoment.date()+"/"+mymoment.month()+"/"+mymoment.year())) {
                            var api = new MessageApi();
                            var text = "La fecha de inicio no puede ser menor a la fecha actual";
                            api.CreateMessage(text, "ERROR");
                            if($(this).val()!=fechaInicioTmp)
                                api.Show();
                            $(this).val(fechaInicioTmp);
                        }
                    }).on("focusout",function (e) {
                        validarFechas("fechaInicio");
                    });
                    
                    $('#<%=txtFechaFin.ClientID %>').datetimepicker({
                        format: 'DD/MM/YYYY',
                        locale: 'es'              
                    }).on("dp.change", function (e) {
                        $('#<%=txtFechaInicio.ClientID %>').data("DateTimePicker").maxDate(e.date);
                        if ($.datepicker.parseDate('dd/mm/yy', e.date.date()+"/"+e.date.month()+"/"+e.date.year()) < $.datepicker.parseDate('dd/mm/yy', mymoment.date()+"/"+mymoment.month()+"/"+mymoment.year())) {
                            var api = new MessageApi();
                            var text = "La fecha fin no puede ser menor a la fecha actual";
                            api.CreateMessage(text, "ERROR");
                            if($(this).val()!=fechaFinTmp)
                                api.Show();
                            $(this).val(fechaFinTmp);
                        }
                    }).on("focusout",function (e) {
                        validarFechas("fechaFin");                        
                    });
                    
                    $("#datetimepickerFechaInicio span").click(function (e) {
                        $('#<%=txtFechaInicio.ClientID %>').data("DateTimePicker").show()
                    });
                    $("#datetimepickerFechaFin span").click(function (e) {
                        $('#<%=txtFechaFin.ClientID %>').data("DateTimePicker").show()
                    });

                    $('#<%=txtFechaInicio.ClientID %>').change(function () {
                        validarFechas("fechaInicio");
                    });

                    $('#<%=txtFechaFin.ClientID %>').change(function () {
                        validarFechas("fechaFin");
                    });

                }

                var validarFechas = function (optionVal) {
                    var fInicio = $('#<%=txtFechaInicio.ClientID %>');
                    var fFin= $('#<%=txtFechaFin.ClientID %>');
                
                    if(fInicio.length>0){
                        var api = new MessageApi();
                        try {
                            if ($.datepicker.parseDate('dd/mm/yy', fInicio.val()) > $.datepicker.parseDate('dd/mm/yy', fFin.val())) {
                                api.CreateMessage("La fecha "+(optionVal=="fechaInicio"?"de inicio":"fin")+" no puede ser "+(optionVal=="fechaInicio"?"mayor":"menor")+" a la fecha "+(optionVal=="fechaInicio"?"fin":"de inicio"), "ERROR");
                                api.Show();
                                if(optionVal=="fechaInicio")
                                    fInicio.val('');
                                else
                                    fFin.val('');
                            }
                        } catch (e) {
                            api.CreateMessage("Error con el formato de fecha", "ERROR");
                            api.Show();
                            if(optionVal=="fechaInicio")
                                fInicio.val('');
                            else
                                fFin.val('');
                        }
                    }
                }

                function endRequests(sender, args) {

                }
            </script>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
    </div>
</asp:Content>
