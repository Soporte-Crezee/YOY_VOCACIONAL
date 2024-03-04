<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalGrupo/PortalGrupo.master"
    AutoEventWireup="true" CodeBehind="MiDocente.aspx.cs" Inherits="POV.Web.PortalSocial.PortalGrupo.MiDocente" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/")%>fullcalendar.css" rel="stylesheet" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>autoresize.jquery.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.textareaCounter.plugin.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.publicaciones.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reporteabuso.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/moment.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/fullcalendar.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/es.js")%>" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.orientacionvocacional.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.orientacionvocacional.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.publicaciones.mi.docente.js" type="text/javascript"></script>
    <style type="text/css">
        h3.ui-accordion-header.ui-corner-top.ui-accordion-header-collapsed.ui-corner-all.ui-state-default.ui-accordion-icons,
        h3.ui-accordion-header.ui-corner-top.ui-state-default.ui-accordion-icons.ui-accordion-header-active.ui-state-active,
        h3.ui-accordion-header.ui-corner-top.ui-state-default.ui-accordion-icons.ui-state-focus {
            background-color: #ff9e19 !important;
            border-color: #FF9E19 !important;
            color: #FFF !important;
            font-family: Roboto-Bold !important;
        }

        ul.nav.nav-tabs.ui-tabs-nav.ui-corner-all.ui-helper-reset.ui-helper-clearfix.ui-widget-header {
            background: #f2f2f2 !important;
            border:none !important;
        }
        li.ui-tabs-tab.ui-corner-top.ui-state-default.ui-tab.ui-tabs-active.ui-state-active.active,
        li.ui-tabs-tab.ui-corner-top.ui-state-default.ui-tab.ui-tabs-active.ui-state-active.active a {
            border: 1px solid #05AED9 !important;
            background: #05AED9 !important;
            color: #FFF !important;
        }
        td.fc-head-container,
        th.fc-week-number,
        th.fc-day-header,
        th.fc-axis.fc-week-number,
        th.fc-day-header {
            border-color: #05AED9 !important;
            background: #05AED9 !important;
        }
        button.fc-today-button.ui-button:active,
        button.fc-month-button.ui-state-active,
        button.fc-month-button.ui-button:active,
        button.fc-agendaWeek-button.ui-state-active,
        button.fc-agendaWeek-button.ui-button:active,
        button.fc-agendaDay-button.ui-state-active,
        button.fc-agendaDay-button.ui-button:active,
        button.fc-prev-button.ui-button:active,
        button.fc-next-button.ui-button:active {
            background: #ff9e19 !important;
            border-color: #ff9e19 !important;
        }

        button.fc-today-button.ui-button:focus,
        button.fc-month-button.ui-button:focus,
        button.fc-agendaWeek-button.ui-button:focus,
        button.fc-agendaDay-button.ui-button:focus,
        button.fc-prev-button.ui-button:focus,
        button.fc-next-button.ui-button:focus,
        button.fc-listMonth-button.ui-button:focus {
            outline-color: transparent !important;
        }

        td.fc-list-item-title,
        td.fc-list-item-time {
            font-size: 16px !important;
        }

        .fc-content span.fc-title {
            word-wrap: break-word;
            display: inherit;
        }

    </style>
    <script type="text/javascript">
        $(document).ready(initPage);

        function showMessage(text, redirection, typeMessage) {
            $("#txtRedirect").val((typeof redirection !== "undefined" && redirection != '') ? "#" : "MiDocente.aspx?u=" + $("#<%= hdnUsuarioSocialID.ClientID%>").val());
            var api = new MessageApi();
            var link = (typeof redirection !== "undefined" && redirection != '') ? "#" : "MiDocente.aspx?u=" + $("#<%= hdnUsuarioSocialID.ClientID%>").val();
            if (typeof typeMessage !== "undefined") {
                api.CreateMessage(text, typeMessage, link);
                api.Show();
                resetHiden();
            }
            else {
                api.CreateMessage(text, "INFO", link);
                api.Show();
                resetHiden();
            }            
        }

        var cargaCompleta = false;
        var cargaCompletaEvent = false;
        var eventos = [{ eventcalendarid: undefined }];
        var businessHours =
                [ // specify an array instead
                    { configcalendarid: undefined }
                ];

        var minTimeHL = '';
        var maxTimeFL = '';

        function initPage() {
           

            var accion = false;
            var eventData = {};
            var dateCalendar = '';
            var openModal = true;

            var usuarioid = parseInt($('#<%=hdnSessionUsuarioOrientadorID.ClientID%>').val());
            var configcalendar = {};
            var dto = {};
            dto.usuarioid = usuarioid;
            configcalendar.dto = dto;

            var eventcalendar = {};
            var evtDto = {};
            evtDto.usuarioid = usuarioid;
            eventcalendar.dto = evtDto;

            setTimeout(function () {
                getConfigCalendarOrientador(configcalendar);
                getEventCalendarOrientador(eventcalendar);
            }, 500);

            var esperando = setInterval(statusDataCalendar, 1000);
            function statusDataCalendar() {
                var DcargaCompleta = (cargaCompleta == true && cargaCompletaEvent == true) ? true : false;
                if (DcargaCompleta) {
                    startCalendar();
                    minTimeHL = (businessHours[0] != undefined) ? businessHours[0].start : false;//'09:00:00',
                    maxTimeFL = (businessHours[1] != undefined) ? businessHours[1].end : (businessHours[0] != undefined) ? businessHours[0].end : false;//'19:00:00',
                    minTimeHL = parseInt(minTimeHL.split(':')[0]);// - 1;
                    maxTimeFL = parseInt(maxTimeFL.split(':')[0]);// - 1;
                    clearInterval(esperando);
                    myApiBlockUI.unblockContainer()
                }
            }

            var addEvento = function (start, end) {
                if (openModal) {
                    $('#txtNombreEvento').val('');//prompt('Titulo del evento:');
                    $('#txtInicio').val('');
                    $('#txtFin').val('');
                    accion = false;
                    $("#btnSolicitarOrientacion").click();
                }
                else {
                    var api = new MessageApi();
                    api.CreateMessage("La fecha no está disponible, por favor selecciona otra", "ERROR");
                    api.Show();
                }
            };

            function startCalendar() {
                $('#calendar').fullCalendar({
                    theme: true,
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay'
                    },
                    firstDay: 0, //0=dom,1=lun,2=mar,etc.
                    weekNumbers: true,
                    defaultDate: new Date(),
                    navLinks: true, // can click day/week names to navigate views
                    slotEventOverlap: false,
                    businessHours: businessHours,
                    dayClick: function (date, jsEvent, view) {
                        var daysBusiness = businessHours[0].dow;
                        var dateObject = new Date(date);
                        var daySelected = dateObject.getUTCDay();
                        var dayValidBusinesHour = true;
                        var dayValidSelected = true;
                        var today = new Date();

                        dayValidSelected = ($.datepicker.parseDate('dd/mm/yy', date.format('DD/MM/YYYY')) > $.datepicker.parseDate('dd/mm/yy', today.localeFormat("dd/MM/yyyy"))) ? true : false;
                        dayValidBusinesHour = (daysBusiness.indexOf(daySelected) >= 0) ? true : false;
                        openModal = dayValidBusinesHour && dayValidSelected;

                        if (typeof businessHours[1] !== "undefined" && (businessHours[1].start != businessHours[1].end)) {
                            var horaTrabajo1 = businessHours[0].start + " - " + businessHours[0].end;
                            var horaTrabajo2 = businessHours[1].start + " - " + businessHours[1].end;                            
                            $("#horarioTrabajo").empty()
                            .append($("<label>", { 'class': 'col-sm-4 control-label', 'text': 'Horario de trabajo' }))
                            .append($('<div>', { 'class': 'col-sm-8' }).append($('<label>', { 'text': horaTrabajo1 })))
                            .append($("<label>", { 'class': 'col-sm-4 control-label' }))
                            .append($('<div>', { 'class': 'col-sm-8' }).append($('<label>', { 'text': horaTrabajo2 })));
                        } else {
                            var horaTrabajo1 = businessHours[0].start + " - " + businessHours[0].end;
                            $("#horarioTrabajo").empty()
                            .append($("<label>", { 'class': 'col-sm-4 control-label', 'text': 'Horario de trabajo' }))
                            .append($('<div>', { 'class': 'col-sm-8' }).append($('<label>', { 'text': horaTrabajo1 })));
                        }

                        dateCalendar = '';
                        dateCalendar = date.format('YYYY-MM-DD');
                        $('#lblFecha').text(date.format('DD-MMMM-YYYY'));
                        if (typeof businessHours[1] !== "undefined" && (businessHours[1].start == businessHours[1].end)) {
                            getHoras('ddStart', minTimeHL, maxTimeFL - 2);
                            getHoras('ddEnd', minTimeHL + 1, maxTimeFL-1);
                        }else {
                            getHoras('ddStart', minTimeHL, maxTimeFL - 1);
                            getHoras('ddEnd', minTimeHL + 1, maxTimeFL);
                        }

                        $('#ddStart option[name="' + date.format('HH') + '"]').attr("selected", true);

                        var finHr = parseInt(date.format('HH')) + 1;
                        if (finHr < 10)
                            $('#ddEnd option[name="0' + finHr + '"]').attr("selected", true);
                        else
                            $('#ddEnd option[name="' + finHr + '"]').attr("selected", true);

                        getEvents(date);
                    },
                    eventResize: false,
                    eventClick: function (event, jsEvent, view, revertFunc) {
                        openModal = true;

                        $('#detalleNombreEvento').val(event.title);//prompt('Titulo del evento:');
                        $('#detallelblFecha').text(event.start.format('DD-MMMM-YYYY'));
                        
                        $('#detHrsInicio').val(event.start.format('HH:mm'));                        
                        $('#detHrsFin').val(event.end.format('HH:mm'));

                        dateCalendar = '';
                        dateCalendar = event.start.format('YYYY-MM-DD');
                        accion = true;
                        eventData = {};
                        eventData = event;

                        if (event.alumnoid == alumno.val()) {
                            $("#btnDetalle").click();
                        }
                    },
                    nowIndicator: true, //indicador de la hora actual
                    slotDuration: '01:00:00',
                    minTime: (businessHours[0] != undefined) ? businessHours[0].start : false,//'09:00:00',
                    maxTime: (businessHours[1] != undefined) ? businessHours[1].end : (businessHours[0] != undefined) ? businessHours[0].end : false,//'19:00:00',
                    selectable: true,
                    selectHelper: true,
                    allDaySlot: false, //oculta eventos de todo el día
                    select: addEvento,
                    editable: false, //Permite mover el evento de posición
                    eventLimit: true, // allow "more" link when too many events
                    events: eventos,
                    eventColor: '#378006',
                });
            }

            function getHoras(id, start, end) {
                if (start == undefined) start = 0;
                if (end == undefined) end = 0;
                $('#' + id).html('');
                for (var i = 0; i < 24; i++) {                    
                    if (i>=start && i<=end) {
                        if (i < 10) $('#' + id).append('<option name="0' + i + '">0' + i + ':00</option>');
                        else $('#' + id).append('<option name="' + i + '">' + i + ':00</option>');
                    }
                }
            }

            function getEvents(date) {
                if (eventos.length != undefined) {
                    var dateSearch = moment(date).format('YYYY-MM-DD HH:mm');
                    for (var i = 0; i < eventos.length; i++) {
                        var fecha = moment(eventos[i].start).format('YYYY-MM-DD HH:mm');
                        if (fecha == dateSearch) {
                        }
                        else if (fecha <= dateSearch && fecha >= dateSearch) {
                        }
                    }
                }
            }

            $("#btnSolicitar").click(function () {
                var title = $('#txtNombreEvento').val();//prompt('Titulo del evento:');	
                var hrsStart = $('#ddStart').val().split();
                var hrsEnd = $('#ddEnd').val().split();
                var fecha = $('#lblFecha').text();
                var alumnoid = parseInt($('#<%=hdnSessionAlumnoID.ClientID%>').val());
                var nombrealumno = $('#<%=hdnAspiranteNombre.ClientID%>').val();
                var saveValid = true;

                $('#<%=hdnInicio.ClientID%>').val(hrsStart);
                $('#<%=hdnFin.ClientID%>').val(hrsEnd);
                $('#<%=hdnFecha.ClientID%>').val(fecha);

                if (typeof businessHours[1] !== "undefined") {
                    var descansoStart = (typeof businessHours[0] !== "undefined") ? businessHours[0].end : "00:00";//'09:00:00'
                    var descansoEnd = (typeof businessHours[1] !== "undefined") ? businessHours[1].start : "00:00";//'09:00:00'

                    var text = "";
                    var saveValidStart = (parseInt(hrsStart[0].split(':')[0]) < parseInt(descansoStart.split(':')[0]) || parseInt(hrsStart[0].split(':')[0]) >= parseInt(descansoEnd.split(':')[0])) ? true : false;
                    var saveValidEnd = (parseInt(hrsEnd[0].split(':')[0]) > parseInt(descansoEnd.split(':')[0]) || parseInt(hrsEnd[0].split(':')[0]) <= parseInt(descansoStart.split(':')[0])) ? true : false;
                    var noIntervalBusiness = parseInt(hrsStart[0].split(':')[0]) < parseInt(descansoStart.split(':')[0]) && parseInt(hrsEnd[0].split(':')[0]) > parseInt(descansoEnd.split(':')[0]);

                    saveValid = saveValidStart && saveValidEnd && !noIntervalBusiness;

                    if (!saveValidStart && !saveValidEnd) {
                        text = "La hora de inicio y finalización está dentro del tiempo de descanso del orientador, por favor selecciona otra";
                    }
                    else {
                        if (!saveValidStart) text = "La hora de inicio está dentro del tiempo de descanso del orientador, por favor selecciona otra";
                        if (!saveValidEnd) text = "La hora de finalización está dentro del tiempo de descanso del orientador, por favor selecciona otra";
                    }

                    if (noIntervalBusiness)
                        text = "La hora sólo puede ser seleccionada antes o después del descanso del orientador, por favor selecciona otra";

                    if (!saveValid) {
                        var api = new MessageApi();
                        api.CreateMessage(text, "ERROR");
                        api.Show();
                    }
                }
                if (title != '' && saveValid) {
                    if (!accion) {
                        var dto = {};
                        var evento = {};
                        eventData = {};
                        if (eventos.length != undefined)
                            eventData.eventcalendarid = eventos.length;
                        else
                            eventData.eventcalendarid = 0;
                        eventData.title = title;
                        eventData.start = dateCalendar + 'T' + hrsStart;
                        eventData.end = dateCalendar + 'T' + hrsEnd;

                        dto.asunto = eventData.title;
                        dto.hrsinicio = eventData.start;
                        dto.hrsfin = eventData.end;
                        dto.fecha = fecha;
                        //alumno
                        dto.usuarioid = usuarioid;
                        dto.nombrecompletoalumno = nombrealumno;
                        dto.alumnoid = alumnoid;
                        dto.Success = '';
                        dto.Error = '';
                        evento.dto = dto;
                        saveEventCalendarAlumno(evento);
                    }
                    else {
                        var evento = {};
                        var dto = {};
                        var eventEdit = {};
                        eventData.title = title;
                        eventData.start = dateCalendar + 'T' + hrsStart;
                        eventData.end = dateCalendar + 'T' + hrsEnd;

                        for (var i = 0; i < eventos.length; i++) {
                            if (eventos[i].eventcalendarid == eventData.eventcalendarid) {
                                eventEdit.eventcalendarid = eventData.eventcalendarid;
                                eventEdit.title = title;
                                eventEdit.start = dateCalendar + 'T' + hrsStart;
                                eventEdit.end = dateCalendar + 'T' + hrsEnd;
                                eventos.splice(i, 1, eventEdit);
                            }
                        }
                        dto.eventcalendarid = eventEdit.eventcalendarid;
                        dto.asunto = eventEdit.title;
                        dto.hrsinicio = eventEdit.start;
                        dto.hrsfin = eventEdit.end;
                        dto.fecha = eventEdit.start;
                        dto.Success = '';
                        dto.Error = '';
                        evento.dto = dto;
                    }
                }
                $('#calendar').fullCalendar('unselect');
            });

            $('#ddStart').change(function () {
                var startItm = parseInt($(this).val().split(':')[0]) + 1;
                var endItm = parseInt($('#ddEnd').val().split(':')[0]);

                if (startItm > 22) {
                    getHoras('ddEnd', minTimeHL + 1, maxTimeFL + 1);
                    $('#ddEnd option[name="00"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHoras('ddEnd', minTimeHL + 1, maxTimeFL + 1);
                    if (startItm < 10) $('#ddEnd option[name="0' + startItm + '"]').attr("selected", true);
                    else $('#ddEnd option[name="' + startItm + '"]').attr("selected", true);
                }

            });

            $('#detalleddStart').change(function () {
                var startItm = parseInt($(this).val().split(':')[0]) + 1;
                var endItm = parseInt($('#detalleddEnd').val().split(':')[0]);

                if (startItm > 22) {
                    getHoras('detalleddEnd');
                    $('#detalleddEnd option[name="00"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHoras('detalleddEnd');
                    if (startItm < 10) $('#detalleddEnd option[name="0' + startItm + '"]').attr("selected", true);
                    else $('#detalleddEnd option[name="' + startItm + '"]').attr("selected", true);
                }

            });

            $('#ddEnd').change(function () {

                var startItm = parseInt($('#ddStart').val().split(':')[0]);
                var endItm = parseInt($(this).val().split(':')[0]) - 1;

                if (endItm + 1 < 1) {
                    getHoras('ddStart', minTimeHL, maxTimeFL);
                    $('#ddStart option[name="23"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHoras('ddStart', minTimeHL, maxTimeFL);
                    if (endItm < 10) $('#ddStart option[name="0' + endItm + '"]').attr("selected", true);
                    else $('#ddStart option[name="' + endItm + '"]').attr("selected", true);
                }
            });

            $('#detalleddEnd').change(function () {
                var startItm = parseInt($('#detalleddStart').val().split(':')[0]);
                var endItm = parseInt($(this).val().split(':')[0]) - 1;

                if (endItm + 1 < 1) {
                    getHoras('detalleddStart');
                    $('#detalleddStart option[name="23"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHoras('detalleddStart');
                    if (endItm < 10) $('#ddStart option[name="0' + endItm + '"]').attr("selected", true);
                    else $('#detalleddStart option[name="' + endItm + '"]').attr("selected", true);
                }
            });

            $("#btnClose").on("click", function () {

            });
        }

        $(function () {
            $("#docente-tabs").tabs({
                active:0,
                create: function (event, ui) {
                    page_dudas = 1; if (true) {

                        $("#DudasStream").empty(); LoadDudas();
                    }
                }
            });
            var options = {
                'maxCharacterSize': 400,
                'originalStyle': 'display_info_textarea',
                'warningStyle': 'display_warning_textarea',
                'warningNumber': 40,
                'displayFormat': '#left carácteres restantes de #max máx.'
            };
            $('#txtPublicacion').textareaCount(options);

        });
        $(function () {
            $('#tabs a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="page_title_select">
    Mi orientador
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="content_left_panel">
    <div id="user_img" style="margin-top: 10px;" class="profile_img-marcoFoto">
        <asp:Image runat="server" ID="ImgUser" CssClass="profile_img_orientador" />
        <br />
        <div style="text-align: center;">
            <asp:HyperLink ID="HplPerfil" runat="server" CssClass="link_blue">Yo</asp:HyperLink>
        </div>
    </div>
    <div class="text_format_1" style="text-align: center; margin-top: 5px; margin-bottom: 30px">
        <asp:Label ID="LblNombreDocente" runat="server"></asp:Label><br />
        <asp:Label ID="LblNombreEscuela" runat="server" Text="Escuela:" Visible="false"></asp:Label>
        <asp:Label ID="LblAsignatura" runat="server" Text="Asignatura:" Visible="false"></asp:Label>
        <br />
    </div>
    <div class="clear">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_perfil">
        <h1 class="">
            <asp:Label ID="LblNombreUniversidad" runat="server" />
        </h1>
        <h1 class="tBienvenida">
            <asp:Label ID="LblNombreUsuario" CssClass="tBienvenidaLabel" runat="server" />
        </h1>
        <div class="subline_title"></div>
    </div>
    <div>
        <div id="docente-tabs">
            <ul class="nav nav-tabs" role="tablist" id="tabs">
                <li role="presentation" class="active">
                    <a href="#docente-calendar" aria-controls="infoPersonal" role="tab" data-toggle="tab">
                        Solicitar orientaci&oacute;n
                    </a>
                </li>
                <li role="presentation">
                    <a href="#docente-muro" aria-controls="infoPersonal" role="tab" data-toggle="tab">
                        Apuntes de mi orientador
                    </a>
                </li>
                <li role="presentation">
                    <a href="#docente-dudas" aria-controls="infoPersonal" role="tab" data-toggle="tab">
                        Preguntarle a mi orientador
                    </a>
                </li>
            </ul>
            <div id="docente-muro">
                <!-- publicaciones muro -->
                <div>
                    <div id="PublicacionStream">
                    </div>
                    <div id="more">
                    </div>
                </div>
            </div>
            <div id="docente-dudas">
                <!-- registrar publicacion -->
                <div id="panel_insert_pub" class="ui-corner-all ui-widget-content">
                    <textarea id="txtPublicacion" rows="3" class="input_textarea" style="height: 54px;"></textarea>
                    <div class="">
                        <input type="button" value="Publicar" id="btnShare" class="btn-green button_clip_39215E" />
                    </div>
                </div>
                <!-- fin registrar publicacion -->
                <div>
                    <div id="DudasStream">
                    </div>
                    <div id="more-dudas" class="more">
                    </div>
                </div>

            </div>
            <div id="docente-calendar" style="margin-top:15px">
                <div id="config-calendar">
                    <div class="">
                        <button type="button" id="btnSolicitarOrientacion" class="button_clip_39215E" data-toggle="modal"
                            style="display: none" data-target=".dialog-md-2">
                            Abrir ventana modal</button>
                        <div class="modal fade dialog-md-2" tabindex="-1"
                            data-keyboard="false" data-backdrop="static"
                            role="dialog" aria-labelledby="ventanaModalLabel" id="AsignaPaqueteModal">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <!--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>-->
                                        <div class="col-xs-12 modal_titulo_marco_general center-block">Solicitud de sesi&oacute;n de orientaci&oacute;n</div>
                                    </div>
                                    <div class="modal-body container_busqueda_general ui-widget-content">
                                        <div class="">
                                            <div class="row bs-wizard" style="border-bottom: 0;">
                                                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                                    <div class="col-xs-12">
                                                        <div class="col-xs-12 form-group">
                                                            <fieldset>
                                                                <legend align="left">Información</legend>
                                                                <label class="col-sm-4 control-label">Fecha de la sesi&oacute;n:</label>
                                                                <div class="col-sm-8">
                                                                    <label id="lblFecha"></label>
                                                                </div>
                                                                <div id="horarioTrabajo"></div>
                                                            </fieldset>
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                            <fieldset>
                                                                <legend align="left">Horario</legend>

                                                                <label class="col-sm-2 control-label">Inicio:</label>
                                                                <div class="col-sm-4" id="divddStart">
                                                                    <select id="ddStart" class="form-control"></select>
                                                                </div>
                                                                <label class="col-sm-2 control-label">Fin:</label>
                                                                <div class="col-sm-4">
                                                                    <select id="ddEnd" class="form-control"></select>
                                                                </div>
                                                            </fieldset>
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                            <p>
                                                                <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                                                <label class="control-label">
                                                                    Las fechas y horarios disponibles dependen de la carga de trabajo del orientador. 
                                                                </label>
                                                                <label>
                                                                    Una vez que hayas solicitado tu sesión, recuerda mantenerte atento a tu correo para recibir la confirmaci&oacute;n por parte de tu orientador. Además, podrás visualizarlo dentro de tu calendario.
                                                                </label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-6 pull-right">
                                                <button type="button" class="btn btn-green" id="btnSolicitar" data-dismiss="modal">Solicitar</button>
                                                <button type="button" class="btn btn-cancel" id="btnCancel" data-dismiss="modal">Cancelar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <button type="button" id="btnDetalle" class="button_clip_39215E" data-toggle="modal"
                            style="display: none" data-target=".dialog-md-5">
                            Abrir ventana modal</button>
                        <div class="modal fade dialog-md-5" tabindex="-1"
                            data-keyboard="false" data-backdrop="static"
                            role="dialog" aria-labelledby="ventanaModalLabel" id="Div1">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <!--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>-->
                                        <div class="col-xs-12 modal_titulo_marco_general center-block">Orientaci&oacute;n vocacional</div>
                                    </div>
                                    <div class="modal-body container_busqueda_general ui-widget-content">
                                        <div class="">
                                            <div class="row bs-wizard" style="border-bottom: 0;">
                                                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                                    <div class="col-xs-12">
                                                        <div class="col-xs-12 form-group">
                                                            <label class="col-sm-4 control-label">Fecha de la sesi&oacute;n:</label>
                                                            <div class="col-sm-8">
                                                                <label id="detallelblFecha"></label>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                            <label class="col-sm-4 control-label">Horario programado</label>
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                            <label class="col-sm-2 control-label">Inicio:</label>
                                                            <div class="col-sm-4">
                                                                <select id="detalleddStart" class="form-control" style="display: none"></select>
                                                                <input type="text" id="detHrsInicio" class="form-control" disabled />
                                                            </div>
                                                            <label class="col-sm-2 control-label">Fin:</label>
                                                            <div class="col-sm-4">
                                                                <select id="detalleddEnd" class="form-control" style="display: none"></select>
                                                                <input type="text" id="detHrsFin" class="form-control" disabled />
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                        </div>
                                                        <div class="col-xs-12 form-group">
                                                            <p>
                                                                <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>

                                                                <label class="control-label">
                                                                    La solicitud ha sido aceptada, recuerda estar puntual el d&iacute;a de la reuni&oacute;n.
                                                                </label>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-6 pull-right">
                                                <button type="button" class="btn btn-cancel" id="btnAceptar" data-dismiss="modal">Aceptar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="">
                        <div class="" style="">
                            <div class="titulo_marco_general">Informaci&oacute;n del calendario del orientador</div>
                            <div class="container_busqueda_general ui-widget-content">
                                <div id='calendar'></div>
                                <div id="noCalendar" class="more">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <asp:HiddenField ID="hdnSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
        <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
        <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />

        <asp:HiddenField ID="hdnSessionUsuarioOrientadorID" runat="server" />
        <asp:HiddenField ID="hdnSessionAlumnoID" runat="server" />
        <asp:HiddenField ID="hdnAspiranteNombre" runat="server" />
        <asp:HiddenField ID="hdnCorreoOrientador" runat="server" />
        <asp:HiddenField ID="hdnInicio" runat="server" />
        <asp:HiddenField ID="hdnFin" runat="server" />
        <asp:HiddenField ID="hdnFecha" runat="server" />


        <script type="text/javascript">

            var hub = $("#<%=hdnSocialHubID.ClientID%>");
            var usr = $("#<%= hdnUsuarioSocialID.ClientID%>");

            var alumno = $("#<%= hdnSessionAlumnoID.ClientID%>");
            var curpage = 1;

            var INICIO = 0;
            var MURO = 1;
            var VIEW_PUB = 2;
            var place = MURO;

            var hubse = $("#<%=hdnSessionSocialHubID.ClientID%>");
                var usrse = $("#<%= hdnSessionUsuarioSocialID.ClientID%>");

            var TIPO_PUBLICACION_TEXTO = $("#<%=hdnTipoPublicacionTexto.ClientID%>");
            var TIPO_PUBLICACION_REACTIVO = $("#<%=hdnTipoPublicacionSuscripcionReactivo.ClientID%>");
       
        </script>
    </div>
    <div id="dialog-people-likes" title="Personas que le ha gustado">
        <div id="people-stream">
        </div>
    </div>
</asp:Content>
