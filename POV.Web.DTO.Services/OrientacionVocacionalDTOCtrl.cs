using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Web.DTO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using GP.SocialEngine.DA;
using GP.SocialEngine.Utils;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Blog.BO;
using POV.Blog.Services;
using POV.CentroEducativo.Services;
using POV.Seguridad.BO;
using POV.Comun.Service;
using POV.CentroEducativo.BO;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using POV.Logger.Service;
using System.Web;
using POV.Seguridad.Service;
using System.Globalization;
using POV.Licencias.Service;
using POV.Administracion.Services;
using POV.Administracion.BO;
using POV.Modelo.Context;
using System.Data.Entity.Validation;


namespace POV.Web.DTO.Services
{
    public class OrientacionVocacionalDTOCtrl
    {
        private IUserSession userSession;
        private ConfigCalendarCtrl configCalendarCtrl;
        private ConfigCalendar configcalendar;
        private IDataContext dctx;

        // Eventos Calendar
        private EventCalendarCtrl eventCalendarCtrl;
        private EventCalendar eventcalendar;

        // Sesion de orientacion
        private SesionOrientacionCtrl sesionOrientacionCtrl;
        private SesionOrientacion sesionorientacion;

        // Encuesta de satisfaccion
        private EncuestaSatisfaccionCtrl encuestaSatisfaccionCtrl;
        private EncuestaSatisfaccion encuestasatisfaccion;

        public OrientacionVocacionalDTOCtrl()
        {
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
            userSession = new UserSession();
            configCalendarCtrl = new ConfigCalendarCtrl(null);
            configcalendar = new ConfigCalendar();

            // Eventos Calendar
            eventCalendarCtrl = new EventCalendarCtrl(null);
            eventcalendar = new EventCalendar();

            // Sesion de orientacion+
            sesionOrientacionCtrl = new SesionOrientacionCtrl(null);
            sesionorientacion = new SesionOrientacion();

            // Encuesta de satisfaccion
            encuestaSatisfaccionCtrl = new EncuestaSatisfaccionCtrl(null);
            encuestasatisfaccion = new EncuestaSatisfaccion();
        }

