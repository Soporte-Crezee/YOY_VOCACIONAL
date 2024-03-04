<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudOrientacionVocacional.ascx.cs" Inherits="POV.Web.PortalSocial.PortalGrupo.SolicitudOrientacionVocacional" %>
<link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/fullcalendar.css")%>" rel="stylesheet" />

<script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/moment.min.js")%>" type="text/javascript"></script>
<script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/fullcalendar.min.js")%>" type="text/javascript"></script>
<script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/es.js")%>" type="text/javascript"></script>

<script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.orientacionvocacional.js" type="text/javascript"></script>
<script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.orientacionvocacional.js" type="text/javascript"></script>
<style type="text/css">
    body {
        margin: 40px 10px;
        padding: 0;
        font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
        font-size: 14px;
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
</style>

<script type="text/javascript">

    $(document).ready(initPage);

    function showMessage(text, redirection) {
        $("#txtRedirect").val((redirection != undefined) ? "#" : "Alumnos.aspx");
        var api = new MessageApi();
        api.CreateMessage(text, "INFO");
        api.Show();
        resetHiden();
    }

    var cargaCompleta = false;
    var eventos = [];
    var businessHours =
            [ // specify an array instead
                { configcalendarid: undefined }
            ];

    function initPage() {
        //regio calor
        var fecha = $('#lblFecha').val();
        if (fecha != '')
            $("#<%=hdnFin.ClientID%>").val(fecha);
            var inicio = $('#ddStart').val();
            if (inicio != '')
                var fin = $('#ddEnd').val();
            $("#<%=hdnInicio.ClientID%>").val(inicio);
            if (fin != '')
                $("#<%=hdnFin.ClientID%>").val(fin);
        //endcarlo
        //$("#< %=txtSegundoApellido.ClientID%>").focus();        

        $("#ddStart").on("change", function (e) {
            var inicio = $('#ddStart').val();
            var fin = $('#ddEnd').val();
            $("#<%=hdnInicio.ClientID%>").val(inicio);
            $("#<%=hdnFin.ClientID%>").val(fin);
        });

        var accion = false;
        var eventData = {};
        var dateCalendar = '';
        //var formatHours = '24';        


        var usuarioid = parseInt($('#<%=hdnSessionUsuarioID.ClientID%>').val());
        var configcalendar = {};
        var dto = {};
        dto.usuarioid = usuarioid;
        configcalendar.dto = dto;
        setTimeout(function () {
            getConfigCalendarOrientador(configcalendar);
        }, 500);

        var esperando = setInterval(statusDataCalendar, 500);
        function statusDataCalendar() {
            if (cargaCompleta) {
                startCalendar();
                clearInterval();
                myApiBlockUI.unblockContainer()
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
                    right: 'month,agendaWeek'
                    //,agendaDay,listMonth
                },
                firstDay: 0, //0=dom,1=lun,2=mar,etc.
                weekNumbers: true,
                defaultDate: new Date(),
                navLinks: true, // can click day/week names to navigate views
                slotEventOverlap: false,
                businessHours: businessHours,
                dayClick: function (date, jsEvent, view) {
                    dateCalendar = '';
                    dateCalendar = date.format('YYYY-MM-DD');
                    $('#lblFecha').text(date.format('DD-MMMM-YYYY'));

                    getHorasInicio();
                    getHorasFin();
                    $('#ddStart option[name="' + date.format('HH') + '"]').attr("selected", true);

                    var finHr = parseInt(date.format('HH')) + 1;
                    if (finHr < 10)
                        $('#ddEnd option[name="0' + finHr + '"]').attr("selected", true);
                    else
                        $('#ddEnd option[name="' + finHr + '"]').attr("selected", true);

                    getEvents(date);

                    // change the day's background color just for fun
                    /*$(this).addClass('ac_checked_date');
                    $('.fc-day').not($(this)).removeClass('ac_checked_date');
    
                    $('#calendar0').fullCalendar('clientEvents', function(event) {
                        
                        var clickedDate = date;
    
                        
                        if(clickedDate >= event.start && clickedDate <= event.end) {
                        }	
                        });*/
                },
                eventResize: false,/*function(event, delta, revertFunc) {
				console.log(event);
				var title = event.title;
				var end = event.end.format();
				var start = event.start.format();
				},*/
                eventClick: function (event, jsEvent, view, revertFunc) {

                    $('#txtNombreEvento').val(event.title);//prompt('Titulo del evento:');
                    $('#lblFecha').text(event.start.format('DD-MMMM-YYYY'));
                    var fecha = $('#lblFecha').text(event.start.format('DD-MMMM-YYYY'));
                    $("#<%=hdnFin.ClientID%>").val(fecha);
                    getHorasInicio();
                    getHorasFin();
                    $('#ddStart option[name="' + event.start.format('HH') + '"]').attr("selected", true);
                    $('#ddEnd option[name="' + event.end.format('HH') + '"]').attr("selected", true);

                    dateCalendar = '';
                    dateCalendar = event.start.format('YYYY-MM-DD');
                    accion = true;
                    eventData = {};
                    eventData = event;
                    $("#btnSolicitarOrientacion").click();

                    /* var title = prompt('Event Title:', event.title, { buttons: { Ok: true, Cancel: false} });
                     if (title){
                        event.title = title;
                        $('#calendar').fullCalendar('updateEvent',event);
                        
                        $.ajax({
                         url: 'process.php',
                         data: 'type=changetitle&title='+title+'&eventid='+event.id,
                         type: 'POST',
                         dataType: 'json',
                         success: function(response){
                           if(response.status == 'success')
                           $('#calendar').fullCalendar('updateEvent',event);
                         },
                         error: function(e){
                           alert('Error processing your request: '+e.responseText);
                         }
                       });
                       }
                    }*/

                    //if (!confirm('some question')) {
                    //setTimeout(function () { revertFunc(); }, 2000)

                    //}

                },
                nowIndicator: true, //indicador de la hora actual
                slotDuration: '01:00:00',
                minTime: (businessHours[0] != undefined) ? businessHours[0].start : false,//'09:00:00',
                maxTime: (businessHours[1] != undefined) ? businessHours[1].end : (businessHours[0] != undefined) ? businessHours[0].end : false,//'19:00:00',
                selectable: true,
                selectHelper: true,
                //selectConstraint: 'businessHours', //selecciona las fechas si estan en el rango laboral
                //eventConstraint: 'businessHours',  //selecciona los eventos si estan en el rango laboral
                allDaySlot: false, //oculta eventos de todo el día
                select: addEvento,
                editable: false, //Permite mover el evento de posición
                eventLimit: true, // allow "more" link when too many events
                events: eventos,
                timeFormat: 'h(:mm)t',//'HH:mm',
                lang: 'de',
            });
        }

        function getHorasInicio() {
            $('#ddStart').html('');
            for (var i = 0; i < 24; i++) {
                if (i < 10) $('#ddStart').append('<option name="0' + i + '">0' + i + ':00</option>');
                else $('#ddStart').append('<option name="' + i + '">' + i + ':00</option>');
            }
        }

        function getHorasFin() {
            $('#ddEnd').html('');
            for (var i = 0; i < 24; i++) {
                if (i < 10) $('#ddEnd').append('<option name="0' + i + '">0' + i + ':00</option>');
                else $('#ddEnd').append('<option name="' + i + '">' + i + ':00</option>');
            }
        }

        function getEvents(date) {
            if (eventos.length != undefined) {
                var dateSearch = moment(date).format('YYYY-MM-DD');
                for (var i = 0; i < eventos.length; i++) {
                    var fecha = moment(eventos[i].start).format('YYYY-MM-DD');
                    //console.log(moment(eventos[i].start).format('HH:mm'));
                    if (fecha == dateSearch) {
                        //console.log('titulo de evento = ' + eventos[i].title);
                    }
                    else if (fecha <= dateSearch && fecha >= dateSearch) {
                        //console.log(eventos[i].title);
                    }
                }
            }
        }

        $("#btnSolicitar").click(function () {
            var title = $('#txtNombreEvento').val();//prompt('Titulo del evento:');	

            var hrsStart = $('#ddStart').val();
            var hrsEnd = $('#ddEnd').val();            

            if (title != '') {
                if (!accion) {
                    eventData = {};
                    if (eventos.length != undefined)
                        eventData.id = eventos.length;
                    else
                        eventData.id = 0;
                    eventData.title = title;
                    eventData.start = dateCalendar + 'T' + hrsStart;
                    eventData.end = dateCalendar + 'T' + hrsEnd;
                    eventos.push(eventData);
                    $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
                }
                else {
                    var eventEdit = {};
                    eventData.title = title;
                    eventData.start = dateCalendar + 'T' + hrsStart;
                    eventData.end = dateCalendar + 'T' + hrsEnd;

                    for (var i = 0; i < eventos.length; i++) {
                        if (eventos[i].id == eventData.id) {
                            eventEdit.id = eventData.id;
                            eventEdit.title = title;
                            eventEdit.start = dateCalendar + 'T' + hrsStart;
                            eventEdit.end = dateCalendar + 'T' + hrsEnd;
                            eventos.splice(i, 1, eventEdit);
                        }
                    }
                    //eventos.splice(1,0,'pepe');
                    $('#calendar').fullCalendar('updateEvent', eventData);
                }
            }
            $('#calendar').fullCalendar('unselect');
        });

        $('#ddStart').change(function () {
            var startItm = parseInt($(this).val().split(':')[0]) + 1;
            var endItm = parseInt($('#ddEnd').val().split(':')[0]);

            if (startItm > 22) {
                getHorasFin();
                $('#ddEnd option[name="00"]').attr("selected", true);
            }
            else if (startItm > endItm) {
                getHorasFin();
                if (startItm < 10) $('#ddEnd option[name="0' + startItm + '"]').attr("selected", true);
                else $('#ddEnd option[name="' + startItm + '"]').attr("selected", true);
            }
            $("#<=hdnInicio.ClientID%>").val(startItm);
            $("#<=hdnFin.ClientID%>").val(endItm);

        });

        $('#ddEnd').change(function () {
            var startItm = parseInt($('#ddStart').val().split(':')[0]);
            var endItm = parseInt($(this).val().split(':')[0]) - 1;

            if (endItm + 1 < 1) {
                getHorasInicio();
                $('#ddStart option[name="23"]').attr("selected", true);
            }
            else if (startItm > endItm) {
                getHorasInicio();
                if (endItm < 10) $('#ddStart option[name="0' + endItm + '"]').attr("selected", true);
                else $('#ddStart option[name="' + endItm + '"]').attr("selected", true);
            }
            $("#<=hdnInicio.ClientID%>").val(startItm);
            $("#<=hdnFin.ClientID%>").val(endItm);
        });

        $("#btnClose").on("click", function () {

        });

    }
