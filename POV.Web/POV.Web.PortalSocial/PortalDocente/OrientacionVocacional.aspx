<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrientacionVocacional.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.OrientacionVocacional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/cupertino/jquery-ui.min.css")%>" rel="stylesheet" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/fullcalendar.css")%>" rel="stylesheet" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/moment.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/fullcalendar.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/es.js")%>" type="text/javascript" language="javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api.orientacionvocacional.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/core.orientacionvocacional.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>

    <style type="text/css">
        .panel-body {
            padding: 5px;
        }

        #calendar {
            max-width: 900px;
            margin: 0 auto;
        }

        .box-shadow {
            cursor: pointer;
            box-shadow: 0px 2px 9px 0px rgba(0, 0, 0, 0.35);
        }

            .box-shadow:hover {
                cursor: pointer;
                background-color: rgba(247, 178, 52, 0.19);
            }

        .btn span.glyphicon {
            opacity: 0;
        }

        .btn.active span.glyphicon {
            opacity: 1;
        }

        .btn {
            padding: 6px 7px;
        }

        .ui-widget-content a.fc-day-number {
            color: #252626;
        }

        button.fc-today-button,
        button.fc-month-button,
        button.fc-agendaWeek-button,
        button.fc-agendaDay-button,
        button.fc-prev-button,
        button.fc-next-button,
        button.fc-listMonth-button {
            border: 1px solid #c5c5c5 !important;
            background: #f6f6f6 !important;
            font-weight: normal !important;
            color: #454545 !important;
        }

        td.fc-head-container,
        th.fc-week-number,
        th.fc-day-header,
        th.fc-axis.fc-week-number,
        th.fc-day-header,
        td.ui-widget-header {
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
        button.fc-next-button.ui-button:active,
        button.fc-listMonth-button.ui-state-active,
        button.fc-listMonth-button.ui-button:active {
            background: #ff9e19 !important;
            border-color: #ff9e19 !important;
            color: #FFF !important;
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

        .fc-scroller {
            overflow-x: auto !important;
        }
        .fc-content span.fc-title {
            word-wrap: break-word;
            display: inherit;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(initPage);

        function showMessage(text, redirection, typeMessage) {
            $("#txtRedirect").val((typeof redirection !== "undefined" && redirection != '') ? "#" : "OrientacionVocacional.aspx");
            var link = (typeof redirection !== "undefined" && redirection != '') ? "#" : "OrientacionVocacional.aspx";
            var api = new MessageApi();
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

        var accion = false;
        var eventData = {};
        var dateCalendar = '';
        var eventcalendar = {};

        var cargaCompleta = false;
        var cargaCompletaEvent = false;
        var cargaCompletaSolicitudes = false;

        var eventos = [{ eventcalendarid: undefined }];
        var solicitudes = [{ eventcalendarid: undefined }];
        var businessHours = [{ configcalendarid: undefined }];

        var minTimeILab = '';
        var maxTimeFLab = '';
        var EventDetalle = {};

        function verDetalleEvento(itm) {

            $('#txtNombreEvento').val(itm.title);//prompt('Titulo del evento:');
            $('#lblFecha').text(itm.fecha);
            $('#txtNombreAspirante').val(itm.nombrecompletoalumno);
            $('#cantidadHrs').text(itm.cantidadhoras);

            getHours('ddStart', minTimeILab, maxTimeFLab);
            getHours('ddEnd', minTimeILab + 1, maxTimeFLab + 1);
            $('#ddStart option[name="' + itm.start.split('T')[1].split(':')[0] + '"]').attr("selected", true);
            var horaFinSession = $('#ddEnd option[name="' + itm.end.split('T')[1].split(':')[0] + '"]').attr("selected", true);

            $('#detHrsInicio').val(itm.start.split('T')[1]);
            $('#detHrsFin').val(itm.end.split('T')[1]);

            dateCalendar = '';
            dateCalendar = itm.start.split('T')[0];
            accion = false;
            eventData = {};
            eventData = itm;//$.toJSON(itm); //itm;
            EventDetalle = itm;
            $('#btnSolicitar').show();
            $('#btnSolicitar').removeAttr("disabled");
            $("#btnCerrarDetalle").click();
            $("#btnSolicitarOrientacion").click();

        }

        function getHours(id, start, end) {
            if (start == undefined) start = 0;
            if (end == undefined) end = 0;

            $('#' + id).html('');
            for (var i = 0; i < 24; i++) {
                if (start != 0 && start >= i) continue;
                if (end != 0 && end <= i) break;
                if (i < 10) $('#' + id).append('<option name="0' + i + '">0' + i + ':00</option>');
                else $('#' + id).append('<option name="' + i + '">' + i + ':00</option>');
            }
        }

        function initPage() {
            var configcalendar = {};
            var dto = {};
            configcalendar.dto = dto;

            eventcalendar.dto = { eventData: {} };
            setTimeout(function () {
                getEventCalendar(eventcalendar);//
                getConfigCalendar(configcalendar);
            }, 500);

            
            $("#<%=txtNumCantidadHrs.ClientID%>").on("keypress", function (e) {
                return false;
            });

            var esperando = setInterval(statusDataCalendar, 1000);
            function statusDataCalendar() {
                var cargaCompletaFull = (cargaCompleta == true && cargaCompletaEvent == true ) ? true : false;
                if (cargaCompletaFull) {
                    startCalendar();
                    minTimeILab = (businessHours[0] != undefined) ? businessHours[0].start : false;//'19:00:00'
                    maxTimeFLab = (businessHours[1] != undefined) ? businessHours[1].end : (businessHours[0] != undefined) ? businessHours[0].end : false;//'19:00:00'
                    minTimeILab = parseInt(minTimeILab.split(':')[0]) - 1;
                    maxTimeFLab = parseInt(maxTimeFLab.split(':')[0]) - 1;
                    clearInterval(esperando);
                    myApiBlockUI.unblockContainer();
                }
            }

            var addEvento = function (start, end) {
                $('#txtNombreEvento').val('');//prompt('Titulo del evento:');
                $('#txtInicio').val('');
                $('#txtFin').val('');
                accion = false;
                $("#btnSolicitarOrientacion").click();
            };

            function startCalendar() {
                $('#calendar').fullCalendar({
                    theme: true,
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay,listMonth'
                    },
                    firstDay: 0, //0=dom,1=lun,2=mar,etc.
                    weekNumbers: true, //activa los numeros de la semana
                    defaultDate: new Date(),
                    navLinks: true, // can click day/week names to navigate views
                    slotEventOverlap: false,
                    businessHours: businessHours,
                    dayClick: function (date, jsEvent, view) {
                        dateCalendar = '';
                        dateCalendar = date.format('YYYY-MM-DD');
                        $('#lblFecha').text(date.format('DD-MMMM-YYYY'));

                        getHours('ddStart', minTimeILab, maxTimeFLab);
                        getHours('ddEnd', minTimeILab + 1, maxTimeFLab + 1);
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

                        $('#txtNombreEvento').val(event.title);//prompt('Titulo del evento:');
                        $('#lblFecha').text(event.start.format('DD-MMMM-YYYY'));
                        $('#cantidadHrs').text(event.cantidadhoras);
                        $('#txtNombreAspirante').val(event.nombrecompletoalumno);
                        
                        $('#detHrsInicio').val(event.start.format('HH:mm'));
                        $('#detHrsFin').val(event.end.format('HH:mm'));
                        
                        dateCalendar = '';
                        dateCalendar = event.start.format('YYYY-MM-DD');
                        accion = true;
                        eventData = {};
                        eventData = event;
                        $('#btnSolicitar').hide();
                        $("#btnSolicitarOrientacion").click();
                    },
                    nowIndicator: true, //indicador de la hora actual
                    slotDuration: '00:30:00',
                    minTime: (businessHours[0] != undefined) ? businessHours[0].start : false,//'09:00:00',  vista horario de inicio en la semana
                    maxTime: (businessHours[1] != undefined) ? businessHours[1].end : (businessHours[0] != undefined) ? businessHours[0].end : false,//'19:00:00',    vista horario de fin en la semana
                    selectable: true,
                    selectHelper: true,
                    allDaySlot: false, //oculta eventos de todo el día
                    select: false,//addEvento,
                    editable: false, //Permite mover el evento de posición
                    eventLimit: true, // allow "more" link when too many events
                    events: eventos,
                    eventColor: '#378006'
                });

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
                var hrsStart = $('#detHrsInicio').val();
                var hrsEnd = $('#detHrsFin').val();
                var fecha = $('#lblFecha').text();
                                
                if (title != '') {
                    if (!accion) {
                        var dto = {};
                        var evento = {};
                        if (eventData.eventcalendarid == undefined) {
                            eventData = {};
                            if (eventos.length != undefined)
                                eventData.eventcalendarid = eventos.length;
                            else
                                eventData.eventcalendarid = 0;
                        }
                        else {
                            dto.eventcalendarid = eventData.eventcalendarid;
                        }
                        eventData.title = title;
                        eventData.start = dateCalendar + 'T' + hrsStart;
                        eventData.end = dateCalendar + 'T' + hrsEnd;
                        dto.asunto = eventData.title;
                        dto.hrsinicio = eventData.start;
                        dto.hrsfin = eventData.end;
                        dto.fecha = fecha;
                        dto.Success = '';
                        dto.Error = '';
                        dto.alumnoid = eventData.alumnoid;
                        dto.cantidadhoras = eventData.cantidadhoras;
                        dto.nombrecompletoalumno = eventData.nombrecompletoalumno;
                        dto.usuarioid = eventData.usuarioid;
                        evento.dto = dto;
                        eventos.push(eventData);
                        saveEventCalendar(evento);
                        $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
                    }
                    else {
                        var evento = {};
                        var eventEdit = {};
                        var dto = {};
                        eventData.title = title;
                        eventData.start = dateCalendar + 'T' + hrsStart;
                        eventData.end = dateCalendar + 'T' + hrsEnd;

                        for (var i = 0; i < eventos.length; i++) {
                            if (eventos[i].eventcalendarid == eventData.eventcalendarid) {
                                eventEdit.eventcalendarid = eventData.eventcalendarid;
                                eventEdit.start = dateCalendar + 'T' + hrsStart;
                                eventEdit.end = dateCalendar + 'T' + hrsEnd;
                                eventos.splice(i, 1, eventEdit);
                            }
                        }

                        dto.eventcalendarid = eventEdit.eventcalendarid;
                        dto.hrsinicio = eventEdit.start;
                        dto.hrsfin = eventEdit.end;
                        dto.Success = '';
                        dto.Error = '';
                        dto.alumnoid = eventEdit.alumnoid;
                        dto.cantidadhoras = eventEdit.cantidadhoras;
                        dto.nombrecompletoalumno = eventEdit.nombrecompletoalumno;
                        dto.usuarioid = eventEdit.usuarioid;
                        evento.dto = dto;
                        saveEventCalendar(evento);
                        $('#calendar').fullCalendar('updateEvent', eventData);

                    }
                }
                $('#calendar').fullCalendar('unselect');

            });

            $("#divEventos").click(function ()
            {
                $("#tblEventos tbody").empty();
                getSolicitudes(eventcalendar);
            });

            $('#ddStart').change(function () {
                var startItm = parseInt($(this).val().split(':')[0]) + 1;
                var endItm = parseInt($('#ddEnd').val().split(':')[0]);

                if (startItm > 22) {
                    getHours('ddEnd', minTimeILab + 1, maxTimeFLab + 1);
                    $('#ddEnd option[name="0"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHours('ddEnd', minTimeILab + 1, maxTimeFLab + 1);
                    if (startItm < 10) $('#ddEnd option[name="0' + startItm + '"]').attr("selected", true);
                    else $('#ddEnd option[name="' + startItm + '"]').attr("selected", true);
                }
            });

            $('#ddEnd').change(function () {
                var startItm = parseInt($('#ddStart').val().split(':')[0]);
                var endItm = parseInt($(this).val().split(':')[0]) - 1;

                if (endItm + 1 < 1) {
                    getHours('ddStart', minTimeILab, maxTimeFLab);
                    $('#ddStart option[name="23"]').attr("selected", true);
                }
                else if (startItm > endItm) {
                    getHours('ddStart', minTimeILab, maxTimeFLab);
                    if (endItm < 10) $('#ddStart option[name="0' + endItm + '"]').attr("selected", true);
                    else $('#ddStart option[name="' + endItm + '"]').attr("selected", true);
                }
            });
            //btnCancelSesion
            $("#btnCancelSesion").click(function () {
                var title = $('#txtNombreEvento').val();//prompt('Titulo del evento:');			
                var hrsStart = $('#ddStart').val();
                var hrsEnd = $('#ddEnd').val();
                var fecha = $('#lblFecha').text();

                var evento = {};
                var eventEdit = {};
                var dto = {};
                if (title != '') {
                    if (accion) {
                        for (var i = 0; i < eventos.length; i++) {
                            if (eventos[i].eventcalendarid == eventData.eventcalendarid) {
                                eventEdit.eventcalendarid = eventData.eventcalendarid;
                                eventEdit.alumnoid = eventData.alumnoid;
                                eventEdit.usuarioid = eventData.usuarioid;
                                eventEdit.title = title;
                                eventEdit.start = dateCalendar + 'T' + hrsStart;
                                eventEdit.end = dateCalendar + 'T' + hrsEnd;
                                eventos.splice(i, 1, eventEdit);
                            }
                        }

                        dto.eventcalendarid = eventEdit.eventcalendarid;
                        dto.hrsinicio = eventEdit.start;
                        dto.hrsfin = eventEdit.end;
                        dto.fecha = fecha;
                        dto.Success = '';
                        dto.Error = '';
                        dto.alumnoid = eventEdit.alumnoid;
                        dto.cantidadhoras = eventEdit.cantidadhoras;
                        dto.nombrecompletoalumno = eventEdit.nombrecompletoalumno;
                        dto.usuarioid = eventEdit.usuarioid;
                        evento.dto = dto;
                        //deleteSesion
                        deleteSesion(evento);
                        $('#calendar').fullCalendar('removeEvents', eventData.eventcalendarid);
                        $('#calendar').fullCalendar('renderEvent');
                    }
                    else {
                        dto.eventcalendarid = EventDetalle.eventcalendarid;
                        dto.hrsinicio = EventDetalle.start;
                        dto.hrsfin = EventDetalle.end;
                        dto.fecha = EventDetalle.fecha;
                        dto.Success = '';
                        dto.Error = '';
                        dto.alumnoid = EventDetalle.alumnoid;
                        dto.cantidadhoras = EventDetalle.cantidadhoras;
                        dto.nombrecompletoalumno = EventDetalle.nombrecompletoalumno;
                        dto.usuarioid = EventDetalle.usuarioid;
                        evento.dto = dto;
                        //solicitudes
                        solicitudes.splice(i, 1, EventDetalle);
                        //deleteSesion
                        getSolicitudes(eventcalendar);
                        deleteSesion(evento);
                    }
                }
                $('#calendar').fullCalendar('unselect');

            });

            //config Calendar
            $("#divConfigCalendar").click(function () {
                getHours('ddDisponibilidadInicio');
                getHours('ddDisponibilidadFin');
                if (businessHours[0].configcalendarid != undefined) {
                    var dowArray = businessHours[0].dow;
                    for (var i = 0; i < dowArray.length; i++) {
                        $(".check-day").eq(dowArray[i]).addClass(' active');
                        $(".check-day").eq(dowArray[i]).children("input[type='checkbox']").attr('checked', 'checked');
                    }
                }

                if (businessHours[1] != undefined) {
                    var inicioTrab = parseInt(businessHours[0].start.split(':')[0]);
                    var finTrab = parseInt(businessHours[1].end.split(':')[0]);
                    var inicioDesc = parseInt(businessHours[0].end.split(':')[0]);
                    var finDesc = parseInt(businessHours[1].start.split(':')[0]);
                    var descanso = finDesc - inicioDesc;

                    if (inicioTrab < 10) $('#ddDisponibilidadInicio option[name="0' + inicioTrab + '"]').attr("selected", true);
                    else $('#ddDisponibilidadInicio option[name="' + inicioTrab + '"]').attr("selected", true);

                    if (finTrab < 10) $('#ddDisponibilidadFin option[name="0' + finTrab + '"]').attr("selected", true);
                    else $('#ddDisponibilidadFin option[name="' + finTrab + '"]').attr("selected", true);

                    calcularDisponibilidad(false);
                    $('#<%=txtNumCantidadHrs.ClientID%>').val(descanso);

                    getHours('ddDescansoInicio',inicioTrab,finTrab);
                    $('#ddDescansoInicio option[name="' + inicioDesc + '"]').attr("selected", true);
                    calcularDescanso();
                }
                else {
                    var inicioTrab = parseInt((typeof businessHours[0].start !== "undefined")?businessHours[0].start.split(':')[0]:0);
                    var finTrab = parseInt((typeof businessHours[0].end !== "undefined")?businessHours[0].end.split(':')[0]:1);

                    if (inicioTrab < 10) $('#ddDisponibilidadInicio option[name="0' + inicioTrab + '"]').attr("selected", true);
                    else $('#ddDisponibilidadInicio option[name="' + inicioTrab + '"]').attr("selected", true);

                    if (finTrab < 10) $('#ddDisponibilidadFin option[name="0' + finTrab + '"]').attr("selected", true);
                    else $('#ddDisponibilidadFin option[name="' + finTrab + '"]').attr("selected", true);

                    calcularDisponibilidad(false);
                }
            });

            $("#btnGuardarConfig").click(function () {
                var txtDispInicio = $('#ddDisponibilidadInicio').val();
                var txtDispFin = $('#ddDisponibilidadFin').val();
                var txtDescInicio = $('#ddDescansoInicio').val();
                var txtDescFin = $('#<%=txtDescansoFin.ClientID%>').val();
                var txtHrsDesc = parseInt($('#<%=txtNumCantidadHrs.ClientID%>').val());
                
                if (txtHrsDesc > 0) {
                    var dow = [];
                    $(".check-day").children("input[type='checkbox']").each(function (i) {
                        if ($(this)[0].checked) dow.push(i);
                    });

                    var configcalendar = {};
                    var dto = {};

                    if (businessHours[0].configcalendarid != undefined)
                        dto.configcalendarid = businessHours[0].configcalendarid;
                    dto.diaslaborales = "" + dow;
                    dto.iniciotrabajo = txtDispInicio;
                    dto.fintrabajo = txtDispFin;
                    dto.iniciodescanso = txtDescInicio;
                    dto.findescanso = txtDescFin;
                    dto.Success = '';
                    dto.Error = '';

                    configcalendar.dto = dto;

                    saveConfigCalendar(configcalendar);
                }

                else {
                    var dow = [];
                    $(".check-day").children("input[type='checkbox']").each(function (i) {
                        if ($(this)[0].checked) dow.push(i); // Monday, Tuesday, Wednesday
                    });

                    var configcalendar = {};
                    var dto = {};
                    if (businessHours[0].configcalendarid != undefined)
                        dto.configcalendarid = businessHours[0].configcalendarid;
                    dto.diaslaborales = "" + dow;
                    dto.iniciotrabajo = txtDispInicio;
                    dto.fintrabajo = txtDispFin;
                    dto.iniciodescanso = '';
                    dto.findescanso = '';
                    dto.Success = '';
                    dto.Error = '';

                    configcalendar.dto = dto;
                    saveConfigCalendar(configcalendar);
                }
            });

            $('#ddDisponibilidadInicio').change(function () {
                calcularDisponibilidad(true);
            });

            $('#ddDisponibilidadFin').change(function () {
                calcularDisponibilidad(false);
            });

            $('#<%=txtNumCantidadHrs.ClientID%>').change(function () {
                var value = parseInt($(this).val());
                var inicioLab = parseInt($('#ddDisponibilidadInicio').val().split(':')[0]);
                var finLab = parseInt($('#ddDisponibilidadFin').val().split(':')[0]);
                var totalLab = finLab - inicioLab;

                if (value < totalLab && value > 0) $("#ddDescansoInicio").removeAttr('disabled');
                else { $("#ddDescansoInicio").attr('disabled', 'disabled'); }

                calcularDescanso();
            });

            $('#ddDescansoInicio').change(function () {
                calcularDescanso();
            });

            function calcularDisponibilidad(init) {
                var startItm = (init) ? parseInt($('#ddDisponibilidadInicio').val().split(':')[0]) + 1 : parseInt($('#ddDisponibilidadInicio').val().split(':')[0]);
                var endItm = (!init) ? parseInt($('#ddDisponibilidadFin').val().split(':')[0]) - 1 : parseInt($('#ddDisponibilidadFin').val().split(':')[0]);

                if (init) {
                    if (startItm > 22) {
                        getHours('ddDisponibilidadFin');
                        $('#ddDisponibilidadFin option[name="00"]').attr("selected", true);
                    }
                    else if (startItm > endItm) {
                        getHours('ddDisponibilidadFin');
                        if (startItm < 10) $('#ddDisponibilidadFin option[name="0' + startItm + '"]').attr("selected", true);
                        else $('#ddDisponibilidadFin option[name="' + startItm + '"]').attr("selected", true);
                    }
                }
                else {
                    if (endItm + 1 < 1) {
                        getHours('ddDisponibilidadInicio');
                        $('#ddDisponibilidadInicio option[name="23"]').attr("selected", true);
                    }
                    else if (startItm > endItm) {
                        getHours('ddDisponibilidadInicio');
                        if (endItm < 10) $('#ddDisponibilidadInicio option[name="0' + endItm + '"]').attr("selected", true);
                        else $('#ddDisponibilidadInicio option[name="' + endItm + '"]').attr("selected", true);
                    }
                }

                startItm = parseInt($('#ddDisponibilidadInicio').val().split(':')[0]);
                getHours('ddDescansoInicio', (startItm - 1), (endItm + 2));

                var totalLab = endItm - startItm;

                $('#<%=txtNumCantidadHrs.ClientID%>').val(0);
                $('#<%=txtNumCantidadHrs.ClientID%>').attr('max', totalLab);
                $("#ddDescansoInicio").attr('disabled', 'disabled');
                $('#<%=txtDescansoFin.ClientID%>').val('');

                if (totalLab <= 0)
                    $('#<%=txtNumCantidadHrs.ClientID%>').attr('disabled', 'disabled');
                else
                    $('#<%=txtNumCantidadHrs.ClientID%>').removeAttr('disabled');
            }

            function calcularDescanso() {
                var startItm = parseInt($('#ddDescansoInicio').val().split(':')[0]);
                var endItm = parseInt($('#<%=txtNumCantidadHrs.ClientID%>').val());
                var totalItm = startItm + endItm;

                var inicioLab = parseInt($('#ddDisponibilidadInicio').val().split(':')[0]);
                var finLab = parseInt($('#ddDisponibilidadFin').val().split(':')[0]);
                var totalLab = finLab - inicioLab;

                if (endItm < (finLab - startItm)) {
                    if (totalLab > endItm) {
                        if (totalItm < 10) $('#<%=txtDescansoFin.ClientID%>').val('0' + totalItm + ':00');
                        else $('#<%=txtDescansoFin.ClientID%>').val(totalItm + ':00');
                    }
                }
                else {
                    endItm = (finLab - startItm);
                    totalItm = startItm + endItm;
                    $('#<%=txtNumCantidadHrs.ClientID%>').val(endItm)
                    if (totalLab > endItm) {
                        if (totalItm < 10) $('#<%=txtDescansoFin.ClientID%>').val('0' + totalItm + ':00');
                        else $('#<%=txtDescansoFin.ClientID%>').val(totalItm + ':00');
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <div id="panel-container" class="panel_edicion_perfil">
        <div class="bodyadaptable">
            <div class="col-xs-12">
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
                                <div class="col-xs-12 modal_titulo_marco_general center-block">Orientaci&oacute;n vocacional</div>
                            </div>
                            <div class="modal-body container_busqueda_general ui-widget-content">
                                <div class="">
                                    <div class="row bs-wizard" style="border-bottom: 0;">
                                        <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                            <div class="col-xs-12">
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Fecha del evento:</label>
                                                    <div class="col-sm-8">
                                                        <label id="lblFecha"></label>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Nombre del evento:</label>
                                                    <div class="col-sm-8">
                                                        <input type="text" id="txtNombreEvento" class="form-control" disabled />
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Nombre del estudiante:</label>
                                                    <div class="col-sm-8">
                                                        <input type="text" id="txtNombreAspirante" class="form-control" disabled />
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Hora inicio:</label>
                                                    <div class="col-sm-4" id="divddStart">
                                                        <select id="ddStart" class="form-control" style="display: none"></select>
                                                        <input type="text" id="detHrsInicio" class="form-control" disabled />
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Hora fin:</label>
                                                    <div class="col-sm-4">
                                                        <select id="ddEnd" class="form-control" style="display: none"></select>
                                                        <input type="text" id="detHrsFin" class="form-control" disabled />
                                                    </div>
                                                </div>

                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-8 control-label">Cantidad de horas solicitadas:</label>
                                                    <div class="col-sm-4">
                                                        <label id="cantidadHrs"></label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <div class="col-md-6 pull-left">
                                        <button type="button" class="btn btn-cancel" id="btnCancelSesion" data-dismiss="modal">Cancelar sesión de orientación</button>
                                    </div>
                                    <div class="col-md-6 pull-right">
                                        <button type="button" class="btn btn-green" id="btnSolicitar" data-dismiss="modal">Aceptar</button>
                                        <button type="button" class="btn btn-cancel" id="btnCancel" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="modal fade dialog-md-1" tabindex="-1"
                    data-keyboard="false" data-backdrop="static"
                    role="dialog" aria-labelledby="ventanaModalLabel">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <!--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>-->
                                <div class="col-xs-12 modal_titulo_marco_general center-block">Configuraci&oacute;n del calendario</div>
                            </div>
                            <div class="modal-body container_busqueda_general ui-widget-content">
                                <div class="">
                                    <div class="row bs-wizard" style="border-bottom: 0;">
                                        <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                            <div class="col-xs-12">
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-2 control-label">D&iacute;as laborales</label>
                                                    <div class="col-sm-10">
                                                        <div class="btn-group" data-toggle="buttons">
                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="domingo">
                                                                <label>D</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="lunes">
                                                                <label>L</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="martes">
                                                                <label>M</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="miercoles">
                                                                <label>M</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="jueves">
                                                                <label>J</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="viernes">
                                                                <label>V</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>

                                                            <label class="btn btn-success check-day">
                                                                <input type="checkbox" autocomplete="off" name="sabado">
                                                                <label>S</label>
                                                                <span class="glyphicon glyphicon-ok"></span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Disponibilidad de horario:</label>
                                                    <div class="col-sm-4">
                                                        <select id="ddDisponibilidadInicio" class="form-control"></select>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <select id="ddDisponibilidadFin" class="form-control"></select>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Horas de descanso:</label>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox runat="server" ID="txtNumCantidadHrs" TextMode="Number" min="0" MaxLength="2"  step="1" CssClass="form-control" placeholder="0"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 form-group">
                                                    <label class="col-sm-4 control-label">Horario de descanso:</label>
                                                    <div class="col-sm-4">
                                                        <select id="ddDescansoInicio" class="form-control" disabled="disabled"></select>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:TextBox runat="server" ID="txtDescansoFin" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <div class="col-md-6 pull-right">
                                        <button type="button" class="btn btn-green" id="btnGuardarConfig" data-dismiss="modal">Guardar</button>
                                        <button type="button" class="btn btn-cancel" id="btnCancelarConfig" data-dismiss="modal">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12">
                <button type="button" id="btnVerEventos" class="button_clip_39215E" data-toggle="modal"
                    style="display: none" data-target=".modal-md-6">
                    Modal Eventos
                </button>
                <div class="modal fade dialog-md-6" tabindex="1"
                    data-keyboard="false" data-backdrop="static"
                    role="dialog" aria-labelledby="ventanaModalLabel" id="VerDetalleEventosCalendario">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <div class="col-xs-12 modal_titulo_marco_general center-block">Solicitudes de orientaci&oacute;n</div>
                            </div>
                            <div class="modal-body container_busqueda_general ui-widget-block">
                                <div class="">
                                    <div class="row bs-wizard" style="border-bottom: 0;">
                                        <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                                            <div class="col-xs-12 table-responsive">
                                                <table id="tblEventos" class="table table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th style="background-color: #0067ac; color: #fff;">Asunto
                                                            </th>
                                                            <th style="background-color: #0067ac; color: #fff;">Fecha
                                                            </th>
                                                            <th style="background-color: #0067ac; color: #fff;">Nombre estudiante
                                                            </th>
                                                            <th style="background-color: #0067ac; color: #fff;">Hora inicio
                                                            </th>
                                                            <th style="background-color: #0067ac; color: #fff;">Hora fin
                                                            </th>
                                                            <th style="background-color: #0067ac; color: #fff;"></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <div class="col-md-6 pull-right">
                                        <button type="button" class="btn btn-cancel" id="btnCerrarDetalle" data-dismiss="modal">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12 col-md-12">
                <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco_general">Calendario de orientaci&oacute;n</div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div class="col-xs-6 col-sm-6 col-md-2">
                            <div class="panel panel-default box-shadow" data-toggle="modal" data-target=".dialog-md-1" id="divConfigCalendar">
                                <div class="panel-body card">
                                    <div class="col-xs-12 col-sm-12">
                                        <img src="../images/VOCAREER_icon_config_calendar.png" id="imgConfigCalendar" alt="config" class="img img-responsive" />
                                    </div>
                                    <div class="col-xs-12 col-sm-12">
                                        <p>Configuraci&oacute;n del calendario</p>
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default box-shadow" data-toggle="modal" data-target=".dialog-md-6" id="divEventos">
                                <div class="panel-body card">
                                    <div class="col-xs-12 col-sm-12">
                                        <img src="../Images/YOY_icon_solicitudes.png" id="imgEventosCalendar" alt="events" class="img img-responsive" />
                                    </div>
                                    <div class="col-xs-12 col-sm-12">
                                        <p>Solicitudes</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-10">
                            <div id='calendar'></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-md-offset-2">
                <div class="opciones_formulario">
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
    <asp:HiddenField ID="hdnFuente" runat="server" Value="D" />
</asp:Content>