        //Insert
        public configcalendardto GuardarConfiguracionAgenda(configcalendardto dto)
        {
            try
            {
                string sError = string.Empty;

                //Valores Requeridos.
                if (string.IsNullOrEmpty(dto.diaslaborales))
                    sError += " ,Días laborales";
                if (string.IsNullOrEmpty(dto.iniciotrabajo))
                    sError += " ,Inicio trabajo";
                if (string.IsNullOrEmpty(dto.fintrabajo))
                    sError += " ,Fin trabajo";
                
                if (sError.Trim().Length > 0)
                {
                    dto.Error = "Los siguientes datos son requerido" + sError;
                }
                else
                {
                    if (dto.configcalendarid != null) configcalendar.ConfigCalendarID = dto.configcalendarid;
                    configcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;

                    var result = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();
                    if (result != null) configcalendar = result;

                    configcalendar.DiasLaborales = dto.diaslaborales;
                    configcalendar.InicioTrabajo = dto.iniciotrabajo;
                    configcalendar.FinTrabajo = dto.fintrabajo;
                    configcalendar.InicioDescanso = dto.iniciodescanso;
                    configcalendar.FinDescanso = dto.findescanso;

                    if (result != null)
                    {
                        configCalendarCtrl.Update(configcalendar);
                        dto.Success = "La configuración se actualizó correctamente";
                    }

                    else
                    {
                        configCalendarCtrl.Insert(configcalendar);
                        dto.Success = "La configuración se guardó correctamente";
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intente más tarde";
                return dto;
            }
        }

        //Insert
        public configcalendardto GetConfiguracionAgenda(configcalendardto dto)
        {
            try
            {
                configcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;
                var result = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();
                if (result != null)
                {
                    dto.configcalendarid = result.ConfigCalendarID;
                    dto.usuarioid = result.UsuarioID;
                    dto.diaslaborales = result.DiasLaborales;
                    dto.iniciotrabajo = result.InicioTrabajo;
                    dto.fintrabajo = result.FinTrabajo;
                    dto.iniciodescanso = result.InicioDescanso;
                    dto.findescanso = result.FinDescanso;
                    dto.Success = "La configuración se cargó correctamente";
                    dto.Error = "";
                }

                else
                {
                    dto.Success = "";
                    dto.Error = "No se ha creado una configuración de calendario";
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intente más tarde";
                return dto;
            }
        }

        // metodo para cargar el horario del docente seleccionado y mostrarselo al alumno
        public configcalendardto GetConfiguracionAgendaOrientador(configcalendardto dto)
        {
            try
            {
                configcalendar.UsuarioID = dto.usuarioid;
                var result = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();
                if (result != null)
                {
                    dto.configcalendarid = result.ConfigCalendarID;
                    dto.usuarioid = result.UsuarioID;
                    dto.diaslaborales = result.DiasLaborales;
                    dto.iniciotrabajo = result.InicioTrabajo;
                    dto.fintrabajo = result.FinTrabajo;
                    dto.iniciodescanso = result.InicioDescanso;
                    dto.findescanso = result.FinDescanso;
                    dto.Success = "La configuración se cargó correctamente";
                    dto.Error = "";
                }

                else
                {
                    dto.Success = "";
                    dto.Error = "No se ha creado una configuración de calendario";
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intente más tarde";
                return dto;
            }
        }

        // metodo para guardar el evento por parte del orientador
        public eventcalendardto GuardarEvento(eventcalendardto dto)
        {
            try
            {
                // Contexto para update evento, insert orientacion vocacional
                var objeto = new object();
                var contexto = new Contexto(objeto);
                eventCalendarCtrl = new EventCalendarCtrl(contexto);

                if (dto.eventcalendarid != null) eventcalendar.EventCalendarID = dto.eventcalendarid;
                eventcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;

                var result = eventCalendarCtrl.Retrieve(eventcalendar, true).FirstOrDefault();
                string fechaEvent = result.Fecha;
                DateTime fecha = Convert.ToDateTime(fechaEvent);
                var horasEvent = result.CantidadHoras;
                if (result != null) eventcalendar = result;

                configcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;
                var resultCalendar = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();

                eventcalendar.Asunto = dto.asunto;
                eventcalendar.Fecha = dto.fecha;
                eventcalendar.HrsInicio = dto.hrsinicio;
                eventcalendar.HrsFin = dto.hrsfin;
                eventcalendar.ConfigCalendarID = resultCalendar.ConfigCalendarID;

                var dtoHIn = Int32.Parse(dto.hrsinicio.Split(':')[1]);
                var dtoHFn = Int32.Parse(dto.hrsfin.Split(':')[1]);

                string dateTimeIn = dto.hrsinicio;
                DateTime inicioInsert = Convert.ToDateTime(dateTimeIn);
                string dateTimeFn = dto.hrsfin;
                DateTime finInsert = Convert.ToDateTime(dateTimeFn);

                var cantidadHrs = finInsert.Hour - inicioInsert.Hour;

                #region Verificar saldo del alumno
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();

                EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
                Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = dto.alumnoid }, true).FirstOrDefault();
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario() { UsuarioID = dto.usuarioid }));
                Docente orientador = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, usuario);
                #endregion

                var saveValid = true;
                if (result != null)
                {
                    string diaSave = fechaEvent;
                    DateTime fechaSave = Convert.ToDateTime(diaSave);
                    if (fechaSave.Date <= DateTime.Now.Date)
                    {
                        dto.Success = "";
                        dto.Error = "La fecha no puede ser menor o igual a la de hoy.";
                        return dto;
                    }
                    else
                    {
                        if (resultCalendar.EventCalendar.Count > 0)
                        {
                            foreach (EventCalendar evento in resultCalendar.EventCalendar)
                            {
                                string dateTimehIn = evento.HrsInicio;
                                DateTime inicioSave = Convert.ToDateTime(dateTimehIn);
                                string dateTimehFn = evento.HrsFin;
                                DateTime finSave = Convert.ToDateTime(dateTimehFn);

                                var text = "";
                                var saveValidStart = inicioInsert < inicioSave || inicioInsert >= finSave ? true : false;
                                var saveValidEnd = finInsert > finSave || finInsert <= inicioSave ? true : false;
                                var noIntervalBusiness = inicioInsert < inicioSave && finInsert > finSave;
                                
                                saveValid = saveValidStart && saveValidEnd && !noIntervalBusiness;

                                if (!saveValidStart && !saveValidEnd)
                                {
                                    text = "La hora solicitada está ocupada";
                                }
                                else
                                {
                                    if (!saveValidStart) text = "La hora de inicio está ocupada";
                                    if (!saveValidEnd) text = "La hora de finalización está ocupada";
                                }

                                if (noIntervalBusiness)
                                    text = "La hora se cruza con el horario de " + inicioSave.Hour + ":00 a " + finSave.Hour + ":00";

                                if (!saveValid)
                                {
                                    dto.Success = "";
                                    dto.Error = text;
                                    return dto;
                                }
                            }
                        }
                    }

                    if (saveValid)
                    {
                        if (cantidadHrs < horasEvent || cantidadHrs > horasEvent)
                        {
                            dto.Success = "";
                            dto.Error = "La cantidad de horas seleccionada no concuerda con las solicitadas.";
                        }
                        else
                        {
                            // crear orientacion
                            SesionOrientacion sesion = new SesionOrientacion();
                            sesion.Inicio = dateTimeIn;
                            sesion.Fin = dateTimeFn;
                            sesion.Fecha = fechaEvent;
                            sesion.EstatusSesion = ESesionOrientacion.NOINICIADO;
                            sesion.EncuestaContestada = false;
                            sesion.AsistenciaAspirante = false;
                            sesion.AsistenciaOrientador = false;
                            sesion.AlumnoID = result.AlumnoID;
                            sesion.DocenteID = orientador.DocenteID;
                            sesion.CantidadHoras = cantidadHrs;
                            SesionOrientacionCtrl sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);
                            eFAlumnoCtrl.Update(alumno);
                            sesionOrientacionCtrl.Insert(sesion);
                            eventCalendarCtrl.Update(eventcalendar);
                            contexto.Commit(objeto);
                            contexto.Dispose();
                            dto.Error = "";
                            dto.Success = "El evento se guardó correctamente";
                        }
                    }
                }
                else
                {
                    string diaSave = result.Fecha;
                    DateTime fechaSave = Convert.ToDateTime(diaSave);
                    if (fechaSave.Date <= DateTime.Now.Date)
                    {
                        dto.Success = "";
                        dto.Error = "La fecha no puede ser menor o igual a la de hoy.";
                        return dto;
                    }
                    else
                    {
                        if (resultCalendar.EventCalendar.Count > 0)
                        {
                            foreach (EventCalendar evento in resultCalendar.EventCalendar)
                            {
                                string dateTimehIn = evento.HrsInicio;
                                DateTime inicioSave = Convert.ToDateTime(dateTimehIn);
                                string dateTimehFn = evento.HrsFin;
                                DateTime finSave = Convert.ToDateTime(dateTimehFn);

                                var text = "";
                                var saveValidStart = inicioInsert < inicioSave || inicioInsert >= finSave ? true : false;
                                var saveValidEnd = finInsert > finSave || finInsert <= inicioSave ? true : false;
                                var noIntervalBusiness = inicioInsert < inicioSave && finInsert > finSave;

                                saveValid = saveValidStart && saveValidEnd && !noIntervalBusiness;

                                if (!saveValidStart && !saveValidEnd)
                                {
                                    text = "La hora de inicio y finalización están ocupados";
                                }
                                else
                                {
                                    if (!saveValidStart) text = "La hora de inicio está ocupada";
                                    if (!saveValidEnd) text = "La hora de finalización está ocupada";
                                }

                                if (noIntervalBusiness)
                                    text = "La hora se cruza con el horario de " + inicioSave.Hour + ":00 a " + finSave.Hour + ":00";

                                if (!saveValid)
                                {
                                    dto.Success = "";
                                    dto.Error = text;
                                    return dto;
                                }
                            }
                        }
                        if (saveValid)
                        {
                            if (cantidadHrs < horasEvent || cantidadHrs > horasEvent)
                            {
                                dto.Success = "";
                                dto.Error = "La cantidad de horas seleccionada no concuerda con las solicitadas.";
                            }
                            else
                            {
                                // crear orientacion
                                SesionOrientacion sesion = new SesionOrientacion();
                                sesion.Inicio = dateTimeIn;
                                sesion.Fin = dateTimeFn;
                                sesion.Fecha = fechaEvent;
                                sesion.EstatusSesion = ESesionOrientacion.NOINICIADO;
                                sesion.EncuestaContestada = false;
                                sesion.AsistenciaAspirante = false;
                                sesion.AsistenciaOrientador = false;
                                sesion.AlumnoID = result.AlumnoID;
                                sesion.DocenteID = orientador.DocenteID;
                                SesionOrientacionCtrl sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);
                                eFAlumnoCtrl.Update(alumno);
                                sesionOrientacionCtrl.Insert(sesion);
                                eventCalendarCtrl.Update(eventcalendar);
                                contexto.Commit(objeto);
                                contexto.Dispose();
                                dto.Error = "";
                                dto.Success = "El evento se guardó correctamente";
                            }
                        }
                    }
                }
                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba el evento, intente más tarde";
                return dto;
            }
        }

        // metodo para guardar el evento por parte del aspirante
        public eventcalendardto GuardarEventoAlumno(eventcalendardto dto)
        {
            try
            {
                eventcalendar.Asunto = "Orientacion Vocacional";
                eventcalendar.Fecha = dto.fecha;
                eventcalendar.HrsInicio = dto.hrsinicio;
                eventcalendar.HrsFin = dto.hrsfin;
                eventcalendar.UsuarioID = dto.usuarioid;
                //alumno
                eventcalendar.AlumnoID = dto.alumnoid;
                eventcalendar.NombreCompletoAlumno = dto.nombrecompletoalumno;

                string dateTimeIn = dto.hrsinicio;
                DateTime inicioInsert = Convert.ToDateTime(dateTimeIn);
                string dateTimeFn = dto.hrsfin;
                DateTime finInsert = Convert.ToDateTime(dateTimeFn);

                // fecha
                string dateTimeDay = dto.fecha;
                DateTime dia = Convert.ToDateTime(dateTimeDay);

                var cantidadHrs = finInsert.Hour - inicioInsert.Hour;

                eventcalendar.CantidadHoras = cantidadHrs;

                var saveValid = true;

                if (dia.Date <= DateTime.Now.Date)
                {
                    dto.Success = "";
                    dto.Error = "La fecha no puede ser menor o igual a la de hoy.";
                    return dto;
                }
                else
                {
                    List<EventCalendar> eventResult = eventCalendarCtrl.Retrieve(new EventCalendar { UsuarioID = dto.usuarioid, Fecha = dto.fecha }, false);
                    if (eventResult.Count > 0)
                    {
                        foreach (EventCalendar evento in eventResult)
                        {
                            string dateTimehIn = evento.HrsInicio;
                            DateTime inicioSave = Convert.ToDateTime(dateTimehIn);
                            string dateTimehFn = evento.HrsFin;
                            DateTime finSave = Convert.ToDateTime(dateTimehFn);

                            var text = "";
                            var saveValidStart = inicioInsert < inicioSave || inicioInsert >= finSave ? true : false;
                            var saveValidEnd = finInsert > finSave || finInsert <= inicioSave ? true : false;
                            var noIntervalBusiness = inicioInsert < inicioSave && finInsert > finSave;

                            saveValid = saveValidStart && saveValidEnd && !noIntervalBusiness;

                            if (!saveValidStart && !saveValidEnd)
                            {
                                text = "La hora de inicio y finalización están ocupados por otra solicitud de orientación, por favor selecciona otra";
                            }
                            else
                            {
                                if (!saveValidStart) text = "La hora de inicio está ocupada por otra solicitud de orientación, por favor selecciona otra";
                                if (!saveValidEnd) text = "La hora de finalización está ocupada por otra solicitud de orientación, por favor selecciona otra";
                            }

                            if (noIntervalBusiness)
                                text = "La hora solicitada se cruza con el horario de " + inicioSave.Hour + ":00 a " + finSave.Hour + ":00 de otra solicitud, por favor selecciona otra";

                            if (!saveValid)
                            {
                                dto.Success = "";
                                dto.Error = text;
                                return dto;
                            }
                        }
                    }
                    if (saveValid)                    
                    {
                        eventCalendarCtrl.Insert(eventcalendar);
                        dto.Error = "";
                        dto.Success = "Tu solicitud se ha enviado correctamente";
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Success = "";
                dto.Error = "Ha ocurrido un error mientras se guardaba el evento, intente más tarde";
                return dto;
            }
        }

        public List<eventcalendardto> GetEvento(eventcalendardto dto)
        {
            List<eventcalendardto> listaEventos = new List<eventcalendardto>();
            try
            {
                configcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;
                var resultCalendar = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();
                if (resultCalendar != null)
                {
                    eventcalendar.ConfigCalendarID = resultCalendar.ConfigCalendarID;
                    List<EventCalendar> eventResult = eventCalendarCtrl.Retrieve(eventcalendar, true);
                    foreach (EventCalendar evento in eventResult)
                    {
                        eventcalendardto evt = EventCalendarToDTO(evento);
                        listaEventos.Add(evt);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listaEventos;
        }

        public eventcalendardto EventCalendarToDTO(EventCalendar eventCalendar)
        {
            eventcalendardto dto = new eventcalendardto();
            if (eventCalendar != null)
            {
                dto.eventcalendarid = eventCalendar.EventCalendarID;
                dto.asunto = eventCalendar.Asunto;
                dto.fecha = eventCalendar.Fecha;
                dto.hrsinicio = eventCalendar.HrsInicio;
                dto.hrsfin = eventCalendar.HrsFin;
                dto.usuarioid = eventCalendar.UsuarioID;
                dto.alumnoid = eventCalendar.AlumnoID;
                dto.nombrecompletoalumno = eventCalendar.NombreCompletoAlumno;
                dto.cantidadhoras = eventCalendar.CantidadHoras;
                dto.Success = "Dato cargado correctamente.";
                dto.Error = "";
            }
            else
            {
                dto.Success = "";
                dto.Error = "Error al cargar el datos.";
            }
            return dto;
        }

        // metodo para cargar los eventos que el orientador ha aceptado al alumno
        public List<eventcalendardto> GetEventoOrientador(eventcalendardto dto)
        {
            List<eventcalendardto> listaEventos = new List<eventcalendardto>();
            try
            {
                configcalendar.UsuarioID = dto.usuarioid;
                var resultCalendar = configCalendarCtrl.Retrieve(configcalendar, true).FirstOrDefault();
                if (resultCalendar != null)
                {
                    eventcalendar.ConfigCalendarID = resultCalendar.ConfigCalendarID;
                    List<EventCalendar> eventResult = eventCalendarCtrl.Retrieve(eventcalendar, true);
                    foreach (EventCalendar evento in eventResult)
                    {
                        eventcalendardto evt = EventCalendarToDTO(evento);
                        listaEventos.Add(evt);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listaEventos;
        }

        // metodo para cargar la lista de solicitudes del orientador
        public List<eventcalendardto> GetSolicitudes(eventcalendardto dto)
        {
            List<eventcalendardto> listaEventos = new List<eventcalendardto>();
            try
            {
                eventcalendar.UsuarioID = userSession.CurrentUser.UsuarioID;//resultCalendar.ConfigCalendarID;
                List<EventCalendar> eventResult = eventCalendarCtrl.Retrieve(eventcalendar, true);
                foreach (EventCalendar evento in eventResult)
                {
                    DateTime Fecha = Convert.ToDateTime(evento.Fecha);
                    DateTime Today = Convert.ToDateTime(DateTime.Now);
                    if (evento.ConfigCalendarID == null)
                    {
                        eventcalendardto evt = EventCalendarToDTO(evento);
                        listaEventos.Add(evt);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            return listaEventos;
        }

        // metodo para eliminar/cancelar sesion de orientacion
        public eventcalendardto EliminarSesion(eventcalendardto dto) 
        {
            try
            {
                var objeto = new object();
                var contexto = new Contexto(objeto);
                eventCalendarCtrl = new EventCalendarCtrl(contexto);
                configCalendarCtrl = new ConfigCalendarCtrl(contexto);


                configcalendar.UsuarioID = dto.usuarioid;
                var resultconfig = configCalendarCtrl.RetrieveWithRelationship(configcalendar, true).FirstOrDefault();
                eventcalendar.EventCalendarID = dto.eventcalendarid;
                var result = eventCalendarCtrl.Retrieve(eventcalendar, true).FirstOrDefault();               

                SesionOrientacionCtrl sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario() { UsuarioID = dto.usuarioid }));
                Docente orientador = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, usuario);
                SesionOrientacion sesion = new SesionOrientacion();
                sesion.AlumnoID = dto.alumnoid;
                sesion.DocenteID = orientador.DocenteID;
                sesion.EstatusSesion = ESesionOrientacion.NOINICIADO;
                string dtoFecha = result.Fecha; //dto.fecha;
                DateTime fecha = Convert.ToDateTime(dtoFecha);
                sesion.Fecha = dtoFecha;
                string dtoInicio = result.HrsInicio; //dto.hrsinicio;
                DateTime Inicio = Convert.ToDateTime(dtoInicio);
                sesion.Inicio = Inicio.ToString();
                string dtoFin = result.HrsFin; //dto.hrsinicio;
                DateTime Fin = Convert.ToDateTime(dtoFin);
                sesion.Inicio = Fin.ToString();
                var resultSesion = sesionOrientacionCtrl.Retrieve(sesion, true).FirstOrDefault();

                if (result.ConfigCalendarID == null) 
                {
                    eventCalendarCtrl.Delete(result);
                    dto.Success = "La sesion se canceló correctamente";
                    dto.Error = "";
                    contexto.Commit(objeto);
                    contexto.Dispose();
                }
                else
                {

                    if (result != null && resultSesion != null && resultSesion.EstatusSesion == ESesionOrientacion.NOINICIADO)
                    {
                        eventCalendarCtrl.Delete(result);
                        configCalendarCtrl.Update(resultconfig);
                        sesionOrientacionCtrl.Delete(resultSesion);
                        contexto.Commit(objeto);
                        contexto.Dispose();
                        dto.Success = "La sesion se canceló correctamente";
                        dto.Error = "";
                    }

                    else
                    {
                        dto.Success = "";
                        dto.Error = "No se pudo cancelar la sesión, intente más tarde";
                    }
                }

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intente más tarde";
                return dto;
            }
        }

        // metodo para consultar las sesiones de orientacion pendientes del alumno
        public List<sesionorientaciondto> GetSesionOrientacionAlumno(sesionorientaciondto dto) 
        {
            List<sesionorientaciondto> listaSesiones = new List<sesionorientaciondto>();
            try
            {
                if (dto != null)
                    sesionorientacion.AlumnoID = dto.alumnoid;
                else
                sesionorientacion.AlumnoID = userSession.CurrentAlumno.AlumnoID;
                List<SesionOrientacion> sesionResult = sesionOrientacionCtrl.Retrieve(sesionorientacion, true);
                foreach (SesionOrientacion sesion in sesionResult)
                {
                    DateTime Fecha = Convert.ToDateTime(sesion.Fecha);
                    DateTime Today = Convert.ToDateTime(DateTime.Now);
                    // verifcar que la sesion no este iniciada ni finalizada
                    // verficar que sea de la fecha de actual
                    if (Fecha.Day == Today.Day) 
                    {
                        sesionorientaciondto sov = SesionOrientacionToDTO(sesion);
                        listaSesiones.Add(sov);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }

            return listaSesiones;
        }

        public sesionorientaciondto SesionOrientacionToDTO(SesionOrientacion sesionOrientacion)
        {
            sesionorientaciondto dto = new sesionorientaciondto();
            if (sesionOrientacion != null)
            {
                dto.sesionorientacionid = sesionOrientacion.SesionOrientacionID;
                dto.inicio = sesionOrientacion.Inicio;
                dto.fin = sesionOrientacion.Fin;
                dto.fecha = sesionOrientacion.Fecha;
                dto.asistenciaaspirante = Convert.ToInt32(sesionOrientacion.AsistenciaAspirante);
                dto.asistenciaorientador = Convert.ToInt32(sesionOrientacion.AsistenciaOrientador);
                dto.cantidadhoras = sesionOrientacion.CantidadHoras;
                dto.alumnoid = sesionOrientacion.AlumnoID;
                dto.docenteid = sesionOrientacion.DocenteID;
                dto.estatussesion = Convert.ToInt32(sesionOrientacion.EstatusSesion);
                dto.encuestacontestada = Convert.ToInt32(sesionOrientacion.EncuestaContestada);
                dto.horafinalizada = sesionOrientacion.HoraFinalizado;

                dto.Success = "Dato cargado correctamente.";
                dto.Error = "";
            }
            else
            {
                dto.Success = "";
                dto.Error = "Error al cargar el datos.";
            }
            return dto;
        }

        // metodo para consultar las sesiones de orientacion pendientes del docente
        public List<sesionorientaciondto> GetSesionOrientacionDocente(sesionorientaciondto dto)
        {
            List<sesionorientaciondto> listaSesiones = new List<sesionorientaciondto>();
            try
            {
                sesionorientacion.DocenteID = userSession.CurrentDocente.DocenteID;
                List<SesionOrientacion> sesionResult = sesionOrientacionCtrl.Retrieve(sesionorientacion, true);
                foreach (SesionOrientacion sesion in sesionResult)
                {
                     DateTime Fecha = Convert.ToDateTime(sesion.Fecha);
                    DateTime Today = Convert.ToDateTime(DateTime.Now);
                    // verifcar que la sesion no este iniciada ni finalizada
                    // verficar que sea de la fecha de actual
                    if (Fecha.Day == Today.Day) {
                        sesionorientaciondto sov = SesionOrientacionToDTO(sesion);
                        listaSesiones.Add(sov);
                    }
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }

            return listaSesiones;
        }

        // metodo para actualizar el estatus de la sesion y presentarse a la misma
        public sesionorientaciondto UpdateSesionOrientacion(sesionorientaciondto dto) 
        {
            try
            {
                var objeto = new object();
                var contexto = new Contexto(objeto);
                sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);

                if (dto.sesionorientacionid != null) sesionorientacion.SesionOrientacionID = dto.sesionorientacionid;
                var result = sesionOrientacionCtrl.Retrieve(sesionorientacion, true).FirstOrDefault();
                if (result != null) sesionorientacion = result;
                if (dto.alumnoid != null)
                {
                    if (sesionorientacion.AlumnoID == dto.alumnoid)
                    {
                        sesionorientacion.AsistenciaAspirante = true;
                    }
                }

                if (dto.docenteid != null)
                {
                    if (sesionorientacion.DocenteID == dto.docenteid)
                    {
                        sesionorientacion.AsistenciaOrientador = true;
                    }
                }

                

                if (result != null) 
                {
                    sesionOrientacionCtrl.Update(sesionorientacion);
                }

                var resultUpd = sesionOrientacionCtrl.Retrieve(sesionorientacion, true).FirstOrDefault();
                if (resultUpd != null)
                {
                    if (resultUpd.AsistenciaAspirante == true && resultUpd.AsistenciaOrientador == true)
                    {
                        sesionorientacion.EstatusSesion = ESesionOrientacion.ENCURSO;
                        sesionOrientacionCtrl.Update(sesionorientacion);
                        
                    }
                }
                contexto.Commit(objeto);
                contexto.Dispose();
                dto.Success = "La sesion se ha actualizado correctamente";

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intentelo más tarde";
                return dto;
            }
        }

        // metodo para finalizar la sesion
        public sesionorientaciondto FinalizarSesionOrientacion(sesionorientaciondto dto)
        {
            try
            {
                var objeto = new object();
                var contexto = new Contexto(objeto);
                sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);
                eventCalendarCtrl=new EventCalendarCtrl(contexto);

                if (dto.sesionorientacionid != null) sesionorientacion.SesionOrientacionID = dto.sesionorientacionid;
                var result = sesionOrientacionCtrl.Retrieve(sesionorientacion, true).FirstOrDefault();
                if (result != null) sesionorientacion = result;

                sesionorientacion.EstatusSesion = ESesionOrientacion.FINALIZADO;
                sesionorientacion.HoraFinalizado = dto.horafinalizada;

                eventcalendar.Fecha = result.Fecha;
                eventcalendar.HrsInicio = result.Inicio;
                eventcalendar.HrsFin = result.Fin;
                eventcalendar.AlumnoID = result.AlumnoID;
                eventcalendar.CantidadHoras = result.CantidadHoras;
                var eventResult = eventCalendarCtrl.Retrieve(eventcalendar, true).FirstOrDefault();
                if (result.AsistenciaOrientador == false)
                    sesionorientacion.EncuestaContestada = true;

                if (result != null)
                {
                    sesionOrientacionCtrl.Update(sesionorientacion);
                }

                if (sesionorientacion.AsistenciaOrientador == false && sesionorientacion.AsistenciaAspirante == true)
                {
                    dto.asistenciaorientador = 0;
                }
                contexto.Commit(objeto);
                contexto.Dispose();
                
                dto.Success = "La sesion se ha finalizado correctamente";

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se finalizaba la sesión, intentelo más tarde";
                return dto;
            }
        }

        public respuestaencuestadto GuardarEncuestaSatisfaccion(respuestaencuestadto dto)
        {
            try
            {
                var objeto = new object();
                var contexto = new Contexto(objeto);
                sesionOrientacionCtrl = new SesionOrientacionCtrl(contexto);
                encuestaSatisfaccionCtrl = new EncuestaSatisfaccionCtrl(contexto);

                if (dto.sesionorientacionid != null) sesionorientacion.SesionOrientacionID = dto.sesionorientacionid;
                var result = sesionOrientacionCtrl.Retrieve(sesionorientacion, true).FirstOrDefault();
                if (result != null) sesionorientacion = result;
                sesionorientacion.EncuestaContestada = true;

                if (result != null)
                {
                    encuestasatisfaccion.PreguntaUno = dto.preguntauno;
                    encuestasatisfaccion.RespuestaUno = dto.respuestauno;

                    encuestasatisfaccion.PreguntaDos = dto.preguntados;
                    encuestasatisfaccion.RespuestaDos = dto.respuestados;

                    encuestasatisfaccion.PreguntaTres = dto.preguntatres;
                    encuestasatisfaccion.RespuestaTres = dto.respuestatres;

                    encuestasatisfaccion.PreguntaCuatro = dto.preguntacuatro;
                    encuestasatisfaccion.RespuestaCuatro = dto.respuestacuatro;

                    encuestasatisfaccion.PreguntaCinco = dto.preguntacinco;
                    encuestasatisfaccion.RespuestaCinco = dto.respuestacinco;

                    encuestasatisfaccion.AlumnoID = dto.alumnoid;
                    encuestasatisfaccion.DocenteID = dto.docenteid;
                    encuestasatisfaccion.SesionOrientacionID = dto.sesionorientacionid;

                    encuestaSatisfaccionCtrl.Insert(encuestasatisfaccion);
                    sesionOrientacionCtrl.Update(sesionorientacion);
                    dto.Success = "La sesion se ha finalizado correctamente";
                    dto.Error = "";
                }

                contexto.Commit(objeto);
                contexto.Dispose();

                return dto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.Success = "";
                dto.Error = "Ha ocurrido un error finalizava la sesión, intente más tarde";
                return dto;
            }
        }
    }
}