</script>

<div id="panel-container" class="panel_edicion_perfil">
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
                                            <label class="col-sm-4 control-label">Fecha de la sesi&oacute;n</label>
                                            <div class="col-sm-8">
                                                <label id="lblFecha"></label>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Nombre de la sesi&oacute;n</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox runat="server" ID="txtNombreEvento" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Horario sugerido</label>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Inicio</label>
                                            <div class="col-sm-4" id="divddStart">
                                                <select id="ddStart" class="form-control"></select>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <label class="col-sm-4 control-label">Fin</label>
                                            <div class="col-sm-4">
                                                <select id="ddEnd" class="form-control"></select>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 form-group">
                                            <p>
                                                <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                                <label class="control-label">
                                                    La fecha que usted este solicitando depender&aacute; de la disponibilidad de horario del orientador, mantente atento a tu correo
                                                    para recibir la respuesta de confirmaci&oacute;n de la sesi&oacute;n por parte de tu orientador.
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
                            <%--<div class="col-md-6 pull-left">
                                <button type="button" class="btn btn-cancel" id="btnCancelSesion"
                                    data-dismiss="modal">
                                    Cancelar sesión de orientación</button>
                            </div>--%>
                            <div class="col-md-6 pull-right">
                                <button type="button" class="btn btn-cancel" id="btnCancel" data-dismiss="modal">Cancelar</button>
                                <asp:Button runat="server" class="btn btn-green" ID="btnSolicitar" Text="Solicitar" OnClick="btnSolicitar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-xs-12 col-md-12">
        <div class="col-md-12" style="padding: 20px 0px 0px 0px">
            <div class="col-xs-12 titulo_marco_general">Informaci&oacute;n del calendario del orientador</div>
            <div class="col-xs-12 container_busqueda_general ui-widget-content">
                <div id='calendar'></div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(loadControls);

    function loadControls() {
        //$('#< %=txtFechaNacimiento.ClientID %>').datepicker({ changeYear: true });
    }
</script>

<asp:HiddenField ID="hdnSocialHubID" runat="server" />
<asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
<asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
<asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
<asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
<asp:HiddenField ID="hdnSessionUsuarioID" runat="server" />
<asp:HiddenField ID="hdnInicio" runat="server" />
<asp:HiddenField ID="hdnFin" runat="server" />
<asp:HiddenField ID="hdnFecha" runat="server" />
<asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
<script type="text/javascript">
    var hub = $("#<%=hdnSocialHubID.ClientID%>");
    var usr = $("#<%= hdnUsuarioSocialID.ClientID%>");
    var usuario = $("#<%= hdnSessionUsuarioID.ClientID%>");
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
